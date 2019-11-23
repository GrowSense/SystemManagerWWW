using System;
using uPLibrary.Networking.M2Mqtt;
using GrowSense.SystemManager.Devices;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel;

namespace GrowSense.SystemManager.Mqtt
{
  public class DeviceMqtt : IDisposable
  {
    public DeviceInfo[] Devices = new DeviceInfo[] { };
    MqttClient Client;
    public Dictionary<string, Dictionary<string, string>> Data = new Dictionary<string, Dictionary<string, string>> ();
    public DeviceManager Manager;
    public FileSystemWatcher DevicesWatcher;

    public DeviceMqtt (DeviceManager manager)
    {
      Manager = manager;
    }
    #region Connect
    public void Connect (string clientId, string mqttHost, string mqttUsername, string mqttPassword, int mqttPort)
    {
      Client = new MqttClient (mqttHost, mqttPort, false, null, null, MqttSslProtocols.None);
      Client.MqttMsgPublishReceived += HandleMqttMsgPublishReceived;

      Client.Connect (clientId, mqttUsername, mqttPassword);
      
      Client.Subscribe (new string[] { "garden/#" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
      
      AddDevices (Manager.GetDevicesInfo ());
      
      WatchDevicesFolder ();
    }
    #endregion
    #region Handle MQTT Message
    void HandleMqttMsgPublishReceived (object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
    {
      var topic = e.Topic;
      
      var value = System.Text.Encoding.UTF8.GetString (e.Message);
      
      var topicParts = topic.Split ('/');
      
      var deviceName = topicParts [0];
      
      var topicKey = topicParts [1];
      
      if (!Data.ContainsKey (deviceName))
        Data.Add (deviceName, new Dictionary<string, string> ());
      
      Data [deviceName] [topicKey] = value;
      
      if (deviceName == "garden")
        HandleGardenStatusMessage (topicKey, value);
    }

    public void HandleGardenStatusMessage (string topicKey, string value)
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
    public void SubscribeToDeviceData ()
    {
      foreach (var deviceInfo in Devices) {
        SubscribeToDeviceData (deviceInfo.Name);
      }
    }

    public void SubscribeToDeviceData (string deviceName)
    {
      Client.Subscribe (new string[] { deviceName + "/#" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
    }

    public void UnsubscribeFromDeviceData (string deviceName)
    {
      Client.Unsubscribe (new string[] { deviceName + "/#" });
    }
    #endregion
    #region Device Functions
    public void RefreshDevices ()
    {
      var latestDevices = Manager.GetDevicesInfo ();
      foreach (var device in latestDevices) {
        if (!DeviceIsInList (device.Name))
          AddDevice (device);
      }
    }

    public bool DeviceIsInList (string deviceName)
    {
      foreach (var device in Devices) {
        if (device.Name == deviceName)
          return true;
      }
      return false;
    }

    public void AddDevices (DeviceInfo[] devicesInfo)
    {
      foreach (var deviceInfo in devicesInfo)
        AddDevice (deviceInfo);
    }

    public void AddDevice (DeviceInfo deviceInfo)
    {
      if (!DeviceExists (deviceInfo.Name)) {
        var list = new List<DeviceInfo> ();
        if (Devices.Length > 0)
          list.AddRange (Devices);
        list.Add (deviceInfo);
        Devices = list.ToArray ();
      
        SubscribeToDeviceData (deviceInfo.Name);
      }
    }

    public void RemoveDevice (string deviceName)
    {
      if (DeviceExists (deviceName)) {
        var list = new List<DeviceInfo> ();
        if (Devices.Length > 0)
          list.AddRange (Devices);
        
        for (int i = 0; i < list.Count; i++) {
          if (list [i].Name == deviceName)
            list.RemoveAt (i);
        }
        Devices = list.ToArray ();
      
        UnsubscribeFromDeviceData (deviceName);
      }
    }

    public bool DeviceExists (string deviceName)
    {
      foreach (var deviceInfo in Devices) {
        if (deviceInfo.Name == deviceName)
          return true;
      }
      return false;
    }
    #endregion
    #region Device Watcher Functions
    public void WatchDevicesFolder ()
    {
      DevicesWatcher = new FileSystemWatcher (Manager.DevicesDirectory);
      DevicesWatcher.Created += HandleDeviceCreated;
      DevicesWatcher.Deleted += HandleDeviceDeleted;
      DevicesWatcher.EnableRaisingEvents = true;
    }

    void HandleDeviceDeleted (object sender, FileSystemEventArgs e)
    {
      var deviceName = Path.GetFileName (e.FullPath);
      RemoveDevice (deviceName);
    }

    void HandleDeviceCreated (object sender, FileSystemEventArgs e)
    {
      var deviceName = Path.GetFileName (e.FullPath);
      AddDevice (Manager.GetDeviceInfo (deviceName));
    }
    #endregion
    #region Publish Functions
    public void Publish (string deviceName, string topicKey, int value)
    {
      Publish (deviceName, topicKey, value.ToString ());
    }

    public void Publish (string deviceName, string topicKey, string value)
    {
      Client.Publish (deviceName + "/" + topicKey + "/in", Encoding.UTF8.GetBytes (value), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
    }
    #endregion
    #region IDisposable implementation
    public void Dispose ()
    {
      Client.Disconnect();
      DevicesWatcher.Dispose ();
    }
    #endregion
  }
}

