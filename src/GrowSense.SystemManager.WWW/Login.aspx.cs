using System.Web.Security;

namespace GrowSense.SystemManager.WWW
{
  using System;
  using System.Web;
  using System.Web.UI;

  public partial class Login : System.Web.UI.Page
  {
    public bool IsInvalid = false;

    public void Page_Load (object sender, EventArgs e)
    {
      if (Request.QueryString ["Logout"] == "true")
        FormsAuthentication.SignOut ();
    }

    public void LogInButton_Click (object sender, EventArgs e)
    {
      if (FormsAuthentication.Authenticate (Username.Text, Password.Text)) {  
        FormsAuthentication.RedirectFromLoginPage (Username.Text, RememberMe.Checked);  
      } else {  
        IsInvalid = true;
      }
    }
  }
}

