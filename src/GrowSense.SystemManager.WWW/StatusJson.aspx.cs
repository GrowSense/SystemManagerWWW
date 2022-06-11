using System;
using System.Web;
using System.Web.UI;
using GrowSense.SystemManager.Common;
using Newtonsoft.Json;

namespace GrowSense.SystemManager.WWW
{

    public partial class StatusJson : System.Web.UI.Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            var statusValue = SystemStatusEnum.Online;

            var statusInfo = new SystemStatus(statusValue);

            var json = JsonConvert.SerializeObject(statusInfo);

            Response.Write(json);
            Response.End();
        }
    }
}
