using GrowSense.SystemManager.Computers;
using System.IO;
using System.Configuration;
using GrowSense.SystemManager.Devices;
using GrowSense.SystemManager.Web;

namespace GrowSense.SystemManager.WWW
{
  using System;
  using System.Web;
  using System.Web.UI;

  public partial class ComputerTools : System.Web.UI.Page
  {
    public ComputerManager Manager;
    public DeviceManager DeviceManager;
    public ComputerInfo Computer;
    //public string ComputerName;
    public string ErrorMessage = String.Empty;
    public DeviceInfo[] Devices = new DeviceInfo[] { };

    public void Page_Load (object sender, EventArgs e)
    {
      var indexDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["IndexDirectory"]);
    
      var computersDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["ComputersDirectory"]);
    
      var devicesDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["DevicesDirectory"]);
    
      Manager = new ComputerManager (indexDirectory, computersDirectory);
      DeviceManager = new DeviceManager (indexDirectory, devicesDirectory);
     
      var computerName = Request.QueryString ["ComputerName"];
    
      Computer = Manager.GetComputerInfo (computerName);
      
      Devices = DeviceManager.GetDevicesInfo (Computer.Host);
    }

    public string GeneratePlugAndPlayServiceStatusIcon ()
    {      
      var status = Manager.GetServiceStatus (Computer.Name, "arduino-plug-and-play.service");
      
      return GenerateServiceStatusIcon (status);
    }

    public string GenerateSupervisorServiceStatusIcon ()
    {      
      var status = Manager.GetServiceStatus (Computer.Name, "growsense-supervisor.service");
      
      return GenerateServiceStatusIcon (status);
    }

    public string GenerateMeshManagerServiceStatusIcon ()
    {
      var status = Manager.GetServiceStatus (Computer.Name, "growsense-mesh-manager.service");
      
      return GenerateServiceStatusIcon (status);
    }
    
    public string GenerateWebUIServiceStatusIcon ()
    {
      var status = Manager.GetServiceStatus (Computer.Name, "growsense-www.service");
      
      return GenerateServiceStatusIcon (status);
    }
    
    public string GenerateUpgradeServiceStatusIcon ()
    {
      var status = Manager.GetServiceStatus (Computer.Name, "growsense-upgrade.service");
      
      return GenerateServiceStatusIcon (status);
    }
    
    
    public string GenerateDeviceServiceStatusIcon (DeviceInfo device)
    {
      var serviceName = "";
      var status = ServiceStatus.NotSet;
      
      if (device.Board == "esp")
        status = ServiceStatus.NotRequired;
      else {
        var statusMessage = "";
        if (DeviceMqttHolder.Current.Data.ContainsKey(device.Name) && DeviceMqttHolder.Current.Data[device.Name].ContainsKey("StatusMessage"))
          statusMessage = DeviceMqttHolder.Current.Data[device.Name]["StatusMessage"];
        var isConnected = statusMessage != "Disconnected";
        if (!isConnected)
          status = ServiceStatus.Disconnected;
        else
        {
          serviceName = GetDeviceServiceName (device);
    
          status = Manager.GetServiceStatus (Computer.Name, serviceName);
        }
      }
      
      return GenerateServiceStatusIcon (status);
    }

    public string GenerateServiceStatusIcon (ServiceStatus status)
    {
      var cssClass = "label-info";
      var statusText = "NotSet";
      
      if (status == ServiceStatus.Active)
        cssClass = "label-success";
      else if (status == ServiceStatus.NotRequired)
        cssClass = "label-info";
      else
        cssClass = "label-danger";
      
      statusText = status.ToString ();
      if (status == ServiceStatus.NotRequired)
        statusText = "Not Required";
      if (status == ServiceStatus.NotFound)
        statusText = "Not Found";
      if (status == ServiceStatus.NotSet)
        statusText = "Not Set";
      
      return String.Format ("<div class=\"label {0} label-mini\">{1}</div>",
                            cssClass,
                            statusText
      );
    }
    
    public string GetDeviceServiceName(DeviceInfo device)
    {
      var serviceName = "";
    
      if (device.Group == "ui")
        serviceName = "growsense-ui-1602-" + device.Name + ".service";
      else
        serviceName = "growsense-mqtt-bridge-" + device.Name + ".service";
          
      return serviceName;
    }
    
    public string GetViewServicePath(string serviceName)
    {
      return "ViewService.aspx?Computer=" + Computer.Name + "&Service=" + serviceName;
    }
    
    public string GetServiceActionPath(string action, string serviceName, string label)
    {
      return "ServiceAction.aspx?Computer=" + Computer.Name + "&Action=" + action + "&Service=" + serviceName + "&Label=" + label;
    }
    
    public string GetViewDeviceServicePath(DeviceInfo device)
    {
      var serviceName = GetDeviceServiceName(device);
    
      return "ViewService.aspx?Computer=" + Computer.Name + "&Service=" + serviceName;
    }
    
    public string GetDeviceServiceActionPath(DeviceInfo device, string action)
    {
      var serviceName = GetDeviceServiceName(device);
    
      return "ServiceAction.aspx?Computer=" + Computer.Name + "&Action=" + action + "&Service=" + serviceName + "&Label=" + device.Label + "+device";
    }
  }
}

