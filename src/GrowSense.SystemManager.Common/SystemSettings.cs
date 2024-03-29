﻿using System;
namespace GrowSense.SystemManager.Common
{
  public class SystemSettings
  {
    public string Branch = "dev";
    public string Version = "0.0.0.1";

    public string Username = "user";
    public string Password = "pass123";

    public string MqttHost = "127.0.0.1";
    public string MqttUsername = "user";
    public string MqttPassword = "pass123";
    public int MqttPort = 1883;

    public string WiFiName = "";
    public string WiFiPassword = "";

    public string SmtpServer = "";
    public string SmtpUsername = "";
    public string SmtpPassword = "";
    public int SmtpPort = 25;
    
    public string Email = "";
    
    public string HostName { get; set; }

    public bool IsMockSystemCtl = false;
    public bool IsMockDocker = false;
    
    public SystemSettings()
    {
    }

  }
}
