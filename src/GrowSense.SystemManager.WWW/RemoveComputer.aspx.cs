using GrowSense.SystemManager.Computers;
using System.IO;
using System.Configuration;
using System.Web.Configuration;

namespace GrowSense.SystemManager.WWW
{
  using System;
  using System.Web;
  using System.Web.UI;

  public partial class RemoveComputer : System.Web.UI.Page
  {
    public void Page_Load (object sender, EventArgs e)
    {
      var computerName = Request.QueryString ["ComputerName"];
      
      var resultMessage = "";
      
      if (String.IsNullOrEmpty (computerName)) {
        resultMessage = ("No computer name specified in the query string.");
        Response.Redirect ("Computers.aspx?Result=" + resultMessage);
      }
      var indexDirectory = Path.GetFullPath (WebConfigurationManager.AppSettings ["IndexDirectory"]);
    
      var computersDirectory = Path.GetFullPath (WebConfigurationManager.AppSettings ["ComputersDirectory"]);
    
      var manager = new ComputerManager (indexDirectory, computersDirectory);
      
      var result = manager.RemoveComputer (computerName);
      
      var postFixQueryString = "";
      if (result.IsSuccess)
        resultMessage = "Computer '" + computerName + "' was removed successfully!";
      else {
        if (result.Error == ComputerActionError.NotFound)
          resultMessage = "Computer '" + computerName + "' wasn't found.";
        postFixQueryString = "&IsSuccess=false";
      }
      Response.Redirect ("Computers.aspx?Result=" + (resultMessage).Replace (" ", "+") + postFixQueryString);
    }
  }
}

