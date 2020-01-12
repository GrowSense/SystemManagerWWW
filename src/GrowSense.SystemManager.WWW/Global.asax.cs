using GrowSense.SystemManager.Web;
using System.Net.Sockets;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using System.IO;

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
      DeviceMqttHolder.Initialize ();
    }

    protected void Application_BeginRequest (Object sender, EventArgs e)
    {
      var pageName = Path.GetFileName (Request.Path);
      if (pageName != "MqttConnectionFailure.aspx")
        DeviceMqttHolder.EnsureConnected ();
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
  }
}

