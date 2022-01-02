using System.IO;
using System.Configuration;
using GrowSense.SystemManager.Computers;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.Configuration;
using GrowSense.SystemManager.Common;
using System;
using System.Web;
using GrowSense.SystemManager.Mqtt;

namespace GrowSense.SystemManager.WWW
{
  public partial class LoginSettings : System.Web.UI.Page
  {
    public SettingsManager Manager;

    public SystemSettings Settings;

    public string Result;
    public bool IsSuccess;
    
    public void Page_Load (object sender, EventArgs e)
    {
      var indexDirectory = Path.GetFullPath (WebConfigurationManager.AppSettings ["IndexDirectory"]);

      Manager = new SettingsManager(indexDirectory);
      Settings = Manager.LoadSettings();

      /*if (!String.IsNullOrEmpty(Request.QueryString["Result"]))
        Result = Request.QueryString["Result"];
        
      if (!String.IsNullOrEmpty(Request.QueryString["Result"]))
        Result = Request.QueryString["Result"];*/
    
    
      if (!IsPostBack) {
        PopulateForm ();
      } 
    }

    public void PopulateForm()
    {
      Username.Text = Settings.Username;
      Password.Text = Settings.Password;
    }

    public void Save_Click (object sender, EventArgs e)
    {
      Settings.Username = Username.Text;
      Settings.Password = Password.Text;

      if (Manager.SaveLoginSettings(Settings))
      {
        var message = "Login settings updated!";

        Response.Redirect("Settings.aspx?Result=" + HttpUtility.UrlEncode(message));
      }
      else
      {
        Result = "Failed to update login settings.";
        IsSuccess = false;
      }
    }
  }
}

