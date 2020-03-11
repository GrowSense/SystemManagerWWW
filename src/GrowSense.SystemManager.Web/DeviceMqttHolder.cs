using System;
using GrowSense.SystemManager.Mqtt;
using GrowSense.SystemManager.Devices;
using System.IO;
using System.Configuration;
using System.Web.Configuration;

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
      
        var indexDirectory = Path.GetFullPath (WebConfigurationManager.AppSettings ["IndexDirectory"]);
    
        var devicesDirectory = Path.GetFullPath (WebConfigurationManager.AppSettings ["DevicesDirectory"]);
     
        var deviceManager = new DeviceManager (indexDirectory, devicesDirectory);
      
        var mqttDeviceName = WebConfigurationManager.AppSettings ["MqttDeviceName"] + "-" + Guid.NewGuid ().ToString ();
        var mqttHost = WebConfigurationManager.AppSettings ["MqttHost"];
        var mqttUsername = WebConfigurationManager.AppSettings ["MqttUsername"];
        var mqttPassword = WebConfigurationManager.AppSettings ["MqttPassword"];
        var mqttPort = Convert.ToInt32 (WebConfigurationManager.AppSettings ["MqttPort"]);
      
        Current = new DeviceMqtt (deviceManager, mqttDeviceName, mqttHost, mqttUsername, mqttPassword, mqttPort);
      }
      
      if (!Current.IsConnected && !Current.IsConnecting)
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

