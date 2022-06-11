using System;
namespace GrowSense.SystemManager.Common
{
    public class SystemStatus
    {
        public int StatusCode { get; set; }
        public string StatusText { get; set; }

        public SystemStatus(SystemStatusEnum statusValue)
        {
            StatusCode = (int)statusValue;
            StatusText = statusValue.ToString();
        }
    }
}
