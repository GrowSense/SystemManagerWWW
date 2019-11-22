using System;
using System.Web;
using GrowSense.SystemManager.Devices;

namespace GrowSense.SystemManager.Web
{
  public class DeviceWebUtility
  {
    public DeviceManager Manager;

    public DeviceWebUtility (DeviceManager manager)
    {
      Manager = manager;
    }

    public void EnsureDeviceNameProvided (string deviceName)
    {
      if (String.IsNullOrEmpty (deviceName)) {
        var message = "No device name specified in the query string!";
        RedirectToDevicesPage (message, true);
      }
    }

    public void EnsureDeviceExists (string deviceName)
    {
      if (!Manager.DeviceExists (deviceName)) {
        var message = deviceName + " not found!";
        RedirectToDevicesPage (message, true);
      }
    }

    public void EnsureDeviceDataExists (string deviceName)
    {
      if (!DeviceMqttHolder.Current.Data.ContainsKey (deviceName)) {
        var message = "No MQTT data found for " + deviceName + " device!";
        RedirectToDevicesPage (message, true);
      }
    }

    public void EnsureDeviceDataExists (string deviceName, string key)
    {
      EnsureDeviceDataExists (deviceName);
    
      if (!DeviceMqttHolder.Current.Data [deviceName].ContainsKey (key)) {
        var message = "No MQTT data found for " + deviceName + " device with key '" + key + "'!";
        RedirectToDevicesPage (message, true);
      }
    }

    public string GetDeviceData (string deviceName, string key)
    {
      EnsureDeviceDataExists (deviceName, key);
    
      return DeviceMqttHolder.Current.Data [deviceName] [key];
    }

    public void Redirect (string url)
    {
      HttpContext.Current.Response.Redirect (url);
    }

    public void RedirectToDevicesPage (string result, bool isError)
    {
      Redirect ("Devices.aspx?Result=" + HttpUtility.UrlEncode (result) + "&IsError=" + isError.ToString ());
    }
  }
}

