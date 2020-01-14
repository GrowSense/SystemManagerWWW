using GrowSense.SystemManager.Computers;
using System.Configuration;
using System.IO;

namespace GrowSense.SystemManager.WWW
{
  using System;
  using System.Web;
  using System.Web.UI;

  public partial class ServiceAction : System.Web.UI.Page
  {
    public ComputerManager Manager;

    public void Page_Load (object sender, EventArgs e)
    {
      var indexDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["IndexDirectory"]);
    
      var computersDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["ComputersDirectory"]);
    
      Manager = new ComputerManager (indexDirectory, computersDirectory);
      
      var computerName = Request.QueryString ["Computer"];
      var action = Request.QueryString ["Action"];
      var serviceName = Request.QueryString ["Service"];
      var serviceLabel = Request.QueryString ["Label"];
      
      var output = Manager.RunServiceAction (computerName, action, serviceName);
      
      var actionLabel = action + "ed";
      if (action == "stop")
        actionLabel = "stopped";
      if (action == "disable")
        actionLabel = "disabled";
      
      var message = serviceLabel + " service " + actionLabel + " on " + computerName + " computer.";
      Response.Redirect ("ComputerTools.aspx?ComputerName=" + computerName + "&Result=" + HttpUtility.UrlEncode (message));
    }
  }
}

