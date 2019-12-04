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

  public partial class EditSoilMoistureMonitor : BaseEditDevicePage
  {
    public override void InitializeForm ()
    {      
      for (int i = 0; i <= 1024; i++) {
        DryCalibration.Items.Add (new ListItem (i.ToString (), i.ToString ()));
        WetCalibration.Items.Add (new ListItem (i.ToString (), i.ToString ()));
      }
      
      base.InitializeForm ();
    }

    public override void PopulateForm ()
    {
      DryCalibration.Items.FindByValue (Utility.GetDeviceData (Device.Name, "D")).Selected = true;
      WetCalibration.Items.FindByValue (Utility.GetDeviceData (Device.Name, "W")).Selected = true;
      
      PopulateBoardType ();
      
      base.PopulateForm ();
    }

    public void PopulateBoardType ()
    {
      if (Device.Board == "esp")
        BoardType.SelectedValue = "esp";
    }

    public override void HandleSubmission ()
    {
      HandleDryCalibrationSubmission ();
      HandleWetCalibrationSubmission ();
      
      base.HandleSubmission ();
    }

    public void HandleDryCalibrationSubmission ()
    {
      HandleSimpleValueSubmission ("D", DryCalibration.SelectedValue);
    }

    public void HandleWetCalibrationSubmission ()
    {
      HandleSimpleValueSubmission ("W", WetCalibration.SelectedValue);
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

