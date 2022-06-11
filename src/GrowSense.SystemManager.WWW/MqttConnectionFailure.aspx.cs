using System.Configuration;
using System.Web.Configuration;
using GrowSense.SystemManager.Common;
using System.IO;

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

        public SettingsManager Manager;
        public SystemSettings Settings;

        public void Page_Load(object sender, EventArgs e)
        {
            var indexDirectory = Path.GetFullPath(WebConfigurationManager.AppSettings["IndexDirectory"]);

            Manager = new SettingsManager(indexDirectory);
            Settings = Manager.LoadSettings();

            MqttHost = Settings.MqttHost;
            MqttUsername = Settings.MqttUsername;
            MqttPort = Settings.MqttPort;
        }
    }
}
