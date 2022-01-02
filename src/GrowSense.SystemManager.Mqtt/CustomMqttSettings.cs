using System;
using uPLibrary.Networking.M2Mqtt.Session;
using uPLibrary.Networking.M2Mqtt;
namespace GrowSense.SystemManager.Mqtt
{
  public class CustomMqttSettings : MqttSettings
  {
  public int TimeoutOnConnection { get; set; }
  
    public CustomMqttSettings() : base(
    {
    }
  }
}
