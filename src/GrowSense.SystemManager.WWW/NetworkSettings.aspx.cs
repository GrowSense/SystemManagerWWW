using System.IO;
using System.Configuration;
using GrowSense.SystemManager.Computers;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace GrowSense.SystemManager.WWW
{
  using System;
  using System.Web;
  using System.Web.UI;

  public partial class NetworkSettings : System.Web.UI.Page
  {
    public Dictionary<string, DropDownList> DDLS = new Dictionary<string, DropDownList> ();
    public ComputerManager Manager;
    public bool RemoteComputersExist;

    public int Stage {
      get {
        if (ViewState ["Stage"] == null)
          ViewState ["Stage"] = 1;
        return (int)ViewState ["Stage"];
      }
      set { ViewState ["Stage"] = value; }
    }

    public void Page_Load (object sender, EventArgs e)
    {
      var indexDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["IndexDirectory"]);
     
      var computersDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["ComputersDirectory"]);
     
      Manager = new ComputerManager (indexDirectory, computersDirectory);
    
      GenerateComputerFields ();
        
      if (!IsPostBack) {
        PopulateForm ();
      } 
    }

    public void PopulateForm ()
    {
      var localConnectionType = Manager.GetNetworkConnectionType ("Local");
    
      if (localConnectionType == NetworkConnectionType.Ethernet)
        ActivateEthernet.Checked = true;
      
      var wifiName = GetSecurityValue ("wifi-network-name");
      if (!String.IsNullOrEmpty (wifiName) && wifiName != "na")
        WiFiName.Text = wifiName;
          
      var wifiPass = GetSecurityValue ("wifi-network-password");
      if (!String.IsNullOrEmpty (wifiPass) && wifiPass != "na")
        WiFiPass.Text = wifiPass;
        
      if (!String.IsNullOrEmpty (wifiName) && wifiName != "na" && 
        !String.IsNullOrEmpty (wifiPass) && wifiPass != "na")
        ActivateWiFiNetwork.Checked = true;
          
      var hotSpotName = GetSecurityValue ("wifi-hotspot-name");
      if (!String.IsNullOrEmpty (hotSpotName) && hotSpotName != "na")
        HotSpotName.Text = hotSpotName;
      else
        HotSpotName.Text = "GrowSense" + new Random ().Next (10000);
          
      var hotSpotPass = GetSecurityValue ("wifi-hotspot-password");
      if (!String.IsNullOrEmpty (hotSpotPass) && hotSpotPass != "na")
        HotSpotPass.Text = hotSpotPass;
      else
        HotSpotPass.Text = "pass" + new Random ().Next (100000, 999999);
        
      if (!String.IsNullOrEmpty (hotSpotName) && hotSpotName != "na" && 
        !String.IsNullOrEmpty (hotSpotPass) && hotSpotPass != "na" &&
        localConnectionType == NetworkConnectionType.WiFiHotSpot)
        ActivateWiFiHotSpot.Checked = true;
    }

    public string GetSecurityValue (string key)
    {
      var indexDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["IndexDirectory"]);
     
      var fileName = Path.Combine (indexDirectory, key + ".security");
      
      var value = "";
      
      if (File.Exists (fileName))
        value = File.ReadAllText (fileName).Trim ();
        
      return value;
    }

    public void Save_Click (object sender, EventArgs e)
    {
      /*var name = "";
      var pass = "";
      
      if (ConnectionType.SelectedValue == "Ethernet" ||
        ConnectionType.SelectedValue == "WiFi") {
        name = WiFiName.Text;
        pass = WiFiPass.Text;
      } else {
        name = HotSpotName.Text;
        pass = HotSpotPass.Text;
      }*/
      
      //var connectionType = (NetworkConnectionType)Enum.Parse (typeof(NetworkConnectionType), LocalConnectionType.SelectedValue);
      Manager.SetNetworkDetails (ActivateEthernet.Checked, ActivateWiFiNetwork.Checked, WiFiName.Text, WiFiPass.Text, ActivateWiFiHotSpot.Checked, HotSpotName.Text, HotSpotPass.Text);
      
      Stage = 2;
    }

    public void Reconnect_Click (object sender, EventArgs e)
    {
      Manager.NetworkReconnect ("Local");
      
      var message = "Your network connection has been updated!";
      Response.Redirect ("Settings.aspx?Result=" + HttpUtility.UrlEncode (message));
    }

    public void Back_Click (object sender, EventArgs e)
    {
      Stage = 1;
    }

    public void GenerateComputerFields ()
    {
      var computers = Manager.GetComputersInfo ();
      RemoteComputersExist = false; // Disabled because it's not yet fully implemented
      
      /*foreach (var computer in computers) {
        if (computer.Name != "Local") {
          var startLiteral = new LiteralControl (@"
          <div class=""form-group"">
            <label class=""col-sm-2 col-sm-2 control-label"">" + computer.Name + @"</label>
            <div class=""col-sm-4"">");
            
          var control = new DropDownList ();
          control.CssClass = "form-control";
          control.ID = computer.Name + "ConnectionType";
          control.Items.Add (new ListItem ("Ethernet", "Ethernet"));
          control.Items.Add (new ListItem ("WiFi", "WiFi"));
        
          control.SelectedValue = Manager.GetNetworkConnectionType (computer.Name).ToString ();
        
          var endLiteral = new LiteralControl (@"          
            </div>
          </div>");
        
          ComputerFieldsHolder.Controls.Add (startLiteral);
          ComputerFieldsHolder.Controls.Add (control);
          ComputerFieldsHolder.Controls.Add (endLiteral);
      
          DDLS.Add (computer.Name, control);
        }
      }*/
    }

    public string GetComputerConnectionTypeFieldsArray ()
    {
      var output = "[";
    
      //foreach (var computerConnectionTypeField in DDLS.Values) {
      //  output += "'" + computerConnectionTypeField.ClientID + "',";
      //}
      
      //output = output.Trim (',');
      
      output = output + "]";
      
      return output;
    }
  }
}

