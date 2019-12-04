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

  public partial class EditVentilator : BaseEditDevicePage
  {
    public override void InitializeForm ()
    {
      base.InitializeForm ();
    }

    public override void PopulateForm ()
    {
      FanMode.Items.FindByValue (Utility.GetDeviceData (Device.Name, "M")).Selected = true;
      
      PopulateTemperatureSettings ();
      PopulateHumiditySettings ();
      
      base.PopulateForm ();
    }

    public void PopulateTemperatureSettings ()
    {
      for (int i = 0; i < 100; i++) {
        MinimumTemperature.Items.Add (new ListItem (i + "c", i.ToString ()));
        MaximumTemperature.Items.Add (new ListItem (i + "c", i.ToString ()));
      }
      
      var minTemperature = Utility.GetDeviceData (Device.Name, "S");
      var maxTemperature = Utility.GetDeviceData (Device.Name, "U");

      MinimumTemperature.Items.FindByValue (minTemperature).Selected = true;
      MaximumTemperature.Items.FindByValue (maxTemperature).Selected = true;
    }

    public void PopulateHumiditySettings ()
    {
      for (int i = 0; i < 100; i++) {
        MinimumHumidity.Items.Add (new ListItem (i + "%", i.ToString ()));
        MaximumHumidity.Items.Add (new ListItem (i + "%", i.ToString ()));
      }
      
      var minHumidity = Utility.GetDeviceData (Device.Name, "G");
      var maxHumidity = Utility.GetDeviceData (Device.Name, "J");

      MinimumHumidity.Items.FindByValue (minHumidity).Selected = true;
      MaximumHumidity.Items.FindByValue (maxHumidity).Selected = true;
    }

    public override void HandleSubmission ()
    {
      HandleFanModeSubmission ();
      
      HandleTemperatureSubmission ();
      HandleHumiditySubmission ();
      
      base.HandleSubmission ();
    }

    public void HandleFanModeSubmission ()
    {
      HandleSimpleValueSubmission ("M", FanMode.SelectedValue);
    }

    public void HandleTemperatureSubmission ()
    {
      HandleSimpleValueSubmission ("S", MinimumTemperature.SelectedValue);
      HandleSimpleValueSubmission ("U", MaximumTemperature.SelectedValue);
    }

    public void HandleHumiditySubmission ()
    {
      HandleSimpleValueSubmission ("G", MinimumHumidity.SelectedValue);
      HandleSimpleValueSubmission ("J", MaximumHumidity.SelectedValue);
    }
  }
}

