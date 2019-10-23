using System;
using System.IO;
using System.Collections.Generic;

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
        var deviceInfo = new DeviceInfo ();
        deviceInfo.Name = Path.GetFileNameWithoutExtension (deviceDirectory);
        deviceInfo.Label = File.ReadAllText (Path.Combine (deviceDirectory, "label.txt"));
        deviceInfo.Group = File.ReadAllText (Path.Combine (deviceDirectory, "group.txt"));
        deviceInfo.Project = File.ReadAllText (Path.Combine (deviceDirectory, "project.txt"));
        list.Add (deviceInfo);
      }
      return list.ToArray ();
    }
  }
}

