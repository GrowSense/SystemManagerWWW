using GrowSense.SystemManager.Web;
using System.Net.Sockets;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using System.IO;
using System.Configuration;

namespace WWW
{
  using System;
  using System.Collections;
  using System.ComponentModel;
  using System.Web;
  using System.Web.SessionState;

  public class Global : System.Web.HttpApplication
  {
		
    protected void Application_Start (Object sender, EventArgs e)
    {
    }

    protected void Session_Start (Object sender, EventArgs e)
    {
      LoadVersions ();
      var pageName = Path.GetFileName (Request.Path);
      if (pageName != "MqttConnectionFailure.aspx") {
        DeviceMqttHolder.Initialize ();
      }
    }

    protected void Application_BeginRequest (Object sender, EventArgs e)
    {
      var pageName = Path.GetFileName (Request.Path);
      if (pageName != "MqttConnectionFailure.aspx") {
        DeviceMqttHolder.Initialize ();
        DeviceMqttHolder.EnsureConnected ();
      }
      
      LoadVersions ();
    }

    protected void Application_EndRequest (Object sender, EventArgs e)
    {
    }

    protected void Application_AuthenticateRequest (Object sender, EventArgs e)
    {
    }

    protected void Application_Error (Object sender, EventArgs e)
    {
      var error = Server.GetLastError ();
    
      if (error is MqttConnectionException
        || error is MqttCommunicationException
        || error is SocketException) {
        var pageName = Path.GetFileName (Request.Path);
        if (pageName != "MqttConnectionFailure.aspx")
          HttpContext.Current.Response.Redirect ("MqttConnectionFailure.aspx");
      }
    }

    protected void Session_End (Object sender, EventArgs e)
    {
    }

    protected void Application_End (Object sender, EventArgs e)
    {
      DeviceMqttHolder.End ();
    }

    protected void LoadVersions ()
    {
      LoadSystemVersion ();
      LoadWebUIVersion ();
    }

    protected void LoadSystemVersion ()
    {
      if (String.IsNullOrEmpty ((string)Application ["SystemVersion"])) {
        var indexDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["IndexDirectory"]);
        var versionNumber = File.ReadAllText (Path.Combine (indexDirectory, "version.txt")).Trim ();
        var buildNumber = File.ReadAllText (Path.Combine (indexDirectory, "buildnumber.txt")).Trim ();
        var fullVersion = versionNumber + "-" + buildNumber;
        fullVersion = fullVersion.Replace ("-", ".");
        Application.Add ("SystemVersion", fullVersion);
      }
    }

    protected void LoadWebUIVersion ()
    {
      if (String.IsNullOrEmpty ((string)Application ["WebUIVersion"])) {
        var baseDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["BaseDirectory"]);
        var versionNumber = File.ReadAllText (Path.Combine (baseDirectory, "version.txt")).Trim ();
        var buildNumber = File.ReadAllText (Path.Combine (baseDirectory, "buildnumber.txt")).Trim ();
        var fullVersion = versionNumber + "-" + buildNumber;
        fullVersion = fullVersion.Replace ("-", ".");
        Application.Add ("WebUIVersion", fullVersion);
      }
    }
  }
}

