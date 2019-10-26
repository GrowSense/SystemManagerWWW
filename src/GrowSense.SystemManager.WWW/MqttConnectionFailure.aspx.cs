using System.Configuration;

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
      MqttHost = ConfigurationSettings.AppSettings ["MqttHost"];
      MqttUsername = ConfigurationSettings.AppSettings ["MqttUsername"];
      MqttPort = Convert.ToInt32 (ConfigurationSettings.AppSettings ["MqttPort"]);
    }
  }
}

