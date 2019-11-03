using System;

namespace GrowSense.SystemManager.Computers
{
  public enum ComputerActionError
  {
    NotSet,
    NotFound,
    NameInUse,
    HostAlreadyExists,
    PingFailed,
    ConnectionFailed,
    PermissionDenied
  }
}

