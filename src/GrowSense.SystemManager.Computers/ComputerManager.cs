using System;
using System.Collections.Generic;
using System.IO;
using GrowSense.SystemManager.Common;

namespace GrowSense.SystemManager.Computers
{
  public class ComputerManager
  {
    public string IndexDirectory;
    public string ComputersDirectory;
    public ProcessStarter Starter;

    public ComputerManager (string indexDirectory, string computersDirectory)
    {
      IndexDirectory = indexDirectory;
      ComputersDirectory = computersDirectory;
      
      Starter = new ProcessStarter ();
      Starter.WorkingDirectory = indexDirectory;
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
      localComputerInfo.Name = "Local";
      localComputerInfo.Host = GetHostName ();
        
      list.Add (localComputerInfo);
        
      if (Directory.Exists (ComputersDirectory)) {
        foreach (var dir in Directory.GetDirectories(ComputersDirectory)) {
          var computerName = Path.GetFileName (dir);
          var computerInfo = GetComputerInfo (computerName);
          list.Add (computerInfo);
        }
      }
      
      return list.ToArray ();
    }

    public ComputerInfo GetComputerInfo (string computerName)
    {
      var computerInfo = new ComputerInfo ();
      
      var computerDirectory = Path.Combine (ComputersDirectory, computerName);
      
      computerInfo.Name = File.ReadAllText (Path.Combine (computerDirectory, "name.security")).Trim ();
      computerInfo.Host = File.ReadAllText (Path.Combine (computerDirectory, "host.security")).Trim ();
      computerInfo.Username = File.ReadAllText (Path.Combine (computerDirectory, "username.security")).Trim ();
      computerInfo.Password = File.ReadAllText (Path.Combine (computerDirectory, "password.security")).Trim ();
      computerInfo.Port = Convert.ToInt32 (File.ReadAllText (Path.Combine (computerDirectory, "port.security")).Trim ());
      if (File.Exists (Path.Combine (computerDirectory, "is-offline.txt")))
        computerInfo.IsOnline = Convert.ToInt32 (File.ReadAllText (Path.Combine (computerDirectory, "is-offline.txt")).Trim ()) == 0;
      else
        computerInfo.IsOnline = true;
      
      return computerInfo;
    }

    public ComputerActionResult AddComputer (ComputerInfo computer)
    {
      Starter.Start (
        String.Format ("bash add-remote-index.sh {0} {1} {2} {3} {4}",
                       computer.Name,
                       computer.Host,
                       computer.Username,
                       computer.Password,
                       computer.Port)
      );
      
      var result = GetResult (Starter);
      
      return result;
    }

    public ComputerActionResult UpdateComputer (string originalName, ComputerInfo computer)
    {
      var nameHasChanged = (originalName != computer.Name);
      if (nameHasChanged) {
        Starter.Start (
          String.Format ("bash set-remote-index-name.sh {0} {1}",
                         originalName,
                         computer.Name)
        );
      }
      
      var renameResult = GetResult (Starter);
    
      if (!renameResult.IsSuccess)
        return renameResult;
    
      Starter.Start (
        String.Format ("bash update-remote-index.sh {0} {1} {2} {3} {4}",
                       computer.Name,
                       computer.Host,
                       computer.Username,
                       computer.Password,
                       computer.Port)
      );
      
      var result = GetResult (Starter);
      
      return result;
    }

    public ComputerActionResult RemoveComputer (string computerName)
    {
      Starter.Start (
        String.Format ("bash remove-remote-index.sh {0}",
                       computerName)
      );
      
      var result = GetResult (Starter);
      
      return result;
    }

    public ComputerActionResult GetResult (ProcessStarter starter)
    {
      var result = new ComputerActionResult ();
    
      var output = starter.Output;
      
      if (!starter.IsError) {
        result.IsSuccess = true;
      } else {
        result.IsSuccess = false;
        if (output.Contains ("wasn't found"))
          result.Error = ComputerActionError.NotFound;
        else if (output.Contains ("offline or cannot be found"))
          result.Error = ComputerActionError.PingFailed;
        else if (output.Contains ("Permission denied"))
          result.Error = ComputerActionError.PermissionDenied;
        else if (output.Contains ("Connection") && output.Contains ("failed"))
          result.Error = ComputerActionError.ConnectionFailed;
        else if (output.Contains ("already in use"))
          result.Error = ComputerActionError.NameInUse;
        else if (output.Contains ("has already been added"))
          result.Error = ComputerActionError.HostAlreadyExists;
      }
      
      return result;
    }

    public string GetHostName ()
    {
      Starter.Start ("cat /etc/hostname");
      return Starter.Output.Trim ();
    }
  }
}

