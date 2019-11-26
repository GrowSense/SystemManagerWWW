using System.Security.Cryptography.X509Certificates;
using System.IO;
using GrowSense.SystemManager.Devices;
using GrowSense.SystemManager.Web;
using System.Configuration;

namespace GrowSense.SystemManager
{
  using System;
  using System.Web;
  using System.Web.UI;

  public partial class DevicesPage : System.Web.UI.Page
  {
    public DeviceInfo[] DevicesInfo = new DeviceInfo[] { };

    public void Page_Load (object sender, EventArgs e)
    {
      LoadDevicesInfo ();
    }

    public void LoadDevicesInfo ()
    {
      var indexDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["IndexDirectory"]);
    
      var devicesDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["DevicesDirectory"]);
    
      var deviceManager = new DeviceManager (indexDirectory, devicesDirectory);
      DevicesInfo = deviceManager.GetDevicesInfo ();
    }
    
    public string GetDeviceData (string deviceName, string topicKey)
    {
      if (!DeviceMqttHolder.Current.Data.ContainsKey (deviceName))
        return 0.ToString(); // throw new Exception ("Device not found in DeviceMqttHolder.Current.Data: " + deviceName);
      if (!DeviceMqttHolder.Current.Data [deviceName].ContainsKey (topicKey))
        return 0.ToString(); // throw new Exception ("Topic key not found in DeviceMqttHolder.Current.Data[" + deviceName + "]: " + topicKey);
      return DeviceMqttHolder.Current.Data [deviceName] [topicKey];
    }
    
    public string GenerateDeviceStatusIcon (string deviceName)
    {
      var statusText = GetDeviceData(deviceName, "StatusMessage");
    
      if (statusText == "0")
        statusText = "Unknown";
    
      var cssClass = "label-info";
    
      if (statusText == "Online")
        cssClass = "label-success";
      if (statusText == "Connected")
        cssClass = "label-success";
      if (statusText == "Offline")
        cssClass = "label-danger";
      if (statusText == "Upgrading")
        cssClass = "label-info";
      if (statusText == "Upgrade Complete")
        cssClass = "label-info";
      if (statusText == "Unknown")
        cssClass = "label-info";
    
      return String.Format("<div class=\"label {0} label-mini\">{1}</div>",
        cssClass,
        statusText
        );
    }
    
    public string GenerateDeviceProgressBars (DeviceInfo device)
    {
      if (device.Group == "ui") {
        var version = GetDeviceData (device.Name, "V");
        return "Version: " + version;
      }
      if (device.Group == "irrigator") {
        var value = GetDeviceData (device.Name, "C");
        return GenerateDeviceProgressBar ("Soil Moisture: " + value + "%", value, "blue");
      }
      if (device.Group == "illuminator") {
        var value = GetDeviceData (device.Name, "L");
        return GenerateDeviceProgressBar ("Light: " + value + "%", value, "yellow");
      }
      if (device.Group == "ventilator") {
        var temperatureValue = GetDeviceData (device.Name, "T");
        var humidityValue = GetDeviceData (device.Name, "H");
        return GenerateDeviceProgressBar ("Temperature: " + temperatureValue + "c", temperatureValue, "red") +
          GenerateDeviceProgressBar ("Humidity: " + humidityValue + "%", humidityValue, "blue");
      }
      if (device.Group == "monitor") {
        if (device.Project.StartsWith("SoilMoisture")) {
          var value = GetDeviceData (device.Name, "C");
          return GenerateDeviceProgressBar ("Soil Moisture: " + value + "%", value, "blue");
        }
        if (device.Project.StartsWith("Light")) {
          var value = GetDeviceData (device.Name, "L");
          return GenerateDeviceProgressBar ("Light: " + value + "%", value, "yellow");
        }
        if (device.Project.StartsWith("TemperatureHumidity")) {
          var temperatureValue = GetDeviceData (device.Name, "T");
          var humidityValue = GetDeviceData (device.Name, "H");
          return GenerateDeviceProgressBar ("Temperature: " + temperatureValue + "c", temperatureValue, "red") +
            GenerateDeviceProgressBar ("Humidity: " + humidityValue + "%", humidityValue, "blue");
        }
      }
      return "Device type not implemented: " + device.Group + " " + device.Project;
    }
    
    public string GenerateDeviceProgressBar (string text, string value, string color)
    {
      var cssClass = "";
      
      if (color == "blue")
        cssClass = "progress-bar-info";
      if (color == "red")
        cssClass = "progress-bar-danger";
      if (color == "green")
        cssClass = "progress-bar-success";
      if (color == "yellow")
        cssClass = "progress-bar-warning";
    
      return String.Format(@"{1}
                      <div class=""progress"">
                        <div class=""progress-bar {2}"" role=""progressbar"" aria-valuenow=""{0}"" aria-valuemin=""0"" aria-valuemax=""100"" style=""width: {0}%"">
                          <span class=""sr-only"">{0}</span>
                        </div>
                      </div>",
                      value,
                      text,
                      cssClass
        );
    }
    
    public string GetDeviceEditLink (DeviceInfo deviceInfo)
    {
      var targetName = "";
      
      if (deviceInfo.Group == "irrigator")
        targetName = "Irrigator";
      if (deviceInfo.Group == "ventilator")
        targetName = "Ventilator";
      if (deviceInfo.Group == "illuminator")
        targetName = "Illuminator";
      if (deviceInfo.Group == "monitor") {
        if (deviceInfo.Project.StartsWith("SoilMoisture"))
          targetName = "SoilMoistureMonitor";
        if (deviceInfo.Project.StartsWith("TemperatureHumidity"))
          targetName = "TemperatureHumidityMonitor";
        if (deviceInfo.Project.StartsWith("Light"))
          targetName = "LightMonitor";
      }
    
      return "Edit" + targetName + ".aspx?DeviceName=" + deviceInfo.Name;
    }
  }
}

