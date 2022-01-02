using GrowSense.SystemManager.Devices;
using System.IO;
using System.Configuration;
using GrowSense.SystemManager.Web;
using System.Web.UI.WebControls;
using System.Web.Configuration;

namespace GrowSense.SystemManager.WWW
{
  using System;
  using System.Web;
  using System.Web.UI;

  public partial class EditIrrigator : BaseEditDevicePage
  {
    public override void InitializeForm ()
    {
      for (int i = 0; i <= 100; i++) {
        Threshold.Items.Add (new ListItem (i + "%", i.ToString ()));
      }
      
      base.InitializeForm ();
    }

    public override void PopulateForm ()
    {
      base.PopulateForm ();
    
      PopulatePumpBurstOn ();
      PopulatePumpBurstOff ();
      
      PumpMode.Items.FindByValue (Utility.GetDeviceData (Device.Name, "M")).Selected = true;
      
      Threshold.Items.FindByValue (Utility.GetDeviceData (Device.Name, "T")).Selected = true;
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

    public override void HandleSubmission ()
    {
      HandleThresholdSubmission ();
      HandlePumpModeSubmission ();
      HandleBurstOnTimeSubmission ();
      HandleBurstOffTimeSubmission ();
      
      base.HandleSubmission ();
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
      var existingValue = Convert.ToInt32 (Utility.GetDeviceData (Device.Name, "B"));
      
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
      var existingValue = Convert.ToInt32 (Utility.GetDeviceData (Device.Name, "O"));
      
      var newValue = Convert.ToInt32 (BurstOffQuantity.Text);
      
      if (BurstOffType.SelectedValue == "Minutes")
        newValue = newValue * 60;
      if (BurstOffType.SelectedValue == "Hours")
        newValue = newValue * 60 * 60;
      
      if (existingValue != newValue)  
        DeviceMqttHolder.Current.Publish (Device.Name, "O", newValue);
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
