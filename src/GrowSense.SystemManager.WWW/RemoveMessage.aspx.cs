using System.IO;
using System.Configuration;
using GrowSense.SystemManager.Messages;
using System.Web.Configuration;

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
        Response.Redirect ("Messages.aspx?Result=" + resultMessage + "&IsSuccess=false");
      }
      
      var indexDirectory = Path.GetFullPath (WebConfigurationManager.AppSettings ["IndexDirectory"]);
    
      var messagesDirectory = Path.GetFullPath (WebConfigurationManager.AppSettings ["MessagesDirectory"]);
    
      var manager = new MessageManager (indexDirectory, messagesDirectory);
      
      var isSuccess = false;
      
      if (messageId == "all")
        isSuccess = manager.RemoveAllMessages ();
      else
        isSuccess = manager.RemoveMessage (messageId);
      
      var postFixQueryString = "";
      if (isSuccess) {
        if (messageId == "all")
          resultMessage = "All messages were removed successfully!";
        else
          resultMessage = "Message was removed successfully!";
      } else {
        if (messageId == "all")
          resultMessage = "Error when removing all messages!";
        else
          resultMessage = "Error when removing message!";
        postFixQueryString = "&IsSuccess=false";
      }
      
      Response.Redirect ("Messages.aspx?Result=" + (resultMessage).Replace (" ", "+") + postFixQueryString);
 
    }
  }
}

