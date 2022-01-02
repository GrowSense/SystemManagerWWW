using System;
using GrowSense.SystemManager.Common;
using System.IO;
using System.Configuration;
namespace GrowSense.SystemManager.Web
{
  public class Authenticator
  {
    public SettingsManager Manager;
    
    public Authenticator()
    {
      var indexDirectory = Path.GetFullPath(ConfigurationSettings.AppSettings["IndexDirectory"]);
    
      Manager = new SettingsManager(indexDirectory);
      
    }

    public bool IsAuthentic(string username, string password)
    {
      var settings = Manager.LoadSettings();

      var isAuthentic = settings.Username.Equals(username) && settings.Password.Equals(password);

      return isAuthentic;
    }
  }
}
