using System;
using GrowSense.SystemManager.Mqtt;
using GrowSense.SystemManager.Devices;
using System.IO;
using System.Configuration;

namespace GrowSense.SystemManager.Web
{
  public class DeviceMqttListenerHolder
  {
    static public DeviceMqttListener Current;

    public DeviceMqttListenerHolder ()
    {
    }

    static public void Initialize ()
    {
      var mqttDeviceName = ConfigurationSettings.AppSettings ["MqttDeviceName"];
      var mqttHost = ConfigurationSettings.AppSettings ["MqttHost"];
      var mqttUsername = ConfigurationSettings.AppSettings ["MqttUsername"];
      var mqttPassword = ConfigurationSettings.AppSettings ["MqttPassword"];
      var mqttPort = Convert.ToInt32 (ConfigurationSettings.AppSettings ["MqttPort"]);
      
      var devicesDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["DevicesDirectory"]);
    
      Current = new DeviceMqttListener (new DeviceManager (devicesDirectory).GetDevicesInfo ());
      Current.Connect (mqttDeviceName, mqttHost, mqttUsername, mqttPassword, mqttPort);
    }
  }
}

