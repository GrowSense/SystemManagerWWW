using System;
using uPLibrary.Networking.M2Mqtt;
using GrowSense.SystemManager.Devices;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Threading;

namespace GrowSense.SystemManager.Mqtt
{
  public class DeviceMqtt : IDisposable
  {
    public DeviceInfo[] Devices = new DeviceInfo[] { };
    public MqttClient Client;
    public Dictionary<string, Dictionary<string, string>> Data = new Dictionary<string, Dictionary<string, string>>();
    public DeviceManager Manager;
    public FileSystemWatcher DevicesWatcher;

    public string ClientId;
    public string MqttHost;
    public string MqttUsername;
    public string MqttPassword;
    public int MqttPort;

    public bool IsDisposing = false;

    public bool IsConnecting = false;

    public DateTime ConnectingStartTime;
    public int ConnectionTimeoutInSeconds = 20;

    public bool IsConnected
    {
      get
      {
        if (Client == null)
          return false;
        return Client.IsConnected;
      }
    }

    public DeviceMqtt(DeviceManager manager, string clientId, string mqttHost, string mqttUsername, string mqttPassword, int mqttPort)
    {
      Manager = manager;

      ClientId = clientId;
      MqttHost = mqttHost;
      MqttUsername = mqttUsername;
      MqttPassword = mqttPassword;
      MqttPort = mqttPort;

    }
    #region Connect
    public void Connect()
    {
      if ((!IsConnected && !IsConnecting) || HasConnectionTimedOut())
      {
        IsConnecting = true;
        ConnectingStartTime = DateTime.Now;
        
        Console.WriteLine("Connecting to MQTT broker...");

        Client = new MqttClient(MqttHost, MqttPort, false, null, null, MqttSslProtocols.None);
        Client.MqttMsgPublishReceived += HandleMqttMsgPublishReceived;
        Client.ConnectionClosed += HandleConnectionClosed;
        

        var b = Client.Connect(ClientId, MqttUsername, MqttPassword, true, 10000);

        if (b == 0 && Client.IsConnected)
        {
          Console.WriteLine("  Connected to MQTT");
          Client.Subscribe(new string[] { "garden/#" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

          AddDevices(Manager.GetDevicesInfo());

          WatchDevicesFolder();

          IsConnecting = false;
        }
        else
        {
          Console.WriteLine("  Failed to connect to MQTT");

          if (b == 1)
            throw new Exception("MQTT connection error: Unacceptable protocol version.");
          if (b == 2)
            throw new Exception("MQTT connection error: Identifier rejected.");
          if (b == 3)
            throw new Exception("MQTT connection error: Server unavailable.");
          if (b == 4)
            throw new Exception("MQTT connection error: Bad username or password.");
          if (b == 5)
            throw new Exception("MQTT connection error: Not authorized.");
        }
      }
      else
        Console.WriteLine("Is already connecting. Skipping connection for this thread.");
    }

    void HandleConnectionClosed(object sender, EventArgs e)
    {
      if (!IsDisposing)
      {
        Console.WriteLine("MQTT connection closed. Reconnecting...");
        Connect();
      }
    }

    bool HasConnectionTimedOut()
    {
      var hasTimedOut = IsConnecting && ConnectingStartTime.AddSeconds(ConnectionTimeoutInSeconds) < DateTime.Now; ;

      if (hasTimedOut)
        Console.WriteLine("  Timed out while connecting");

      return hasTimedOut;
    }
    #endregion
    #region Handle MQTT Message
    void HandleMqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
    {
      var topic = e.Topic;

      var value = System.Text.Encoding.UTF8.GetString(e.Message);

      var topicParts = topic.Split('/');

      var deviceName = topicParts[0];

      var topicKey = topicParts[1];

      if (!Data.ContainsKey(deviceName))
        Data.Add(deviceName, new Dictionary<string, string>());

      Data[deviceName][topicKey] = value;

      if (deviceName == "garden")
        HandleGardenStatusMessage(topicKey, value);
    }

    public void HandleGardenStatusMessage(string topicKey, string value)
    {
      // TODO: Remove if not needed. Disabled because it's likely to cause problems. Device info needs to be pulled via the supervisor script.
      /*if (topicKey == "StatusMessage") {
        if (value.Contains ("Detected")) {
          RefreshDevices ();
        }
      }*/
    }
    #endregion
    #region Subscribe to MQTT Functions
    public void SubscribeToDeviceData()
    {
      foreach (var deviceInfo in Devices)
      {
        SubscribeToDeviceData(deviceInfo.Name);
      }
    }

    public void SubscribeToDeviceData(string deviceName)
    {
      Client.Subscribe(new string[] { deviceName + "/#" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
    }

    public void UnsubscribeFromDeviceData(string deviceName)
    {
      Client.Unsubscribe(new string[] { deviceName + "/#" });
    }
    #endregion
    #region Device Functions
    public void RefreshDevices()
    {
      var latestDevices = Manager.GetDevicesInfo();
      foreach (var device in latestDevices)
      {
        if (!DeviceIsInList(device.Name))
          AddDevice(device);
      }
    }

    public bool DeviceIsInList(string deviceName)
    {
      foreach (var device in Devices)
      {
        if (device.Name == deviceName)
          return true;
      }
      return false;
    }

    public void AddDevices(DeviceInfo[] devicesInfo)
    {
      foreach (var deviceInfo in devicesInfo)
        AddDevice(deviceInfo);
    }

    public void AddDevice(DeviceInfo deviceInfo)
    {
      if (!DeviceExists(deviceInfo.Name))
      {
        var list = new List<DeviceInfo>();
        if (Devices.Length > 0)
          list.AddRange(Devices);
        list.Add(deviceInfo);
        Devices = list.ToArray();

        SubscribeToDeviceData(deviceInfo.Name);
      }
    }

    public void RemoveDevice(string deviceName)
    {
      if (DeviceExists(deviceName))
      {
        var list = new List<DeviceInfo>();
        if (Devices.Length > 0)
          list.AddRange(Devices);

        for (int i = 0; i < list.Count; i++)
        {
          if (list[i].Name == deviceName)
            list.RemoveAt(i);
        }
        Devices = list.ToArray();

        UnsubscribeFromDeviceData(deviceName);
      }
    }

    public bool DeviceExists(string deviceName)
    {
      foreach (var deviceInfo in Devices)
      {
        if (deviceInfo.Name == deviceName)
          return true;
      }
      return false;
    }

    public void RenameDevice(string originalName, string newName)
    {
      var dataEntry = Data[originalName];
      RemoveDevice(originalName);
      AddDevice(Manager.GetDeviceInfo(newName));
      Data[newName] = dataEntry;
    }
    #endregion
    #region Device Watcher Functions
    public void WatchDevicesFolder()
    {
      DevicesWatcher = new FileSystemWatcher(Manager.DevicesDirectory);
      DevicesWatcher.Created += HandleDeviceCreated;
      DevicesWatcher.Deleted += HandleDeviceDeleted;
      DevicesWatcher.EnableRaisingEvents = true;
    }

    void HandleDeviceDeleted(object sender, FileSystemEventArgs e)
    {
      var deviceName = Path.GetFileName(e.FullPath);
      RemoveDevice(deviceName);
    }

    void HandleDeviceCreated(object sender, FileSystemEventArgs e)
    {
      var deviceName = Path.GetFileName(e.FullPath);
      AddDevice(Manager.GetDeviceInfo(deviceName));
    }
    #endregion
    #region Publish Functions
    public void Publish(string deviceName, string topicKey, int value)
    {
      Publish(deviceName, topicKey, value.ToString());
    }

    public void Publish(string deviceName, string topicKey, string value)
    {
      Client.Publish(deviceName + "/" + topicKey + "/in", Encoding.UTF8.GetBytes(value), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
    }
    #endregion
    #region IDisposable implementation
    public void Dispose()
    {
      IsDisposing = true;

      if (Client != null)
        Client.Disconnect();
      if (DevicesWatcher != null)
        DevicesWatcher.Dispose();
    }
    #endregion
  }
}