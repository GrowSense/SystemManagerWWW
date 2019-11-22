
namespace GrowSense.SystemManager.WWW
{
  using System;
  using System.Web;
  using System.Web.UI;

  public partial class Error : System.Web.UI.Page
  {
    public string ErrorMessage = "No error occurred.";

    public void Page_Loage (object sender, EventArgs e)
    {
      if (!String.IsNullOrEmpty (Request.QueryString ["Message"]))
        ErrorMessage = Request.QueryString ["Message"];
    }
  }
}

