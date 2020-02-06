using GrowSense.SystemManager.Computers;
using System.IO;
using System.Configuration;

namespace GrowSense.SystemManager.WWW
{
  using System;
  using System.Web;
  using System.Web.UI;

  public partial class NetworkSetupStatus : System.Web.UI.Page
  {
    public string Result = "Reconnecting... (please wait)";
    public string ServiceOutput = "Pending...";

    public void Page_Load (object sender, EventArgs e)
    {
      var indexDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["IndexDirectory"]);
     
      var computersDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["ComputersDirectory"]);
     
      var manager = new ComputerManager (indexDirectory, computersDirectory);
      var output = manager.GetServiceStatusText ("Local", "growsense-network-setup.service");
      
      ServiceOutput = output.Replace ("\n", "<br/>");
      
      if (output.Contains ("Failed to issue method call") || output.Contains ("not supported"))
        Result = "Network reconnect not yet supported on this board. Please manually reconnect.";
      if (output.Contains ("connected"))
        Result = "Successfully connected to network.";
      if (output.Contains ("failed"))
        Result = "Failed to connect to network.";
    }
  }
}

