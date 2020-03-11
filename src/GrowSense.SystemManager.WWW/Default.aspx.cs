using GrowSense.SystemManager.Devices;
using System.Configuration;
using System.IO;
using GrowSense.SystemManager.Computers;
using GrowSense.SystemManager.Messages;
using System.Web.Configuration;

namespace GrowSense.SystemManager
{
  using System;
  using System.Web;
  using System.Web.UI;

  public partial class Default : System.Web.UI.Page
  {
    public int TotalDevices = 0;
    public int TotalComputers = 0;
    public int TotalMessages = 0;
    public int TotalAlerts = 0;

    public void Page_Load (object sender, EventArgs e)
    {
      var indexDirectory = Path.GetFullPath (WebConfigurationManager.AppSettings ["IndexDirectory"]);
      var devicesDirectory = Path.GetFullPath (WebConfigurationManager.AppSettings ["DevicesDirectory"]);
      var messagesDirectory = Path.GetFullPath (WebConfigurationManager.AppSettings ["MessagesDirectory"]);
      var computersDirectory = Path.GetFullPath (WebConfigurationManager.AppSettings ["ComputersDirectory"]);
    
      TotalDevices = new DeviceManager (indexDirectory, devicesDirectory).CountDevices ();
      TotalMessages = new MessageManager (indexDirectory, messagesDirectory).CountMessages (MessageType.Message);
      TotalAlerts = new MessageManager (indexDirectory, messagesDirectory).CountMessages (MessageType.Alert);
      TotalComputers = new ComputerManager (indexDirectory, computersDirectory).CountComputers ();
    }
  }
}

