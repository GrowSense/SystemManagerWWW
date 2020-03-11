using System.Configuration;
using System.Web.Configuration;

namespace GrowSense.SystemManager.WWW
{
  using System;
  using System.Web;
  using System.Web.UI;

  public partial class MqttConnectionFailure : System.Web.UI.Page
  {
    public string MqttHost;
    public string MqttUsername;
    public int MqttPort;

    public void Page_Load (object sender, EventArgs e)
    {
      MqttHost = WebConfigurationManager.AppSettings ["MqttHost"];
      MqttUsername = WebConfigurationManager.AppSettings ["MqttUsername"];
      MqttPort = Convert.ToInt32 (WebConfigurationManager.AppSettings ["MqttPort"]);
    }
  }
}

