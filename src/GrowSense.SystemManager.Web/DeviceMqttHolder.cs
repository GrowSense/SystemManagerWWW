using System;
using GrowSense.SystemManager.Mqtt;
using GrowSense.SystemManager.Devices;
using System.IO;
using System.Configuration;
using System.Web.Configuration;
using GrowSense.SystemManager.Common;

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
        
        var settingsManager = new SettingsManager(indexDirectory);
        var settings = settingsManager.LoadSettings();
      
        Current = new DeviceMqtt (deviceManager, mqttDeviceName, settings.MqttHost, settings.MqttUsername, settings.MqttPassword, settings.MqttPort);
      }
      
      Connect ();
    }

    static public void Connect ()
    {
    // TODO: Clean up
      //if (Current != null && !Current.IsConnected) {
        Current.Connect ();
      //}
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

