using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GrowSense.SystemManager.Devices
{
  public class DeviceInfo
  {
    public string Name;
    public string Label;
    public string Group;
    public string Project;
    public string Board;
    public string Host;

    public Dictionary<string, string> Data = new Dictionary<string, string>();
  }
}

