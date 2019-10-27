using GrowSense.SystemManager.Devices;
using System.IO;
using System.Configuration;
using System.Web.UI.WebControls;
using GrowSense.SystemManager.Web;

namespace GrowSense.SystemManager.WWW
{
  using System;
  using System.Web;
  using System.Web.UI;

  public partial class EditSoilMoistureMonitor : System.Web.UI.Page
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
      for (int i = 1; i <= 1023; i++) {
        DryCalibration.Items.Add (new ListItem (i.ToString (), i.ToString ()));
        WetCalibration.Items.Add (new ListItem (i.ToString (), i.ToString ()));
      }
    }

    public void PopulateForm ()
    {
      Label.Text = Device.Label;
        
      PopulateReadingInterval ();
      
      DryCalibration.Items.FindByValue (DeviceMqttHolder.Current.Data [Device.Name] ["D"]).Selected = true;
      WetCalibration.Items.FindByValue (DeviceMqttHolder.Current.Data [Device.Name] ["W"]).Selected = true;
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

    public void HandleSubmission ()
    {
      HandleLabelSubmission ();
      
      HandleReadingIntervalSubmission ();
      HandleDryCalibrationSubmission ();
      HandleWetCalibrationSubmission ();
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

