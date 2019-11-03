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
      var sorted = new SortedDictionary<string, MessageInfo> ();
      if (Directory.Exists (MessagesDirectory)) {
        foreach (var filePath in Directory.GetFiles(MessagesDirectory, "*.txt", SearchOption.AllDirectories)) {
          var fileName = Path.GetFileName (filePath);
        
          var message = new MessageInfo ();
          
          message.Id = ExtractIdFromFileName (fileName);
          
          message.Timestamp = ExtractDateTimeFromFileName (fileName);
        
          message.FileName = fileName;
        
          message.Text = File.ReadAllText (filePath);
        
          if (fileName.Contains ("alert"))
            message.Type = MessageType.Alert;
          else
            message.Type = MessageType.Message;
            
          message.Host = ExtractMessageHostFromFilePath (filePath);
          
          var key = message.Timestamp + "--" + message.Id;
          
          sorted.Add (key, message);
        }
      }
      var list = new List<MessageInfo> ();
      foreach (var messageInfo in sorted.Values)
        list.Insert (0, messageInfo);
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
        return Directory.GetFiles (MessagesDirectory, "*.msg.txt", SearchOption.AllDirectories).Length;
      else if (type == MessageType.Alert)
        return Directory.GetFiles (MessagesDirectory, "*.alert.txt", SearchOption.AllDirectories).Length;
      else
        return Directory.GetFiles (MessagesDirectory, "*.txt", SearchOption.AllDirectories).Length;
    }

    public string ExtractMessageHostFromFilePath (string filePath)
    {
      var folderPath = Path.GetDirectoryName (filePath);
    
      var host = "";
    
      if (!folderPath.EndsWith ("msgs"))
        host = Path.GetFileName (folderPath);
      else
        host = "localhost";
    
      return host;
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

    public string ExtractIdFromFileName (string fileName)
    {
      var fileNameWithoutExtension = Path.GetFileNameWithoutExtension (Path.GetFileNameWithoutExtension (fileName));
    
      var startPosition = fileNameWithoutExtension.IndexOf ("--") + 2;
    
      var id = fileNameWithoutExtension.Substring (startPosition, fileNameWithoutExtension.Length - startPosition);
      
      return id;
    }
  }
}

