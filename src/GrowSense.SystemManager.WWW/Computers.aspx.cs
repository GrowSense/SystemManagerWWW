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
    
    public string GenerateComputerStatusIcon (ComputerInfo computer)
    {
      var cssClass = "label-info";
      var statusText = "Online";
      if (computer.IsOnline || computer.Name.ToLower().Contains("local")) {
        cssClass = "label-success";
        statusText = "Online";
      } else {
        cssClass = "label-danger";
        statusText = "Offline";
      }
      
      return String.Format ("<div class=\"label {0} label-mini\">{1}</div>",
                           cssClass,
                           statusText
      );
    }
  }
}

