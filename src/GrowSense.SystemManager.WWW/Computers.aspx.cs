using System.IO;
using System.Configuration;
using GrowSense.SystemManager.Computers;

namespace GrowSense.SystemManager
{
  using System;
  using System.Web;
  using System.Web.UI;

  public partial class ComputersPage : System.Web.UI.Page
  {
    public ComputerInfo[] ComputersInfo = new ComputerInfo[] { };

    public void Page_Load (object sender, EventArgs e)
    {
      LoadComputersInfo ();
    }

    public void LoadComputersInfo ()
    {
      var indexDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["IndexDirectory"]);
    
      var computersDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["ComputersDirectory"]);
    
      var computersManager = new ComputerManager (indexDirectory, computersDirectory);
      ComputersInfo = computersManager.GetComputersInfo ();
    }
    
    public string GetEditComputerLink(ComputerInfo computer)
    {
    return "";
    }
  }
}

