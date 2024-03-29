﻿using System;
using Newtonsoft.Json;
using System.IO;
namespace GrowSense.SystemManager.Common
{
  public class SettingsManager
  {
    public string WorkingDirectory;

    public SettingsManager(string workingDirectory)
    {
      WorkingDirectory = workingDirectory;
    }

    public SystemSettings LoadSettings()
    {
      Console.WriteLine("Loading settings...");

      var settingsFile = GetSettingsFilePath();

      Console.WriteLine("  Path: " + settingsFile);

      if (File.Exists(settingsFile))
      {
        Console.WriteLine("  Settings file exists.");
        var settingsJson = File.ReadAllText(settingsFile);
        var settings = JsonConvert.DeserializeObject<SystemSettings>(settingsJson);

        var starter = new ProcessStarter(WorkingDirectory);
        starter.StartBash("hostname");
        settings.HostName = starter.Output.Trim();

        return settings;
      }
      else
      {
        Console.WriteLine("  Settings file doesn't exist. Loading from legacy settings files...");
        var settings = new SystemSettings();


        // Load settings from legacy files if they exist
        if (File.Exists(WorkingDirectory + "/mqtt-host.security"))
          settings.MqttHost = File.ReadAllText(WorkingDirectory + "/mqtt-host.security").Trim();
        if (File.Exists(WorkingDirectory + "/mqtt-username.security"))
          settings.MqttUsername = File.ReadAllText(WorkingDirectory + "/mqtt-username.security").Trim();
        if (File.Exists(WorkingDirectory + "/mqtt-password.security"))
          settings.MqttPassword = File.ReadAllText(WorkingDirectory + "/mqtt-password.security").Trim();
        if (File.Exists(WorkingDirectory + "/mqtt-port.security"))
          settings.MqttPort = Convert.ToInt32(File.ReadAllText(WorkingDirectory + "/mqtt-port.security").Trim());

        if (File.Exists(WorkingDirectory + "/wifi-name.security"))
          settings.WiFiName = File.ReadAllText(WorkingDirectory + "/wifi-name.security").Trim();
        if (File.Exists(WorkingDirectory + "/wifi-password.security"))
          settings.WiFiPassword = File.ReadAllText(WorkingDirectory + "/wifi-password.security").Trim();

        if (File.Exists(WorkingDirectory + "/smtp-server.security"))
          settings.SmtpServer = File.ReadAllText(WorkingDirectory + "/smtp-server.security").Trim();
        if (File.Exists(WorkingDirectory + "/smtp-username.security"))
          settings.SmtpUsername = File.ReadAllText(WorkingDirectory + "/smtp-username.security").Trim();
        if (File.Exists(WorkingDirectory + "/smtp-password.security"))
          settings.SmtpPassword = File.ReadAllText(WorkingDirectory + "/smtp-password.security").Trim();
        if (File.Exists(WorkingDirectory + "/smtp-port.security"))
          settings.SmtpPort = Convert.ToInt32(File.ReadAllText(WorkingDirectory + "/smtp-port.security").Trim());
        if (File.Exists(WorkingDirectory + "/admin-email.security"))
          settings.Email = File.ReadAllText(WorkingDirectory + "/admin-email.security").Trim();

        return settings;
      }
    }

    public bool SaveLoginSettings(SystemSettings settings)
    {
      var starter = new ProcessStarter(WorkingDirectory);

      var command = String.Format(
        "bash gs.sh config --username={0} --password={1}",
        settings.Username,
        settings.Password
      );

      starter.StartBash(command);

      var isSuccess = !starter.IsError;

      return isSuccess;      
    }

    public bool SaveMqttSettings(SystemSettings settings)
    {
      var starter = new ProcessStarter(WorkingDirectory);

      var command = String.Format(
        "bash gs.sh config --mqtt-host={0} --mqtt-username={1} --mqtt-password={2} --mqtt-port={3}",
        settings.MqttHost,
        settings.MqttUsername,
        settings.MqttPassword,
        settings.MqttPort
      );

      starter.StartBash(command);

      var isSuccess = !starter.IsError;

      return isSuccess;      
    }

    // TODO: Remove if not needed
    /*public void SaveSettings(Settings settings)
    {
      Console.WriteLine("Saving settings...");
      
      var json = JsonConvert.SerializeObject(settings, Formatting.Indented);

      var settingsFile = GetSettingsFilePath();

      Console.WriteLine("  Path: " + settingsFile);

      File.WriteAllText(settingsFile, json);

      SaveSettingsToLegacyFiles(settings);
    }

    public void SaveSettingsToLegacyFiles(CLISettings settings)
    {
      // TODO: Phase this out once scripts no longer rely on it.
    
      var basePath = WorkingDirectory;

      File.WriteAllText(basePath + "/mqtt-host.security", settings.MqttHost);
      File.WriteAllText(basePath + "/mqtt-username.security", settings.MqttUsername);
      File.WriteAllText(basePath + "/mqtt-password.security", settings.MqttPassword);
      File.WriteAllText(basePath + "/mqtt-port.security", settings.MqttPort.ToString());
      
      File.WriteAllText(basePath + "/wifi-name.security", settings.WiFiName);
      File.WriteAllText(basePath + "/wifi-password.security", settings.WiFiPassword);
      
      File.WriteAllText(basePath + "/smtp-server.security", settings.SmtpServer);
      File.WriteAllText(basePath + "/smpt-username.security", settings.SmtpUsername);
      File.WriteAllText(basePath + "/smtp-password.security", settings.SmtpPassword);
      File.WriteAllText(basePath + "/smtp-port.security", settings.SmtpPort.ToString());
      File.WriteAllText(basePath + "/admin-email.security", settings.Email);
    }*/

    public string GetSettingsFilePath()
    {
      return Path.Combine(WorkingDirectory, "growsense.settings");
    }
  }
}
