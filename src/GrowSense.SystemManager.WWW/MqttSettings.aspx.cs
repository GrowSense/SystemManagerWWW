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
  public partial class MqttSettings : System.Web.UI.Page
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
    
    
      //GenerateComputerFields ();
        
      if (!IsPostBack) {
        PopulateForm ();
      } 
    }

    public void PopulateForm()
    {
      MqttHost.Text = Settings.MqttHost;
      MqttUsername.Text = Settings.MqttUsername;
      MqttPassword.Text = Settings.MqttPassword;
      MqttPort.Text = Settings.MqttPort.ToString();
    }

    
    public string GetSecurityValue (string key)
    {
      var indexDirectory = Path.GetFullPath (WebConfigurationManager.AppSettings ["IndexDirectory"]);
     
      var fileName = Path.Combine (indexDirectory, key + ".security");
      
      var value = "";
      
      if (File.Exists (fileName))
        value = File.ReadAllText (fileName).Trim ();
        
      return value;
    }

    public void Save_Click (object sender, EventArgs e)
    {
      Settings.MqttHost = MqttHost.Text;
      Settings.MqttUsername = MqttUsername.Text;
      Settings.MqttPassword = MqttPassword.Text;
      Settings.MqttPort = Convert.ToInt32(MqttPort.Text);

      var isLocal = IsRunningLocally.Checked;

      var isValid = false;

      if (!isLocal)
      {
        var tester = new MqttConnectionTester();
        isValid = tester.IsValid(Settings);
      }
      else
        isValid = true;

      if (!isValid && !isLocal)
      {
        Result = "Failed to connect to MQTT broker. Invalid settings.";
        IsSuccess = false;
      }
      else if (Manager.SaveMqttSettings(Settings))
      {
        var message = "MQTT settings updated!";

        Response.Redirect("Settings.aspx?Result=" + HttpUtility.UrlEncode(message));
      }
      else
      {
        Result = "Failed to update MQTT settings.";
        IsSuccess = false;
      }
    }

    public void Apply_Click (object sender, EventArgs e)
    {
      //Manager.NetworkSetup ("Local");
      
      //var message = "Your network connection has been updated!";
      //Response.Redirect ("Settings.aspx?Result=" + HttpUtility.UrlEncode (message));
      
     // Stage = 3;
    }
  }
}

