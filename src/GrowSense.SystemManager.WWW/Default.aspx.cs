using GrowSense.SystemManager.Devices;
using System.Configuration;
using System.IO;
using GrowSense.SystemManager.Computers;

namespace GrowSense.SystemManager
{
  using System;
  using System.Web;
  using System.Web.UI;

  public partial class Default : System.Web.UI.Page
  {
    public int TotalDevices = 0;
    public int TotalComputers = 0;

    public void Page_Load (object sender, EventArgs e)
    {
      var indexDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["IndexDirectory"]);
      var devicesDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["DevicesDirectory"]);
      var computersDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["ComputersDirectory"]);
    
      TotalDevices = new DeviceManager (indexDirectory, devicesDirectory).CountDevices ();
      TotalComputers = new ComputerManager (computersDirectory).CountComputers ();
    }
  }
}

