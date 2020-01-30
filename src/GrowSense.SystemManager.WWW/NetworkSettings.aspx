<%@ Page Language="C#" Inherits="GrowSense.SystemManager.WWW.NetworkSettings" enableEventValidation="false" MasterPageFile="~/Master.master" %>
<%@ MasterType VirtualPath="~/Master.master" %>
<asp:Content ContentPlaceHolderID="contentPlaceHolder" ID="contentPlaceHolderContent" runat="server">
  <h3><i class="fa fa-angle-right"></i> Settings</h3>
  <div class="row mt">
    <div class="col-lg-12">
      <div class="form-panel">
        <h4 class="mb"><i class="fa fa-angle-right"></i> WiFi Network</h4>
        <% if (Stage == 1){ %>
        <script language="javascript">
        var computerConnectionTypeFields = <%= GetComputerConnectionTypeFieldsArray() %>;
        
        function connectionChanged()
        {
          showHideFields();
        }
        
        function showHideFields()
        {
          var c = document.getElementById("");
          var connectionType = c.options[c.selectedIndex].value;
          switch(connectionType) {
            case "Ethernet":
            case "WiFi":
              $("#WiFiNameHolder").css("display","block");
              $("#WiFiPassHolder").css("display","block");
              $("#HotSpotNameHolder").css("display","none");
              $("#HotSpotPassHolder").css("display","none");
              break;
            case "WiFiHotSpot":
              $("#WiFiNameHolder").css("display","none");
              $("#WiFiPassHolder").css("display","none");
              $("#HotSpotNameHolder").css("display","block");
              $("#HotSpotPassHolder").css("display","block");
              break;
          }
        }
        
        function wifiNetworkEnabledChanged()
        {
          var wifiNetworkEnabled = document.getElementById("<%= EnableWiFiNetwork.ClientID %>");
          var wifiHotSpotEnabled = document.getElementById("<%= EnableWiFiHotSpot.ClientID %>");
          
          if (wifiNetworkEnabled.checked)
          {
              $("#<%= WiFiName.ClientID %>").prop("disabled", false);
              $("#<%= WiFiPass.ClientID %>").prop("disabled", false);
              $("#<%= HotSpotName.ClientID %>").prop("disabled", true);
              $("#<%= HotSpotPass.ClientID %>").prop("disabled", true);
              
              wifiHotSpotEnabled.checked = false;
          }
          
          updateComputerFields();
        }
        
        function wifiHotSpotEnabledChanged()
        {
          var wifiNetworkEnabled = document.getElementById("<%= EnableWiFiNetwork.ClientID %>");
          var wifiHotSpotEnabled = document.getElementById("<%= EnableWiFiHotSpot.ClientID %>");
          
          if (wifiHotSpotEnabled.checked)
          {
              $("#<%= WiFiName.ClientID %>").prop("disabled", true);
              $("#<%= WiFiPass.ClientID %>").prop("disabled", true);
              $("#<%= HotSpotName.ClientID %>").prop("disabled", false);
              $("#<%= HotSpotPass.ClientID %>").prop("disabled", false);
              
              wifiNetworkEnabled.checked = false;
          }
          
          updateComputerFields();
        }
        
        function updateComputerFields()
        {
        
          var wifiNetworkEnabled = document.getElementById("<%= EnableWiFiNetwork.ClientID %>");
          var wifiHotSpotEnabled = document.getElementById("<%= EnableWiFiHotSpot.ClientID %>");
          
          var localConnectionType = document.getElementById("<%= LocalConnectionType.ClientID %>");
          
          if (wifiHotSpotEnabled.checked)
          {
            $("#<%= LocalConnectionType.ClientID %>").val("WiFiHotSpot");
          }
          
          if (wifiNetworkEnabled.checked)
          {
            if ($("#<%= LocalConnectionType.ClientID %>").val() != "Ethernet")
              $("#<%= LocalConnectionType.ClientID %>").val("WiFi");
          }
          
          if (!wifiHotSpotEnabled.checked && !wifiNetworkEnabled.checked)
          {
            $("#<%= LocalConnectionType.ClientID %>").val("Ethernet");
          }
        
          for (i = 0; i < computerConnectionTypeFields.length; i++) { 
            if (wifiHotSpotEnabled.checked)
            {
              $("#" + computerConnectionTypeFields[i]).val("WiFi");
            }
            
            if (!wifiHotSpotEnabled.checked && !wifiNetworkEnabled.checked)
            {
              $("#" + computerConnectionTypeFields[i]).val("Ethernet");
            }
          }
        }
        </script>
        <div class="form-horizontal style-form">
          <div class="form-group" id="WiFiNameHolder">
            <label class="col-sm-2 col-sm-2 control-label">Enable:</label>
            <div class="col-sm-10">
              <asp:CheckBox runat="server" id="EnableWiFiNetwork" CssClass="form-control" OnClick="wifiNetworkEnabledChanged();"></asp:CheckBox>
            </div>
          </div>
          <div class="form-group" id="WiFiNameHolder">
            <label class="col-sm-2 col-sm-2 control-label">WiFi Name:</label>
            <div class="col-sm-10">
              <asp:TextBox runat="server" id="WiFiName" CssClass="form-control"></asp:TextBox>
            </div>
          </div>
          <div class="form-group" id="WiFiPassHolder">
            <label class="col-sm-2 col-sm-2 control-label">WiFi Pass:</label>
            <div class="col-sm-10">
              <asp:TextBox runat="server" id="WiFiPass" CssClass="form-control"></asp:TextBox>
            </div>
          </div>
        </div>
        <h4 class="mb"><i class="fa fa-angle-right"></i> WiFi HotSpot</h4>
        <div class="form-horizontal style-form">
          <div class="form-group" id="WiFiNameHolder">
            <label class="col-sm-2 col-sm-2 control-label">Enable:</label>
            <div class="col-sm-10">
              <asp:CheckBox runat="server" id="EnableWiFiHotSpot" CssClass="form-control" OnClick="wifiHotSpotEnabledChanged();"></asp:CheckBox>
            </div>
          </div>
          <div class="form-group" id="HotSpotNameHolder">
            <label class="col-sm-2 col-sm-2 control-label">HotSpot Name:</label>
            <div class="col-sm-10">
              <asp:TextBox runat="server" id="HotSpotName" CssClass="form-control"></asp:TextBox>
            </div>
          </div>
          <div class="form-group" id="HotSpotPassHolder">
            <label class="col-sm-2 col-sm-2 control-label">HotSpot Pass:</label>
            <div class="col-sm-10">
              <asp:TextBox runat="server" id="HotSpotPass" CssClass="form-control"></asp:TextBox>
            </div>
          </div>
        </div>
        <h4 class="mb"><i class="fa fa-angle-right"></i> Computers</h4>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Local:</label>
            <div class="col-sm-4">
              <asp:DropDownList runat="server" id="LocalConnectionType" CssClass="form-control" onchange="connectionChanged()">
                <asp:ListItem Enabled="true" Text="Ethernet" Value="Ethernet"></asp:ListItem>
                <asp:ListItem Enabled="true" Text="WiFi" Value="WiFi"></asp:ListItem>
                <asp:ListItem Enabled="true" Text="WiFi HotSpot (local)" Value="WiFiHotSpot"></asp:ListItem>
              </asp:DropDownList>
            </div>
          </div>
        <asp:PlaceHolder runat="server" id="ComputerFieldsHolder">
        </asp:PlaceHolder>
        </div>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <div class="col-lg-offset-2 col-lg-10">
              <asp:Button runat="server" CssClass="btn btn-theme" type="submit" Text="Save" OnClick="Save_Click" />
              <button class="btn btn-theme04" type="button" onclick="location.href='NetworkSettings.aspx'">Cancel</button>
            </div>
          </div>
        </div>
      <% } else if (Stage == 2) { %>
      <div class="form-horizontal style-form">
          <div class="form-group" id="WiFiNameHolder">
            <label class="col-sm-2 col-sm-2 control-label">Enable:</label>
            <div class="col-sm-10">
              <%= EnableWiFiNetwork.Checked %>
            </div>
          </div>
          <div class="form-group" id="WiFiNameHolder">
            <label class="col-sm-2 col-sm-2 control-label">WiFi Name:</label>
            <div class="col-sm-10">
              <%= WiFiName.Text %>
            </div>
          </div>
          <div class="form-group" id="WiFiPassHolder">
            <label class="col-sm-2 col-sm-2 control-label">WiFi Pass:</label>
            <div class="col-sm-10">
              <%= WiFiPass.Text %>
            </div>
          </div>
        </div>
        <h4 class="mb"><i class="fa fa-angle-right"></i> WiFi HotSpot</h4>
        <div class="form-horizontal style-form">
          <div class="form-group" id="WiFiNameHolder">
            <label class="col-sm-2 col-sm-2 control-label">Enable:</label>
            <div class="col-sm-10">
              <%= EnableWiFiHotSpot.Checked %>
            </div>
          </div>
          <div class="form-group" id="HotSpotNameHolder">
            <label class="col-sm-2 col-sm-2 control-label">HotSpot Name:</label>
            <div class="col-sm-10">
              <%= HotSpotName.Text %>
            </div>
          </div>
          <div class="form-group" id="HotSpotPassHolder">
            <label class="col-sm-2 col-sm-2 control-label">HotSpot Pass:</label>
            <div class="col-sm-10">
              <%= HotSpotPass.Text %>
            </div>
          </div>
        </div>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <div class="col-lg-offset-2 col-lg-10">
              <p>The network settings have been updated. Please confirm that they are correct before continuing.</p>
              <p>Click "Reconnect" below to disconnect from the current network connection and connect to the specified connection.</p>
            </div>
          </div>
        </div>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <div class="col-lg-offset-2 col-lg-10">
              <asp:Button runat="server" CssClass="btn btn-info" type="submit" Text="&laquo; Back" OnClick="Back_Click" />
              <asp:Button runat="server" CssClass="btn btn-theme" type="submit" Text="Reconnect" OnClick="Reconnect_Click" />
              <button class="btn btn-theme04" type="button" onclick="location.href='NetworkSettings.aspx'">Cancel</button>
            </div>
          </div>
        </div>
      <% } %>
      </div>
    </div>
  </div>
  <script language="javascript">
  $( document ).ready(function() {
    wifiNetworkEnabledChanged();
    wifiHotSpotEnabledChanged();
  });
  </script>
</asp:Content>


