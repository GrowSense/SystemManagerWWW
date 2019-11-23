using GrowSense.SystemManager.Web;
using System.Web.UI.WebControls;
using GrowSense.SystemManager.Devices;
using System.IO;
using System.Configuration;

namespace GrowSense.SystemManager.WWW
{
  using System;
  using System.Web;
  using System.Web.UI;

  public partial class EditIlluminator : System.Web.UI.Page
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
      for (int i = 1; i <= 100; i++) {
        Threshold.Items.Add (new ListItem (i + "%", i.ToString ()));
      }
      
      InitializeTimerSettings ();
    }

    public void InitializeTimerSettings ()
    {
      for (int i = 0; i <= 23; i++) {
        var postFix = "";
        if (i > 12)
          postFix = " (" + (i - 12) + ")";
        TimerStartHour.Items.Add (new ListItem (i.ToString () + postFix, i.ToString ()));
        TimerStopHour.Items.Add (new ListItem (i.ToString () + postFix, i.ToString ()));
      }
      
      for (int i = 0; i <= 60; i++) {
        TimerStartMinute.Items.Add (new ListItem (i.ToString (), i.ToString ()));
        TimerStopMinute.Items.Add (new ListItem (i.ToString (), i.ToString ()));
      }
    }

    public void PopulateForm ()
    {
      Label.Text = Device.Label;
        
      Utility.EnsureDeviceDataExists (Device.Name);
      
      PopulateReadingInterval ();
      
      var lightMode = Utility.GetDeviceData (Device.Name, "M");
      if (lightMode == "6")
        lightMode = "3";
      LightMode.Items.FindByValue (lightMode).Selected = true;
      
      var threshold = Utility.GetDeviceData (Device.Name, "T");
      Threshold.Items.FindByValue (threshold).Selected = true;
      
      PopulateTimerSettings ();
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

    public void PopulateTimerSettings ()
    {      
      var timerStartHour = Utility.GetDeviceData (Device.Name, "E");
      var timerStartMinute = Utility.GetDeviceData (Device.Name, "F");
      var timerStopHour = Utility.GetDeviceData (Device.Name, "G");
      var timerStopMinute = Utility.GetDeviceData (Device.Name, "H");
      
      TimerStartHour.Items.FindByValue (timerStartHour).Selected = true;
      TimerStartMinute.Items.FindByValue (timerStartMinute).Selected = true;
      TimerStopHour.Items.FindByValue (timerStopHour).Selected = true;
      TimerStopMinute.Items.FindByValue (timerStopMinute).Selected = true;
    }

    public void HandleSubmission ()
    {
      bool isSuccess = HandleLabelSubmission ();
      
      HandleReadingIntervalSubmission ();
      HandleLightModeSubmission ();
      HandleThresholdSubmission ();
      HandleTimerSubmission ();
      
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

    public void HandleLightModeSubmission ()
    {
      HandleSimpleValueSubmission ("M", LightMode.SelectedValue);
    }

    public void HandleThresholdSubmission ()
    {
      HandleSimpleValueSubmission ("T", Threshold.SelectedValue);
    }

    public void HandleTimerSubmission ()
    {
      HandleSimpleValueSubmission ("E", TimerStartHour.SelectedValue);
      HandleSimpleValueSubmission ("F", TimerStartMinute.SelectedValue);
      HandleSimpleValueSubmission ("G", TimerStopHour.SelectedValue);
      HandleSimpleValueSubmission ("H", TimerStopMinute.SelectedValue);
    }

    public void HandleSimpleValueSubmission (string topicKey, string newValue)
    {
      var existingValue = Utility.GetDeviceData (Device.Name, topicKey);
      
      if (existingValue != newValue)  
        DeviceMqttHolder.Current.Publish (Device.Name, topicKey, newValue);
    }
  }
}

