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
    }

    public void PopulateForm ()
    {
      Label.Text = Device.Label;
        
      PopulateReadingInterval ();
      
      FanMode.Items.FindByValue (DeviceMqttHolder.Current.Data [Device.Name] ["F"]).Selected = true;
      
      PopulateTemperatureSettings ();
      PopulateHumiditySettings ();
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

    public void PopulateTemperatureSettings ()
    {
      for (int i = 0; i < 100; i++) {
        MinimumTemperature.Items.Add (new ListItem (i + "c", i.ToString ()));
        MaximumTemperature.Items.Add (new ListItem (i + "c", i.ToString ()));
      }
      
      var minTemperature = DeviceMqttHolder.Current.Data [Device.Name] ["S"];
      var maxTemperature = DeviceMqttHolder.Current.Data [Device.Name] ["U"];

      MinimumTemperature.Items.FindByValue (minTemperature).Selected = true;
      MaximumTemperature.Items.FindByValue (maxTemperature).Selected = true;
    }

    public void PopulateHumiditySettings ()
    {
      for (int i = 0; i < 100; i++) {
        MinimumHumidity.Items.Add (new ListItem (i + "%", i.ToString ()));
        MaximumHumidity.Items.Add (new ListItem (i + "%", i.ToString ()));
      }
      
      var minHumidity = DeviceMqttHolder.Current.Data [Device.Name] ["G"];
      var maxHumidity = DeviceMqttHolder.Current.Data [Device.Name] ["J"];

      MinimumHumidity.Items.FindByValue (minHumidity).Selected = true;
      MaximumHumidity.Items.FindByValue (maxHumidity).Selected = true;
    }

    public void HandleSubmission ()
    {
      HandleLabelSubmission ();
      
      HandleReadingIntervalSubmission ();
      HandleFanModeSubmission ();
      
      HandleTemperatureSubmission ();
      HandleHumiditySubmission ();
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

    public void HandleFanModeSubmission ()
    {
      HandleSimpleValueSubmission ("F", FanMode.SelectedValue);
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
      var existingValue = DeviceMqttHolder.Current.Data [Device.Name] [topicKey];
      
      if (existingValue != newValue)  
        DeviceMqttHolder.Current.Publish (Device.Name, topicKey, newValue);
    }
  }
}

