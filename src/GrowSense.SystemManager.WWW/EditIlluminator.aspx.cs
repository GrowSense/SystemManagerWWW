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

    public void Page_Load (object sender, EventArgs e)
    {
      var indexDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["IndexDirectory"]);
    
      var devicesDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["DevicesDirectory"]);
    
      DeviceManager = new DeviceManager (indexDirectory, devicesDirectory);
      
      var deviceName = Request.QueryString ["DeviceName"];
  
      Device = DeviceManager.GetDeviceInfo (deviceName);
    
      if (!IsPostBack) {
      
        InitializeForm ();
      
        PopulateForm ();
      } else {
        HandleSubmission ();
        
        Response.Redirect ("Devices.aspx");
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
      for (int i = 1; i <= 24; i++) {
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
        
      PopulateReadingInterval ();
      
      var lightMode = DeviceMqttHolder.Current.Data [Device.Name] ["M"];
      if (lightMode == "6")
        lightMode = "3";
      LightMode.Items.FindByValue (lightMode).Selected = true;
      
      var threshold = DeviceMqttHolder.Current.Data [Device.Name] ["T"];
      Threshold.Items.FindByValue (threshold).Selected = true;
      
      PopulateTimerSettings ();
    }

    public void PopulateReadingInterval ()
    {
      var readingIntervalQuantity = Convert.ToInt32 (DeviceMqttHolder.Current.Data [Device.Name] ["I"]);

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
      var timerStartHour = DeviceMqttHolder.Current.Data [Device.Name] ["E"];
      var timerStartMinute = DeviceMqttHolder.Current.Data [Device.Name] ["F"];
      var timerStopHour = DeviceMqttHolder.Current.Data [Device.Name] ["G"];
      var timerStopMinute = DeviceMqttHolder.Current.Data [Device.Name] ["H"];

      TimerStartHour.Items.FindByValue (timerStartHour).Selected = true;
      TimerStartMinute.Items.FindByValue (timerStartMinute).Selected = true;
      TimerStopHour.Items.FindByValue (timerStopHour).Selected = true;
      TimerStopMinute.Items.FindByValue (timerStopMinute).Selected = true;
    }

    public void HandleSubmission ()
    {
      HandleLabelSubmission ();
      
      HandleReadingIntervalSubmission ();
      HandleLightModeSubmission ();
      HandleThresholdSubmission ();
      HandleTimerSubmission ();
    }

    public void HandleLabelSubmission ()
    {
      var newLabel = Label.Text;
    
      if (Device.Label != newLabel)
        DeviceManager.SetDeviceLabel (Device.Name, newLabel);
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
      var existingValue = DeviceMqttHolder.Current.Data [Device.Name] [topicKey];
      
      if (existingValue != newValue)  
        DeviceMqttHolder.Current.Publish (Device.Name, topicKey, newValue);
    }
  }
}

