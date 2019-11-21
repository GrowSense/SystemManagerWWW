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
    public ProcessStarter Starter;

    public DeviceManager (string indexDirectory, string devicesDirectory)
    {
      IndexDirectory = indexDirectory;
      DevicesDirectory = devicesDirectory;
      
      if (!Directory.Exists (DevicesDirectory))
        Directory.CreateDirectory (DevicesDirectory);
      
      Starter = new ProcessStarter (indexDirectory);
    }

    public int CountDevices ()
    {
      if (!Directory.Exists (DevicesDirectory))
        return 0;
      return Directory.GetDirectories (DevicesDirectory).Length;
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

    public bool RemoveDevice (string deviceName)
    {
      Starter.Start (
        String.Format ("bash remove-device.sh {0}",
                       deviceName)
      );
      
      return !Starter.IsError;
    }

    public bool SetDeviceLabel (string deviceName, string deviceLabel)
    {
      var indexDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["IndexDirectory"]);
    
      var starter = new ProcessStarter ();
      starter.WorkingDirectory = indexDirectory;
      starter.StartBash ("bash set-device-label.sh " + deviceName + " " + deviceLabel);
      
      return !starter.IsError;
    }
  }
}

