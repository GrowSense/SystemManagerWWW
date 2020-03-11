using System.IO;
using System.Configuration;
using GrowSense.SystemManager.Computers;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.Configuration;

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

    public NetworkConnectionType ConnectionType {
      get {
        if (ViewState ["ConnectionType"] == null)
          ViewState ["ConnectionType"] = NetworkConnectionType.None;
        return (NetworkConnectionType)ViewState ["ConnectionType"];
      }
      set { ViewState ["ConnectionType"] = value; }
    }

    public string CountryCode {
      get {
        if (ViewState ["CountryCode"] == null)
          ViewState ["CountryCode"] = "US";
        return (string)ViewState ["CountryCode"];
      }
      set { ViewState ["CountryCode"] = value; }
    }

    public void Page_Load (object sender, EventArgs e)
    {
      var indexDirectory = Path.GetFullPath (WebConfigurationManager.AppSettings ["IndexDirectory"]);
     
      var computersDirectory = Path.GetFullPath (WebConfigurationManager.AppSettings ["ComputersDirectory"]);
     
      Manager = new ComputerManager (indexDirectory, computersDirectory);
    
      GenerateComputerFields ();
        
      if (!IsPostBack) {
        PopulateForm ();
      } 
    }

    public void PopulateForm ()
    {
      var countryCode = Manager.GetCountryCode ("Local");
      
      CountryCodeList.SelectedValue = countryCode;
    
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
        !String.IsNullOrEmpty (wifiPass) && wifiPass != "na" &&
        localConnectionType == NetworkConnectionType.WiFi)
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
      var indexDirectory = Path.GetFullPath (WebConfigurationManager.AppSettings ["IndexDirectory"]);
     
      var fileName = Path.Combine (indexDirectory, key + ".security");
      
      var value = "";
      
      if (File.Exists (fileName))
        value = File.ReadAllText (fileName).Trim ();
        
      return value;
    }

    public void Save_Click (object sender, EventArgs e)
    {
      Manager.SetNetworkDetails ("Local", CountryCodeList.SelectedValue, ActivateEthernet.Checked, ActivateWiFiNetwork.Checked, WiFiName.Text, WiFiPass.Text, ActivateWiFiHotSpot.Checked, HotSpotName.Text, HotSpotPass.Text);
            
      ConnectionType = Manager.GetNetworkConnectionType ("Local");
      CountryCode = CountryCodeList.SelectedValue;
      
      Stage = 2;
    }

    public void Apply_Click (object sender, EventArgs e)
    {
      Manager.NetworkSetup ("Local");
      
      //var message = "Your network connection has been updated!";
      //Response.Redirect ("Settings.aspx?Result=" + HttpUtility.UrlEncode (message));
      
      Stage = 3;
    }

    public void Back_Click (object sender, EventArgs e)
    {
      Stage = 1;
    }

    public void GenerateComputerFields ()
    {
      //var computers = Manager.GetComputersInfo ();
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

