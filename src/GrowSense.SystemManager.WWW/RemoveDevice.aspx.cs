using System.IO;
using System.Configuration;
using GrowSense.SystemManager.Devices;
using GrowSense.SystemManager.Web;

namespace GrowSense.SystemManager.WWW
{
  using System;
  using System.Web;
  using System.Web.UI;

  public partial class RemoveDevice : System.Web.UI.Page
  {
    public void Page_Load (object sender, EventArgs e)
    {
      var deviceName = Request.QueryString ["DeviceName"];
      
      var resultMessage = "";
      
      if (String.IsNullOrEmpty (deviceName)) {
        resultMessage = ("No device name specified in the query string.");
        Response.Redirect ("Devices.aspx?Result=" + resultMessage);
      }
      
      var indexDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["IndexDirectory"]);
    
      var devicesDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["DevicesDirectory"]);
    
      var manager = new DeviceManager (indexDirectory, devicesDirectory);
      
      var utility = new DeviceWebUtility (manager);
      
      var isSuccess = manager.RemoveDevice (deviceName);
      
      if (isSuccess) {
        resultMessage = "Device '" + deviceName + "' was removed successfully!";
        utility.RedirectToDevicesPage (resultMessage);
      } else {
        resultMessage = "Failed to remove '" + deviceName + "' device!";
        var log = manager.Starter.Output;
        utility.RedirectToErrorPage (resultMessage, log);
      }
    }
  }
}

