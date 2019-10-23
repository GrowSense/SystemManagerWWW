using System;
using uPLibrary.Networking.M2Mqtt;
using GrowSense.SystemManager.Devices;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Collections.Generic;

namespace GrowSense.SystemManager.Mqtt
{
  public class DeviceMqttListener
  {
    public DeviceInfo[] Devices;
    MqttClient Client;
    public Dictionary<string, Dictionary<string, string>> Data = new Dictionary<string, Dictionary<string, string>> ();
    //public event MqttClient.MqttMsgPublishEventHandler MqttMsgPublishReceived;
    public DeviceMqttListener (DeviceInfo[] devicesInfo)
    {
      Devices = devicesInfo;
    }

    public void Connect (string clientId, string mqttHost, string mqttUsername, string mqttPassword, int mqttPort)
    {
      Client = new MqttClient (mqttHost, mqttPort, false, null, null, MqttSslProtocols.None);
      Client.MqttMsgPublishReceived += HandleMqttMsgPublishReceived;

      Client.Connect (clientId, mqttUsername, mqttPassword);
      
      foreach (var deviceInfo in Devices) {
        Client.Subscribe (new string[] { "/" + deviceInfo.Name + "/#" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
      }
    }

    void HandleMqttMsgPublishReceived (object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
    {
      var topic = e.Topic;
      
      var value = System.Text.Encoding.UTF8.GetString (e.Message);
      
      var topicParts = topic.Split ('/');
      
      var deviceName = topicParts [1];
      
      var topicKey = topicParts [2];
      
      if (!Data.ContainsKey (deviceName))
        Data.Add (deviceName, new Dictionary<string, string> ());
      
      Data [deviceName] [topicKey] = value;
    }

    public int GetInt32 (string topicKey)
    {
      //var mqtt = new Mqtt
      
      return 0;
    }
  }
}

