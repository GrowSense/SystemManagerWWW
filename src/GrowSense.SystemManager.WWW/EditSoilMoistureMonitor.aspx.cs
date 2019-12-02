using GrowSense.SystemManager.Devices;
using System.IO;
using System.Configuration;
using System.Web.UI.WebControls;
using GrowSense.SystemManager.Web;
using System.Web.Configuration;

namespace GrowSense.SystemManager.WWW
{
  using System;
  using System.Web;
  using System.Web.UI;

  public partial class EditSoilMoistureMonitor : System.Web.UI.Page
  {
    public DeviceInfo Device;
    public DeviceManager DeviceManager;
    public DeviceWebUtility Utility;

    public void Page_Load (object sender, EventArgs e)
    {
      var indexDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["IndexDirectory"]);
    
      var devicesDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["DevicesDirectory"]);
    
      DeviceManager = new DeviceManager (indexDirectory, devicesDirectory);
      Utility = new DeviceWebUtility (DeviceManager);
      
      var deviceName = Request.QueryString ["DeviceName"];
      Utility.EnsureDeviceNameProvided (deviceName);
  
      Device = DeviceManager.GetDeviceInfo (deviceName);
      Utility.EnsureDeviceExists (deviceName);
    
      if (!IsPostBack) {
        InitializeForm ();
      
        PopulateForm ();
      } else {
        HandleSubmission ();
      }
    }

    public void InitializeForm ()
    {      
      for (int i = 0; i <= 1024; i++) {
        DryCalibration.Items.Add (new ListItem (i.ToString (), i.ToString ()));
        WetCalibration.Items.Add (new ListItem (i.ToString (), i.ToString ()));
      }
    }

    public void PopulateForm ()
    {
      Label.Text = Device.Label;
        
      PopulateReadingInterval ();
      
      DryCalibration.Items.FindByValue (Utility.GetDeviceData (Device.Name, "D")).Selected = true;
      WetCalibration.Items.FindByValue (Utility.GetDeviceData (Device.Name, "W")).Selected = true;
      
      PopulateBoardType ();
    }

    public void PopulateReadingInterval ()
    {
      var readingIntervalQuantity = Convert.ToInt32 (Utility.GetDeviceData (Device.Name, "I"));

      var readingIntervalType = "Seconds";
    
      if (readingIntervalQuantity % (60 * 60) == 0) {
        readingIntervalQuantity = readingIntervalQuantity / 60 / 60;
        readingIntervalType = "Hours";
      } else if (readingIntervalQuantity % 60 == 0) {
        readingIntervalQuantity = readingIntervalQuantity / 60;
        readingIntervalType = "Minutes";
      }
    
      ReadingIntervalQuantity.Text = readingIntervalQuantity.ToString ();
      ReadingIntervalType.Items.FindByValue (readingIntervalType).Selected = true;
    }

    public void PopulateBoardType ()
    {
      if (Device.Board == "esp")
        BoardType.SelectedValue = "esp";
    }

    public void HandleSubmission ()
    {
      bool isSuccess = HandleLabelSubmission ();
      
      HandleReadingIntervalSubmission ();
      HandleDryCalibrationSubmission ();
      HandleWetCalibrationSubmission ();
      
      var resultMessage = "";
      var queryStringPostFix = "";
      if (isSuccess)  
        resultMessage = "Device updated successfully!";
      else {
        resultMessage = "Failed to update device!";
        queryStringPostFix = "&IsSuccess=false";
      }
      Response.Redirect ("Devices.aspx?Result=" + resultMessage + queryStringPostFix);
    }

    public bool HandleLabelSubmission ()
    {
      var newLabel = Label.Text;
    
      if (Device.Label != newLabel)
        return DeviceManager.SetDeviceLabel (Device.Name, newLabel);
        
      return true;
    }

    public void HandleReadingIntervalSubmission ()
    {
      var existingValue = Convert.ToInt32 (DeviceMqttHolder.Current.Data [Device.Name] ["I"]);
      
      var newValue = Convert.ToInt32 (ReadingIntervalQuantity.Text);
      
      if (ReadingIntervalType.SelectedValue == "Minutes")
        newValue = newValue * 60;
      if (ReadingIntervalType.SelectedValue == "Hours")
        newValue = newValue * 60 * 60;
      
      if (existingValue != newValue)  
        DeviceMqttHolder.Current.Publish (Device.Name, "I", newValue);
    }

    public void HandleDryCalibrationSubmission ()
    {
      HandleSimpleValueSubmission ("D", DryCalibration.SelectedValue);
    }

    public void HandleWetCalibrationSubmission ()
    {
      HandleSimpleValueSubmission ("W", WetCalibration.SelectedValue);
    }

    public void HandleSimpleValueSubmission (string topicKey, string newValue)
    {
      var existingValue = DeviceMqttHolder.Current.Data [Device.Name] [topicKey];
      
      if (existingValue != newValue)  
        DeviceMqttHolder.Current.Publish (Device.Name, topicKey, newValue);
    }

    public string GetConfigSetting (string key)
    {
      return WebConfigurationManager.AppSettings [key];
    }

    public string GenerateRawProgressBar ()
    {
      int width = (int)((float)100 / (float)1024 * (float)Convert.ToInt32 (Utility.GetDeviceData (Device.Name, "R")));
      return @"<div class=""progress-bar progress-bar-info"" role=""progressbar"" aria-valuenow='" + Utility.GetDeviceData (Device.Name, "R") + @"' aria-valuemin=""0"" aria-valuemax=""1024"" style='width: " + width + @"%'>
                  <span class=""sr-only"">" + Utility.GetDeviceData (Device.Name, "R") + @"</span>
                </div>";
    }
  }
}

