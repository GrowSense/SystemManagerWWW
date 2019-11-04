using System.IO;
using System.Configuration;
using GrowSense.SystemManager.Devices;

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
      
      var isSuccess = manager.RemoveDevice (deviceName);
      
      var postFixQueryString = "";
      if (isSuccess)
        resultMessage = "Device '" + deviceName + "' was removed successfully!";
      else {
        resultMessage = "Device '" + deviceName + "' wasn't removed.";
        postFixQueryString = "&IsSuccess=false";
      }
      Response.Redirect ("Devices.aspx?Result=" + (resultMessage).Replace (" ", "+") + postFixQueryString);
    }
  }
}

