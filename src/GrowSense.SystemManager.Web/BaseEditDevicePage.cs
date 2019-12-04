using System;
using System.Web.UI;
using GrowSense.SystemManager.Devices;
using System.IO;
using System.Configuration;
using System.Web.Configuration;
using System.Threading;

namespace GrowSense.SystemManager.Web
{
  public class BaseEditDevicePage : Page
  {
    protected System.Web.UI.WebControls.TextBox Name;
    protected System.Web.UI.WebControls.TextBox Label;
    protected System.Web.UI.WebControls.TextBox ReadingIntervalQuantity;
    protected System.Web.UI.WebControls.DropDownList ReadingIntervalType;
    public DeviceInfo Device;
    public DeviceManager DeviceManager;
    public DeviceWebUtility Utility;

    public BaseEditDevicePage ()
    {
    }

    protected override void OnLoad (EventArgs e)
    {
      base.OnLoad (e);
      
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

    public virtual void InitializeForm ()
    {
    }

    public virtual void PopulateForm ()
    {
      Label.Text = Device.Label;
      Name.Text = Device.Name;
      
      Utility.EnsureDeviceDataExists (Device.Name);
      
      PopulateReadingInterval ();
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

    public virtual void HandleSubmission ()
    {
      HandleLabelSubmission (Label.Text);
      HandleReadingIntervalSubmission (Convert.ToInt32 (ReadingIntervalQuantity.Text), ReadingIntervalType.SelectedValue);
      
      // Handle name changes last
      HandleNameSubmission (Name.Text);
      
      RedirectToDevicesPageAfterUpdate ();
    }

    public void HandleLabelSubmission (string newLabel)
    {
      if (Device.Label != newLabel)
        Utility.SetDeviceLabel (Device.Name, newLabel);
    }

    public void HandleNameSubmission (string newName)
    {
      if (Device.Name != newName) {
        // Sleep for 3 seconds to give the device time to detect any other changes to settings
        Thread.Sleep (3000);
        Utility.RenameDevice (Device.Name, newName);
      }
    }

    public void HandleReadingIntervalSubmission (int newReadingInterval, string readingIntervalType)
    {
      var existingValue = Convert.ToInt32 (Utility.GetDeviceData (Device.Name, "I"));
      
      if (readingIntervalType == "Minutes")
        newReadingInterval = newReadingInterval * 60;
      if (readingIntervalType == "Hours")
        newReadingInterval = newReadingInterval * 60 * 60;
      
      if (existingValue != newReadingInterval)  
        DeviceMqttHolder.Current.Publish (Device.Name, "I", newReadingInterval);
    }

    public void HandleSimpleValueSubmission (string topicKey, string newValue)
    {
      var existingValue = Utility.GetDeviceData (Device.Name, topicKey);
      
      if (existingValue != newValue)  
        DeviceMqttHolder.Current.Publish (Device.Name, topicKey, newValue);
    }

    public void RedirectToDevicesPageAfterUpdate ()
    {
      Utility.RedirectToDevicesPage ("The device was updated successfully.", false);
    }

    public string GetConfigSetting (string key)
    {
      return WebConfigurationManager.AppSettings [key];
    }
  }
}

