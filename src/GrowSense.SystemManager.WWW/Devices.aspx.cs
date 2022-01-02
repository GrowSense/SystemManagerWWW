using System.Security.Cryptography.X509Certificates;
using System.IO;
using GrowSense.SystemManager.Devices;
using GrowSense.SystemManager.Web;
using System.Configuration;
using GrowSense.SystemManager.Computers;
using System.Collections.Generic;
using System.Web.Configuration;

namespace GrowSense.SystemManager
{
  using System;
  using System.Web;
  using System.Web.UI;

  public partial class DevicesPage : System.Web.UI.Page
  {
    public DeviceInfo[] DevicesInfo = new DeviceInfo[] { };
    public ComputerInfo[] ComputersInfo = new ComputerInfo[] {};
    
    public DeviceManager Manager;

    public void Page_Load (object sender, EventArgs e)
    {
      LoadDevicesInfo ();
      LoadComputersInfo ();
    }

    public void LoadDevicesInfo ()
    {
      var indexDirectory = Path.GetFullPath (WebConfigurationManager.AppSettings ["IndexDirectory"]);
    
      var devicesDirectory = Path.GetFullPath (WebConfigurationManager.AppSettings ["DevicesDirectory"]);
    
      Manager = new DeviceManager (indexDirectory, devicesDirectory);
      DevicesInfo = Manager.GetDevicesInfo ();
    }
    
    public void LoadComputersInfo ()
    {
      var indexDirectory = Path.GetFullPath (WebConfigurationManager.AppSettings ["IndexDirectory"]);
    
      var computersDirectory = Path.GetFullPath (WebConfigurationManager.AppSettings ["ComputersDirectory"]);
    
      var computerManager = new ComputerManager (indexDirectory, computersDirectory);
      ComputersInfo = computerManager.GetComputersInfo ();
    }
    
    public DeviceInfo[] GetDevicesInfo (ComputerInfo computerInfo)
    {
      var list = new List<DeviceInfo> ();
      foreach (var deviceInfo in DevicesInfo) {
        if (deviceInfo.Host == computerInfo.Host)
          list.Add (deviceInfo);
      }
      return list.ToArray ();
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
      if (statusText == "Disconnected")
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
        return GenerateDeviceProgressBar (device.Name, "moisture", "Soil Moisture: <span id='" + device.Name + "-moisture-value'>" + value + "</span>%", value, "blue");
      }
      if (device.Group == "illuminator") {
        var value = GetDeviceData (device.Name, "L");
        return GenerateDeviceProgressBar (device.Name, "light", "Light: <span id='" + device.Name + "-moisture-value'>" + value + "</span>%", value, "yellow");
      }
      if (device.Group == "ventilator") {
        var temperatureValue = GetDeviceData (device.Name, "T");
        var humidityValue = GetDeviceData (device.Name, "H");
        return GenerateDeviceProgressBar (device.Name, "temperature", "Temperature: " + temperatureValue + "c", temperatureValue, "red") +
          GenerateDeviceProgressBar (device.Name, "humidity", "Humidity: " + humidityValue + "%", humidityValue, "blue");
      }
      if (device.Group == "monitor") {
        if (device.Project.StartsWith("SoilMoisture")) {
          var value = GetDeviceData (device.Name, "C");
          return GenerateDeviceProgressBar (device.Name, "moisture", "Soil Moisture: " + value + "%", value, "blue");
        }
        if (device.Project.StartsWith("Light")) {
          var value = GetDeviceData (device.Name, "L");
          return GenerateDeviceProgressBar (device.Name, "light", "Light: " + value + "%", value, "yellow");
        }
        if (device.Project.StartsWith("TemperatureHumidity")) {
          var temperatureValue = GetDeviceData (device.Name, "T");
          var humidityValue = GetDeviceData (device.Name, "H");
          return GenerateDeviceProgressBar (device.Name, "temperature", "Temperature: " + temperatureValue + "c", temperatureValue, "red") +
            GenerateDeviceProgressBar (device.Name, "humidity", "Humidity: " + humidityValue + "%", humidityValue, "blue");
        }
      }
      return "Device type not implemented: " + device.Group + " " + device.Project;
    }

    public string GenerateDeviceProgressBar(string deviceName, string property, string text, string value, string color)
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
                        <div class=""progress-bar {2}"" id=""{3}"" role=""progressbar"" aria-valuenow=""{0}"" aria-valuemin=""0"" aria-valuemax=""100"" style=""width: {0}%"">
                          <span class=""sr-only"">{0}</span>
                        </div>
                      </div>",
                      value,
                      text,
                      cssClass,
                      deviceName + "-" + property + "-bar"
        );
    }
    
    public string GetDeviceEditLink (DeviceInfo deviceInfo)
    {
      var targetName = "";
      
      if (deviceInfo.Group == "ui")
        targetName = "UI";
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
    
    public string GetDeviceCalibrateLink (DeviceInfo deviceInfo)
    {
      var targetName = "";
      
      //if (deviceInfo.Group == "ui")
      //  targetName = "UI";
      if (deviceInfo.Group == "irrigator")
        targetName = "SoilMoistureSensor";
      //if (deviceInfo.Group == "ventilator")
      //  targetName = "Ventilator";
      //if (deviceInfo.Group == "illuminator")
      //  targetName = "Illuminator";
      if (deviceInfo.Group == "monitor") {
        if (deviceInfo.Project.StartsWith("SoilMoisture"))
          targetName = "SoilMoistureSensor";
      //  if (deviceInfo.Project.StartsWith("TemperatureHumidity"))
      //    targetName = "TemperatureHumidityMonitor";
      //  if (deviceInfo.Project.StartsWith("Light"))
      //    targetName = "LightMonitor";
      }
    
      return "Calibrate" + targetName + ".aspx?DeviceName=" + deviceInfo.Name;
    }

    public bool RequiresCalibration(DeviceInfo deviceInfo)
    {
      //if (deviceInfo.Group == "ui")
      //  targetName = "UI";
      if (deviceInfo.Group == "irrigator")
        return true;
      //if (deviceInfo.Group == "ventilator")
      //  targetName = "Ventilator";
      //if (deviceInfo.Group == "illuminator")
      //  targetName = "Illuminator";
      if (deviceInfo.Group == "monitor") {
        if (deviceInfo.Project.StartsWith("SoilMoisture"))
          return true;
      //  if (deviceInfo.Project.StartsWith("TemperatureHumidity"))
      //    targetName = "TemperatureHumidityMonitor";
      //  if (deviceInfo.Project.StartsWith("Light"))
      //    targetName = "LightMonitor";
      }

      return false;
    }
  }
}

