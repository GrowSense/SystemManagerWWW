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

    public ComputerInfo[] GetComputersInfo ()
    {
      var list = new List<ComputerInfo> ();
      
      var localComputerInfo = new ComputerInfo ();
      localComputerInfo.Host = "localhost";
        
      list.Add (localComputerInfo);
        
      foreach (var dir in Directory.GetDirectories(ComputersDirectory)) {
        var computerInfo = new ComputerInfo ();
        computerInfo.Host = File.ReadAllText (Path.Combine (dir, "host.txt"));
        list.Add (computerInfo);
      }
      
      return list.ToArray ();
    }
  }
}

