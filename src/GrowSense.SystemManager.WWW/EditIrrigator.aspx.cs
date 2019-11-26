using GrowSense.SystemManager.Devices;
using System.IO;
using System.Configuration;
using GrowSense.SystemManager.Web;
using System.Web.UI.WebControls;

namespace GrowSense.SystemManager.WWW
{
  using System;
  using System.Web;
  using System.Web.UI;

  public partial class EditIrrigator : System.Web.UI.Page
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
      for (int i = 0; i <= 100; i++) {
        Threshold.Items.Add (new ListItem (i + "%", i.ToString ()));
      }
      
      for (int i = 0; i <= 1024; i++) {
        DryCalibration.Items.Add (new ListItem (i.ToString (), i.ToString ()));
        WetCalibration.Items.Add (new ListItem (i.ToString (), i.ToString ()));
      }
    }

    public void PopulateForm ()
    {
      Label.Text = Device.Label;
        
      Utility.EnsureDeviceDataExists (Device.Name);
      
      PopulateReadingInterval ();
      
      PopulatePumpBurstOn ();
      PopulatePumpBurstOff ();
      
      PumpMode.Items.FindByValue (Utility.GetDeviceData (Device.Name, "M")).Selected = true;
      
      Threshold.Items.FindByValue (Utility.GetDeviceData (Device.Name, "T")).Selected = true;
      
      DryCalibration.Items.FindByValue (Utility.GetDeviceData (Device.Name, "D")).Selected = true;
      WetCalibration.Items.FindByValue (Utility.GetDeviceData (Device.Name, "W")).Selected = true;
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

    public void PopulatePumpBurstOff ()
    {
      var burstOffQuantity = Convert.ToInt32 (Utility.GetDeviceData (Device.Name, "O"));

      var burstOffType = "Seconds";
    
      if (burstOffQuantity % (60 * 60) == 0) {
        burstOffQuantity = burstOffQuantity / 60 / 60;
        burstOffType = "Hours";
      } else if (burstOffQuantity % 60 == 0) {
        burstOffQuantity = burstOffQuantity / 60;
        burstOffType = "Minutes";
      }
    
      BurstOffQuantity.Text = burstOffQuantity.ToString ();
      BurstOffType.Items.FindByValue (burstOffType).Selected = true;
    }

    public void PopulatePumpBurstOn ()
    {
      var burstOnQuantity = Convert.ToInt32 (Utility.GetDeviceData (Device.Name, "B"));

      var burstOnType = "Seconds";
    
      if (burstOnQuantity % (60 * 60) == 0) {
        burstOnQuantity = burstOnQuantity / 60 / 60;
        burstOnType = "Hours";
      } else if (burstOnQuantity % 60 == 0) {
        burstOnQuantity = burstOnQuantity / 60;
        burstOnType = "Minutes";
      }
    
      BurstOnQuantity.Text = burstOnQuantity.ToString ();
      BurstOnType.Items.FindByValue (burstOnType).Selected = true;
    }

    public void HandleSubmission ()
    {
      var isSuccess = HandleLabelSubmission ();
      
      HandleReadingIntervalSubmission ();
      HandleThresholdSubmission ();
      HandlePumpModeSubmission ();
      HandleBurstOnTimeSubmission ();
      HandleBurstOffTimeSubmission ();
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
      var existingValue = Convert.ToInt32 (Utility.GetDeviceData (Device.Name, "I"));
      
      var newValue = Convert.ToInt32 (ReadingIntervalQuantity.Text);
      
      if (ReadingIntervalType.SelectedValue == "Minutes")
        newValue = newValue * 60;
      if (ReadingIntervalType.SelectedValue == "Hours")
        newValue = newValue * 60 * 60;
      
      if (existingValue != newValue)  
        DeviceMqttHolder.Current.Publish (Device.Name, "I", newValue);
    }

    public void HandleThresholdSubmission ()
    {
      HandleSimpleValueSubmission ("T", Threshold.SelectedValue);
    }

    public void HandlePumpModeSubmission ()
    {
      HandleSimpleValueSubmission ("M", PumpMode.SelectedValue);
    }

    public void HandleBurstOnTimeSubmission ()
    {
      var existingValue = Convert.ToInt32 (DeviceMqttHolder.Current.Data [Device.Name] ["B"]);
      
      var newValue = Convert.ToInt32 (BurstOnQuantity.Text);
      
      if (BurstOnType.SelectedValue == "Minutes")
        newValue = newValue * 60;
      if (BurstOnType.SelectedValue == "Hours")
        newValue = newValue * 60 * 60;
      
      if (existingValue != newValue)  
        DeviceMqttHolder.Current.Publish (Device.Name, "B", newValue);
    }

    public void HandleBurstOffTimeSubmission ()
    {
      var existingValue = Convert.ToInt32 (DeviceMqttHolder.Current.Data [Device.Name] ["O"]);
      
      var newValue = Convert.ToInt32 (BurstOffQuantity.Text);
      
      if (BurstOffType.SelectedValue == "Minutes")
        newValue = newValue * 60;
      if (BurstOffType.SelectedValue == "Hours")
        newValue = newValue * 60 * 60;
      
      if (existingValue != newValue)  
        DeviceMqttHolder.Current.Publish (Device.Name, "O", newValue);
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
  }
}

