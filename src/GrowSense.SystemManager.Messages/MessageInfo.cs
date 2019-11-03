using System;

namespace GrowSense.SystemManager.Messages
{
  public struct MessageInfo
  {
    public string Id;
    public string Text;
    public DateTime Timestamp;
    public MessageType Type;
    public string FileName;
    public string Host;
  }
}

