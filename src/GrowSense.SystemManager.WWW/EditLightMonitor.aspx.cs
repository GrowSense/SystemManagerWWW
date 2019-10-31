using GrowSense.SystemManager.Devices;
using System.IO;
using System.Configuration;
using GrowSense.SystemManager.Web;

namespace GrowSense.SystemManager.WWW
{
  using System;
  using System.Web;
  using System.Web.UI;

  public partial class EditLightMonitor : System.Web.UI.Page
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
  }
}
