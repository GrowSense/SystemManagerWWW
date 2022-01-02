using System;
using System.Web;
using System.Web.UI;
using GrowSense.SystemManager.Web;
using GrowSense.SystemManager.Devices;
using System.IO;
using System.Web.Configuration;
using System.Threading;
namespace GrowSense.SystemManager.WWW
{
  public partial class CalibrateSoilMoistureSensor : System.Web.UI.Page
  {
    public DeviceInfo Device;
    
    public DeviceWebUtility Utility;
    public DeviceManager DeviceManager;

    public string Result;
    public bool IsSuccess;

    public void Page_Load(object sender, EventArgs e)
    {
      var indexDirectory = Path.GetFullPath(WebConfigurationManager.AppSettings["IndexDirectory"]);

      var devicesDirectory = Path.GetFullPath(WebConfigurationManager.AppSettings["DevicesDirectory"]);

      DeviceManager = new DeviceManager(indexDirectory, devicesDirectory);
      Utility = new DeviceWebUtility(DeviceManager);
      
      var deviceName = Request.QueryString["DeviceName"];
      Device = DeviceManager.GetDeviceInfo(deviceName);
    }

    public void SaveButton_Click(object sender, EventArgs e)
    {
      var dryValue = Request.Form["DryValue"];
      var wetValue = Request.Form["WetValue"];

      HandleSimpleValueSubmission("W", wetValue);
      HandleSimpleValueSubmission("D", dryValue);

      Thread.Sleep(1000);

      var updatedDryValue = Utility.GetDeviceData(Device.Name, "D");
      var updatedWetValue = Utility.GetDeviceData(Device.Name, "W");

      var didUpdate = true;

      if (updatedDryValue != dryValue)
        didUpdate = false;
      if (updatedWetValue != wetValue)
        didUpdate = false;

      if (!didUpdate)
      {
        Result = "Failed to update calibration values!";
        IsSuccess = false;
      }
      else
      {
        var msg = "Soil moisture sensor calibrated succesfully!";

        Response.Redirect("Devices.aspx?Result=" + msg + "&IsSuccess=true");
      }
    }
    
    public void HandleSimpleValueSubmission (string topicKey, string newValue)
    {
      var existingValue = Utility.GetDeviceData (Device.Name, topicKey);
      
      if (existingValue != newValue)  
        DeviceMqttHolder.Current.Publish (Device.Name, topicKey, newValue);
    }
    
    public string GenerateRawProgressBar ()
    {
      /*int width = Convert.ToInt32 (Utility.GetDeviceData (Device.Name, "R"));
      return @"<div class=""progress-bar progress-bar-info"" id=""rawValue"" role=""progressbar"" aria-valuenow='" + Utility.GetDeviceData (Device.Name, "R") + @"' aria-valuemin=""0"" aria-valuemax=""1024"" style='width: " + width + @"'>
                  <span class=""sr-only"">" + Utility.GetDeviceData (Device.Name, "R") + @"</span>
                </div>";*/
                
      int width = (int)((float)100 / (float)1024 * (float)Convert.ToInt32 (Utility.GetDeviceData (Device.Name, "R")));
      return @"<div class=""progress-bar progress-bar-info"" id=""rawBar"" role=""progressbar"" aria-valuenow='" + Utility.GetDeviceData (Device.Name, "R") + @"' aria-valuemin=""0"" aria-valuemax=""1024"" style='width: " + width + @"%'>
                  <span class=""sr-only"">" + Utility.GetDeviceData (Device.Name, "R") + @"</span>
                </div>";
    }
    
    public string GenerateDryProgressBar ()
    {
      int width = (int)((float)100 / (float)1024 * (float)Convert.ToInt32 (Utility.GetDeviceData (Device.Name, "D")));
      return @"<div class=""progress-bar progress-bar-info"" id=""dryBar"" role=""progressbar"" aria-valuenow='" + Utility.GetDeviceData (Device.Name, "D") + @"' aria-valuemin=""0"" aria-valuemax=""1024"" style='width: " + width + @"%'>
                  <span class=""sr-only"">" + Utility.GetDeviceData (Device.Name, "D") + @"</span>
                </div>";
    }
    
    public string GenerateWetProgressBar ()
    {
      int width = (int)((float)100 / (float)1024 * (float)Convert.ToInt32 (Utility.GetDeviceData (Device.Name, "W")));
      return @"<div class=""progress-bar progress-bar-info"" id=""wetBar"" role=""progressbar"" aria-valuenow='" + Utility.GetDeviceData (Device.Name, "W") + @"' aria-valuemin=""0"" aria-valuemax=""1024"" style='width: " + width + @"%'>
                  <span class=""sr-only"">" + Utility.GetDeviceData (Device.Name, "W") + @"</span>
                </div>";
    }
  }
}
