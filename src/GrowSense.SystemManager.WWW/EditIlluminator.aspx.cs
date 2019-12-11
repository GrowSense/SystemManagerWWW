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

  public partial class EditIlluminator : BaseEditDevicePage
  {
    public override void InitializeForm ()
    {
      for (int i = 1; i <= 100; i++) {
        Threshold.Items.Add (new ListItem (i + "%", i.ToString ()));
      }
      
      InitializeTimerSettings ();
      
      InitializeCalibrationSettings ();
      
      base.InitializeForm ();
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
        var text = i.ToString ();
        if (i <= 9)
          text = "0" + text;
        TimerStartMinute.Items.Add (new ListItem (text, i.ToString ()));
        TimerStopMinute.Items.Add (new ListItem (text, i.ToString ()));
      }
    }

    public void InitializeCalibrationSettings ()
    {
      for (int i = 0; i <= 1024; i++) {
        DarkCalibration.Items.Add (new ListItem (i.ToString (), i.ToString ()));
        BrightCalibration.Items.Add (new ListItem (i.ToString (), i.ToString ()));
      }
    }

    public override void PopulateForm ()
    {      
      var lightMode = Utility.GetDeviceData (Device.Name, "M");
      if (lightMode == "6")
        lightMode = "3";
      LightMode.Items.FindByValue (lightMode).Selected = true;
      
      var threshold = Utility.GetDeviceData (Device.Name, "T");
      Threshold.Items.FindByValue (threshold).Selected = true;
      
      PopulateTimerSettings ();
      
      DarkCalibration.Items.FindByValue (Utility.GetDeviceData (Device.Name, "D")).Selected = true;
      BrightCalibration.Items.FindByValue (Utility.GetDeviceData (Device.Name, "B")).Selected = true;
      
      base.PopulateForm ();
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

    public override void HandleSubmission ()
    {
      HandleLightModeSubmission ();
      HandleThresholdSubmission ();
      HandleTimerSubmission ();
      HandleCalibrationSubmission ();
      
      base.HandleSubmission ();
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

    public void HandleCalibrationSubmission ()
    {
      HandleSimpleValueSubmission ("D", DarkCalibration.SelectedValue);
      HandleSimpleValueSubmission ("B", BrightCalibration.SelectedValue);
    }
  }
}

