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
              
      list.Add (GetComputerInfo ("Local"));
        
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
      
      if (computerName == "Local") {
        computerInfo.Name = "Local";
        computerInfo.Host = GetHostName ();
      } else {
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
      }
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

    public ServiceStatus GetServiceStatus (string computerName, string serviceName)
    {
      var command = "systemctl status " + serviceName;
      if (!computerName.ToLower ().Contains ("local"))
        command = "bash run-on-remote.sh " + computerName + " " + command;
    
      var statusText = GetServiceStatusText (computerName, serviceName);
    
      var status = ServiceStatus.NotSet;
      if (statusText.Contains ("Active: active"))
        status = ServiceStatus.Active;
      if (statusText.Contains ("Active: dead"))
        status = ServiceStatus.Dead;
      if (statusText.Contains ("Active: inactive"))
        status = ServiceStatus.Inactive;
      if (statusText.Contains ("Active: failed"))
        status = ServiceStatus.Failed;
      if (statusText.Contains ("Reason: No such file"))
        status = ServiceStatus.NotFound;
        
      return status;
    }

    public string GetServiceStatusText (string computerName, string serviceName)
    {
      var command = "systemctl status " + serviceName;
      var isRemote = false;
      if (!computerName.ToLower ().Contains ("local")) {
        isRemote = true;
        command = "bash run-on-remote.sh " + computerName + " " + command;
      }
    
      Starter.Start (command);
      
      var output = Starter.Output;
      
      Starter.ClearOutput ();
      
      if (isRemote) {
        var preText = "Launching command on remote computer...";
        var postText = "Finished running command on remote computer.";
        var startPosition = output.IndexOf (preText) + preText.Length + 1;
        output = output.Substring (startPosition, output.Length - startPosition);
        output = output.Replace (postText, "");
      }
    
      return output;
    }

    public string RunServiceAction (string computerName, string action, string serviceName)
    {
      var command = "systemctl " + action + " " + serviceName;
      var isRemote = false;
      if (!computerName.ToLower ().Contains ("local")) {
        isRemote = true;
        command = "bash run-on-remote.sh " + computerName + " " + command;
      }
    
      Starter.Start (command);
      
      var output = Starter.Output;
      
      Starter.ClearOutput ();
      
      if (isRemote) {
        var preText = "Launching command on remote computer...";
        var postText = "Finished running command on remote computer.";
        var startPosition = output.IndexOf (preText) + preText.Length + 1;
        output = output.Substring (startPosition, output.Length - startPosition);
        output = output.Replace (postText, "");
      }
    
      return output;
    }

    public string GetHostName ()
    {
      Starter.Start ("cat /etc/hostname");
      return Starter.Output.Trim ();
    }
  }
}

