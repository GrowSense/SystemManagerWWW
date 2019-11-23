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

  public partial class EditVentilator : System.Web.UI.Page
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
    }

    public void PopulateForm ()
    {
      Label.Text = Device.Label;
        
      Utility.EnsureDeviceDataExists (Device.Name);
      
      PopulateReadingInterval ();
      
      FanMode.Items.FindByValue (Utility.GetDeviceData (Device.Name, "M")).Selected = true;
      
      PopulateTemperatureSettings ();
      PopulateHumiditySettings ();
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

    public void HandleSubmission ()
    {
      var isSuccess = HandleLabelSubmission ();
      
      HandleReadingIntervalSubmission ();
      HandleFanModeSubmission ();
      
      HandleTemperatureSubmission ();
      HandleHumiditySubmission ();
      
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

    public void HandleSimpleValueSubmission (string topicKey, string newValue)
    {
      var existingValue = Utility.GetDeviceData (Device.Name, topicKey);
      
      if (existingValue != newValue)  
        DeviceMqttHolder.Current.Publish (Device.Name, topicKey, newValue);
    }
  }
}

