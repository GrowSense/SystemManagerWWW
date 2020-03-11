using System.IO;
using System.Configuration;
using GrowSense.SystemManager.Messages;
using System.Web.Configuration;

namespace GrowSense.SystemManager.WWW
{
  using System;
  using System.Web;
  using System.Web.UI;

  public partial class Messages : System.Web.UI.Page
  {
    public MessageInfo[] MessagesInfo = new MessageInfo[] { };

    public void Page_Load (object sender, EventArgs e)
    {
      LoadMessagesInfo ();
    }

    public void LoadMessagesInfo ()
    {
      var indexDirectory = Path.GetFullPath (WebConfigurationManager.AppSettings ["IndexDirectory"]);
    
      var messagesDirectory = Path.GetFullPath (WebConfigurationManager.AppSettings ["MessagesDirectory"]);
    
      var messageManager = new MessageManager(indexDirectory, messagesDirectory);
      MessagesInfo = messageManager.GetMessagesInfo ();
    }
    
    public string GenerateMessageTypeIcon (MessageType type)
    {
      var cssClass = "label-info";
    
      if (type == MessageType.Message)
        cssClass = "label-success";
      if (type == MessageType.Alert)
        cssClass = "label-danger";
    
      return String.Format("<div class=\"label {0} label-mini\">{1}</div>",
        cssClass,
        type.ToString()
        );
    }
  }
}

