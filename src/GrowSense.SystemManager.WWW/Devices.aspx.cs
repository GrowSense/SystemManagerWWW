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
      var devicesDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["DevicesDirectory"]);
    
      var deviceManager = new DeviceManager (Path.GetFullPath ("devices"));
      DevicesInfo = deviceManager.GetDevicesInfo ();
    }
    
    public string GetDeviceData (string deviceName, string topicKey)
    {
    return DeviceMqttListenerHolder.Current.Data[deviceName][topicKey];
    }
    
    public string GenerateDeviceStatusIcon (string deviceName)
    {
      var statusText = GetDeviceData(deviceName, "StatusMessage");
    
      var cssClass = string.Empty;
    
      if (statusText == "Online")
        cssClass = "label-success";
      if (statusText == "Connected")
        cssClass = "label-success";
      if (statusText == "Offline")
        cssClass = "label-danger";
    
      return String.Format("<div class=\"label {0} label-mini\">{1}</div>",
        cssClass,
        statusText
        );
    }
    
    public string GenerateDeviceProgressBars (DeviceInfo device)
    {
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
  }
}

