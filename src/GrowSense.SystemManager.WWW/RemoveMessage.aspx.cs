using System.IO;
using System.Configuration;
using GrowSense.SystemManager.Messages;

namespace GrowSense.SystemManager.WWW
{
  using System;
  using System.Web;
  using System.Web.UI;

  public partial class RemoveMessage : System.Web.UI.Page
  {
    public void Page_Load (object sender, EventArgs e)
    {
      var messageId = Request.QueryString ["MessageId"];
      
      var resultMessage = "";
      
      if (String.IsNullOrEmpty (messageId)) {
        resultMessage = ("No message ID specified in the query string.");
        Response.Redirect ("Messages.aspx?Result=" + resultMessage);
      }
      var indexDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["IndexDirectory"]);
    
      var messagesDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["MessagesDirectory"]);
    
      var manager = new MessageManager (indexDirectory, messagesDirectory);
      
      var isSuccess = manager.RemoveMessage (messageId);
      
      var postFixQueryString = "";
      if (isSuccess)
        resultMessage = "Message was removed successfully!";
      else {
        resultMessage = "Message couldn't be removed!";
        postFixQueryString = "&IsSuccess=false";
      }
      
      Response.Redirect ("Messages.aspx?Result=" + (resultMessage).Replace (" ", "+") + postFixQueryString);
 
    }
  }
}

