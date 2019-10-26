using System;
using System.IO;
using System.Collections.Generic;
using GrowSense.SystemManager.Common;
using System.Configuration;

namespace GrowSense.SystemManager.Devices
{
  public class DeviceManager
  {
    public string IndexDirectory;
    public string DevicesDirectory;

    public DeviceManager (string indexDirectory, string devicesDirectory)
    {
      IndexDirectory = indexDirectory;
      DevicesDirectory = devicesDirectory;
    }

    public DeviceInfo[] GetDevicesInfo ()
    {
      var list = new List<DeviceInfo> ();
      if (Directory.Exists (DevicesDirectory)) {
        foreach (var deviceDirectory in Directory.GetDirectories(DevicesDirectory)) {
          var deviceInfo = GetDeviceInfo (Path.GetFileNameWithoutExtension (deviceDirectory));
          list.Add (deviceInfo);
        }
      }
      return list.ToArray ();
    }

    public DeviceInfo GetDeviceInfo (string deviceName)
    {
      var deviceDirectory = Path.Combine (DevicesDirectory, deviceName);
      var deviceInfo = new DeviceInfo ();
      deviceInfo.Name = Path.GetFileNameWithoutExtension (deviceDirectory);
      deviceInfo.Label = File.ReadAllText (Path.Combine (deviceDirectory, "label.txt")).Trim ();
      deviceInfo.Group = File.ReadAllText (Path.Combine (deviceDirectory, "group.txt")).Trim ();
      deviceInfo.Project = File.ReadAllText (Path.Combine (deviceDirectory, "project.txt")).Trim ();
      return deviceInfo;
    }

    public void SetDeviceLabel (string deviceName, string deviceLabel)
    {
      var indexDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["IndexDirectory"]);
    
      var starter = new ProcessStarter ();
      starter.WorkingDirectory = indexDirectory;
      starter.StartBash ("bash set-device-label.sh " + deviceName + " " + deviceLabel);
      
      if (starter.IsError)
        throw new Exception ("Failed to set device label using set-device-label.sh script.\n\n" + starter.Output);
    }
  }
}

