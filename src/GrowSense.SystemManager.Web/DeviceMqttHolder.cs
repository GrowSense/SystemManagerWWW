using System;
using GrowSense.SystemManager.Mqtt;
using GrowSense.SystemManager.Devices;
using System.IO;
using System.Configuration;

namespace GrowSense.SystemManager.Web
{
  public class DeviceMqttHolder
  {
    static public DeviceMqtt Current;

    public DeviceMqttHolder ()
    {
    }

    static public void Initialize ()
    {
      if (Current == null) {
      
        var indexDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["IndexDirectory"]);
    
        var devicesDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["DevicesDirectory"]);
    
        Current = new DeviceMqtt (new DeviceManager (indexDirectory, devicesDirectory));
      }
      
      if (!Current.IsConnected) {
        var mqttDeviceName = ConfigurationSettings.AppSettings ["MqttDeviceName"] + "-" + Guid.NewGuid ().ToString ();
        var mqttHost = ConfigurationSettings.AppSettings ["MqttHost"];
        var mqttUsername = ConfigurationSettings.AppSettings ["MqttUsername"];
        var mqttPassword = ConfigurationSettings.AppSettings ["MqttPassword"];
        var mqttPort = Convert.ToInt32 (ConfigurationSettings.AppSettings ["MqttPort"]);
      
        Current.Connect (mqttDeviceName, mqttHost, mqttUsername, mqttPassword, mqttPort);
      }
    }

    static public void End ()
    {
      if (Current != null)
        Current.Dispose ();
    }
  }
}

