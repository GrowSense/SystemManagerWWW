using System;
using System.IO;
using System.Collections.Generic;

namespace GrowSense.SystemManager.Messages
{
  public class MessageManager
  {
    public string IndexDirectory = String.Empty;
    public string MessagesDirectory = String.Empty;

    public MessageManager (string indexDirectory, string messagesDirectory)
    {
      IndexDirectory = indexDirectory;
      MessagesDirectory = messagesDirectory;
    }

    public MessageInfo[] GetMessagesInfo ()
    {
      var list = new List<MessageInfo> ();
      if (Directory.Exists (MessagesDirectory)) {
        foreach (var filePath in Directory.GetFiles(MessagesDirectory)) {
          var fileName = Path.GetFileName (filePath);
        
          var message = new MessageInfo ();
          message.Timestamp = ExtractDateTimeFromFileName (fileName);
        
          message.FileName = fileName;
        
          message.Text = File.ReadAllText (filePath);
        
          if (fileName.Contains ("alert"))
            message.Type = MessageType.Alert;
          else
            message.Type = MessageType.Message;
          
          list.Insert (0, message);
        }
      }
      return list.ToArray ();
    }

    public int CountMessages ()
    {
      return CountMessages (MessageType.NotSet);
    }

    public int CountMessages (MessageType type)
    {
      if (!Directory.Exists (MessagesDirectory))
        return 0;
        
      if (type == MessageType.Message)
        return Directory.GetFiles (MessagesDirectory, "*.msg.txt").Length;
      else if (type == MessageType.Alert)
        return Directory.GetFiles (MessagesDirectory, "*.alert.txt").Length;
      else
        return Directory.GetFiles (MessagesDirectory).Length;
    }

    public DateTime ExtractDateTimeFromFileName (string fileName)
    {
      var dateSection = fileName.Substring (0, fileName.IndexOf ("--"));
      
      var dateParts = dateSection.Split ('-');
      
      var time = new DateTime (Convert.ToInt32 (dateParts [0]),
                               Convert.ToInt32 (dateParts [1]),
                               Convert.ToInt32 (dateParts [2]),
                               Convert.ToInt32 (dateParts [3]),
                               Convert.ToInt32 (dateParts [4]),
                               Convert.ToInt32 (dateParts [5]));
      
      return time;
    }
  }
}

