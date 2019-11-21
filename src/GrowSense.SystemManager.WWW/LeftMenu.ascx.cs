using System.IO;

namespace GrowSense.SystemManager
{
  using System;
  using System.Web;
  using System.Web.UI;

  public partial class LeftMenu : System.Web.UI.UserControl
  {
    public string GetActiveClassAttribute (string pageName)
    {
      var currentPageName = Path.GetFileNameWithoutExtension (Request.PhysicalPath);
      if (currentPageName == pageName)
        return " class='active'";
      else
        return "";
    }
  }
}

