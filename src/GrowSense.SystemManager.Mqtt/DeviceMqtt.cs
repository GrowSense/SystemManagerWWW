using System;
using uPLibrary.Networking.M2Mqtt;
using GrowSense.SystemManager.Devices;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Collections.Generic;
using System.Text;

namespace GrowSense.SystemManager.Mqtt
{
  public class DeviceMqtt
  {
    public DeviceInfo[] Devices;
    MqttClient Client;
    public Dictionary<string, Dictionary<string, string>> Data = new Dictionary<string, Dictionary<string, string>> ();

    public DeviceMqtt (DeviceInfo[] devicesInfo)
    {
      Devices = devicesInfo;
    }

    public void Connect (string clientId, string mqttHost, string mqttUsername, string mqttPassword, int mqttPort)
    {
      Client = new MqttClient (mqttHost, mqttPort, false, null, null, MqttSslProtocols.None);
      Client.MqttMsgPublishReceived += HandleMqttMsgPublishReceived;

      Client.Connect (clientId, mqttUsername, mqttPassword);
      
      foreach (var deviceInfo in Devices) {
        Client.Subscribe (new string[] { deviceInfo.Name + "/#" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
      }
    }

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
    }

    public void Publish (string deviceName, string topicKey, int value)
    {
      Publish (deviceName, topicKey, value.ToString ());
    }

    public void Publish (string deviceName, string topicKey, string value)
    {
      Client.Publish (deviceName + "/" + topicKey + "/in", Encoding.UTF8.GetBytes (value), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
    }
  }
}

