using System;
using System.Web;
using System.Web.UI;
using GrowSense.SystemManager.Devices;
using System.IO;
using System.Configuration;
using Newtonsoft.Json;
using GrowSense.SystemManager.Web;
using System.Linq;

namespace GrowSense.SystemManager.WWW
{

  public partial class DevicesJson : System.Web.UI.Page
  {
    public void Page_Load(object sender, EventArgs e)
    {
      var deviceName = "";

      if (!String.IsNullOrEmpty(Request.QueryString["Name"]))
        deviceName = Request.QueryString["Name"];
        
      var indexDirectory = Path.GetFullPath(ConfigurationSettings.AppSettings["IndexDirectory"]);
      var devicesDirectory = Path.GetFullPath(ConfigurationSettings.AppSettings["DevicesDirectory"]);

      var deviceManager = new DeviceManager(indexDirectory, devicesDirectory);

      var json = "";

      if (String.IsNullOrEmpty(deviceName))
      {
        var devices = deviceManager.GetDevicesInfo();

        foreach (var device in devices)
        {
          device.Data = DeviceMqttHolder.Current.Data.Where(p => p.Key == device.Name).Select(p=>p.Value).FirstOrDefault();
        }

        json = JsonConvert.SerializeObject(devices);
      }
      else
      {
        var device = deviceManager.GetDeviceInfo(deviceName);
        
        device.Data = DeviceMqttHolder.Current.Data.Where(p => p.Key == device.Name).Select(p=>p.Value).FirstOrDefault();

        json = JsonConvert.SerializeObject(device);
      }

      Response.Write(json);

      Response.End();
    }
  }
}
