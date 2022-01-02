using System;
using GrowSense.SystemManager.Common;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Text;
using System.Threading;
using System.Net.NetworkInformation;

namespace GrowSense.SystemManager.Mqtt
{
  public class MqttConnectionTester
  {
    public bool ResponseReceived = false;
    public string ResponseValue = "";
    public string ResponseTopic = "";

    public MqttConnectionTester()
    {
    }


    public bool IsValid(SystemSettings settings)
    {
      var ping = new Ping();
      var pingResult = ping.Send(settings.MqttHost);

      if (pingResult.Status != IPStatus.Success)
        return false;
        
      var rand = new Random();

      var testTopic = "test/msg" + rand.Next(10000, 99999);
      var testClientId = "client" + rand.Next(10000, 99999);

      //var mqttClientSettings = MqttSettings.
      //mqttClientSettings.TimeoutOnConnection = 2000;
      var client = new MqttClient(settings.MqttHost, settings.MqttPort, false, null, null, MqttSslProtocols.None);
      //client.
      //client.Settings. = new MqttSettings
     // {
      //Timeout=5000
      //};
      client.MqttMsgPublishReceived += HandleMqttMsgPublishReceived;
      //client.ConnectionClosed += HandleConnectionClosed;
      //client.
      try
      {
        client.Connect(testClientId, settings.MqttUsername, settings.MqttPassword, true, 1000);
      }
      catch (Exception ex)
      {
        return false;
      }

      if (!client.IsConnected)
        return false;

      client.Subscribe(new string[] { testTopic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

      var testValue = "value" + rand.Next(10000, 99999);

      client.Publish(testTopic, Encoding.ASCII.GetBytes(testValue), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);

      Thread.Sleep(2000);

      client.Disconnect();

      return ResponseReceived && ResponseValue == testValue && ResponseTopic == testTopic;
    }

    void HandleMqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
    {
      var topic = e.Topic;

      var value = System.Text.Encoding.UTF8.GetString(e.Message);

      ResponseReceived = true;

      ResponseValue = value;

      ResponseTopic = topic;

    }
  }
}
