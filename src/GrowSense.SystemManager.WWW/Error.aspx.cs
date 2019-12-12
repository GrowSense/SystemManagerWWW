
namespace GrowSense.SystemManager.WWW
{
  using System;
  using System.Web;
  using System.Web.UI;

  public partial class Error : System.Web.UI.Page
  {
    public string ErrorMessage = "No error occurred.";
    public string ErrorDetails = "";

    public void Page_Load (object sender, EventArgs e)
    {
      if (!String.IsNullOrEmpty (Request.QueryString ["Message"]))
        ErrorMessage = Request.QueryString ["Message"];
        
      if (Session ["ErrorDetails"] != null) {
        ErrorDetails = (string)Session ["ErrorDetails"];
      }
    }

    public string FormatDetails (string details)
    {
      var formattedDetails = details.Replace ("\n", "<br/>")
        .Replace (" ", "&nbsp;");
        
      return formattedDetails;
    }
  }
}

