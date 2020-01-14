using GrowSense.SystemManager.Computers;
using System.IO;
using System.Configuration;

namespace GrowSense.SystemManager.WWW
{
  using System;
  using System.Web;
  using System.Web.UI;

  public partial class ViewService : System.Web.UI.Page
  {
    public string ComputerName;
    public string ServiceName;
    public string StatusOutput;
    public ComputerManager Manager;

    public void Page_Load (object sender, EventArgs e)
    {
      ComputerName = Request.QueryString ["Computer"];
      ServiceName = Request.QueryString ["Service"];
      
      var indexDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["IndexDirectory"]);
    
      var computersDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["ComputersDirectory"]);
    
      Manager = new ComputerManager (indexDirectory, computersDirectory);
      
      StatusOutput = Manager.GetServiceStatusText (ComputerName, ServiceName);
    }

    public string FormatDetails (string details)
    {
      var formattedDetails = details.Replace ("\n", "<br/>")
        .Replace (" ", "&nbsp;");
        
      return formattedDetails;
    }
  }
}

