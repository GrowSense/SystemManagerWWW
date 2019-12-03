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
     
        var deviceManager = new DeviceManager (indexDirectory, devicesDirectory);
      
        var mqttDeviceName = ConfigurationSettings.AppSettings ["MqttDeviceName"] + "-" + Guid.NewGuid ().ToString ();
        var mqttHost = ConfigurationSettings.AppSettings ["MqttHost"];
        var mqttUsername = ConfigurationSettings.AppSettings ["MqttUsername"];
        var mqttPassword = ConfigurationSettings.AppSettings ["MqttPassword"];
        var mqttPort = Convert.ToInt32 (ConfigurationSettings.AppSettings ["MqttPort"]);
      
        Current = new DeviceMqtt (deviceManager, mqttDeviceName, mqttHost, mqttUsername, mqttPassword, mqttPort);
      }
      
      Connect ();
    }

    static public void Connect ()
    {
      if (Current != null && !Current.IsConnected) {
        Current.Connect ();
      }
    }

    static public void EnsureConnected ()
    {
      Connect ();
    }

    static public void End ()
    {
      if (Current != null)
        Current.Dispose ();
    }
  }
}

