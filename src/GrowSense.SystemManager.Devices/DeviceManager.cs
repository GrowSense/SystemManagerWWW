using System;
using System.IO;
using System.Collections.Generic;
using GrowSense.SystemManager.Common;

namespace GrowSense.SystemManager.Devices
{
  public class DeviceManager
  {
    public string DevicesDirectory = Path.GetFullPath ("devices");

    public DeviceManager (string devicesDirectory)
    {
      DevicesDirectory = devicesDirectory;
    }

    public DeviceInfo[] GetDevicesInfo ()
    {
      var list = new List<DeviceInfo> ();
      foreach (var deviceDirectory in Directory.GetDirectories(DevicesDirectory)) {
        var deviceInfo = GetDeviceInfo (Path.GetFileNameWithoutExtension (deviceDirectory));
        list.Add (deviceInfo);
      }
      return list.ToArray ();
    }

    public DeviceInfo GetDeviceInfo (string deviceName)
    {
      var deviceDirectory = Path.Combine (DevicesDirectory, deviceName);
      var deviceInfo = new DeviceInfo ();
      deviceInfo.Name = Path.GetFileNameWithoutExtension (deviceDirectory);
      deviceInfo.Label = File.ReadAllText (Path.Combine (deviceDirectory, "label.txt"));
      deviceInfo.Group = File.ReadAllText (Path.Combine (deviceDirectory, "group.txt"));
      deviceInfo.Project = File.ReadAllText (Path.Combine (deviceDirectory, "project.txt"));
      return deviceInfo;
    }

    public void SetDeviceLabel (string deviceName, string deviceLabel)
    {
      var starter = new ProcessStarter ();
      starter.Start ("bash set-device-label.sh", deviceName, deviceLabel);
      
      if (starter.IsError)
        throw new Exception ("Failed to set device label using set-device-label.sh script.\n\n" + starter.Output);
    }
  }
}

