using System;
using System.Collections.Generic;
using System.IO;

namespace GrowSense.SystemManager.Computers
{
  public class ComputerManager
  {
    public string ComputersDirectory;

    public ComputerManager (string computersDirectory)
    {
      ComputersDirectory = computersDirectory;
    }

    public int CountComputers ()
    {
      if (!Directory.Exists (ComputersDirectory))
        return 1;
      return Directory.GetDirectories (ComputersDirectory).Length + 1; // +1 to include localhost
    }

    public ComputerInfo[] GetComputersInfo ()
    {
      var list = new List<ComputerInfo> ();
      
      var localComputerInfo = new ComputerInfo ();
      localComputerInfo.Name = "localhost";
      localComputerInfo.Host = "127.0.0.1";
        
      list.Add (localComputerInfo);
        
      if (Directory.Exists (ComputersDirectory)) {
        foreach (var dir in Directory.GetDirectories(ComputersDirectory)) {
          var computerInfo = new ComputerInfo ();
          computerInfo.Name = File.ReadAllText (Path.Combine (dir, "name.security"));
          computerInfo.Host = File.ReadAllText (Path.Combine (dir, "host.security"));
          list.Add (computerInfo);
        }
      }
      
      return list.ToArray ();
    }
  }
}

