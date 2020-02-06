<%@ Page Language="C#" Inherits="GrowSense.SystemManager.WWW.NetworkSettings" enableEventValidation="false" MasterPageFile="~/Master.master" %>
<%@ MasterType VirtualPath="~/Master.master" %>
<%@ Import Namespace="GrowSense.SystemManager.Computers" %>
<asp:Content ContentPlaceHolderID="contentPlaceHolder" ID="contentPlaceHolderContent" runat="server">
  <h3><i class="fa fa-angle-right"></i> Settings</h3>
  <div class="row mt">
    <div class="col-lg-12">
      <div class="form-panel">
        <% if (Stage == 1){ %>
        <script language="javascript">
        var computerConnectionTypeFields = <%= GetComputerConnectionTypeFieldsArray() %>;
        
        function ethernetActivateChanged()
        {
          var activateEthernet = document.getElementById("<%= ActivateEthernet.ClientID %>");
          var activateWiFiNetwork = document.getElementById("<%= ActivateWiFiNetwork.ClientID %>");
          var activateHotSpot = document.getElementById("<%= ActivateWiFiHotSpot.ClientID %>");
          
          if (activateEthernet.checked)
          {
              $("#<%= WiFiName.ClientID %>").prop("disabled", true);
              $("#<%= WiFiPass.ClientID %>").prop("disabled", true);
              $("#<%= HotSpotName.ClientID %>").prop("disabled", true);
              $("#<%= HotSpotPass.ClientID %>").prop("disabled", true);
              
              activateWiFiNetwork.checked = false;
              activateHotSpot.checked = false;
          }
          
          updateComputerFields();
        }
        
        function wifiNetworkActivateChanged()
        {
          var activateEthernet = document.getElementById("<%= ActivateEthernet.ClientID %>");
          var activateWiFiNetwork = document.getElementById("<%= ActivateWiFiNetwork.ClientID %>");
          var activateWiFiHotSpot = document.getElementById("<%= ActivateWiFiHotSpot.ClientID %>");
          
          if (activateWiFiNetwork.checked)
          {
              $("#<%= WiFiName.ClientID %>").prop("disabled", false);
              $("#<%= WiFiPass.ClientID %>").prop("disabled", false);
              $("#<%= HotSpotName.ClientID %>").prop("disabled", true);
              $("#<%= HotSpotPass.ClientID %>").prop("disabled", true);
              
              activateEthernet.checked = false;
              activateWiFiHotSpot.checked = false;
          }
          
          updateComputerFields();
        }
        
        function activateWiFiHotSpotChanged()
        {
          var activateEthernet = document.getElementById("<%= ActivateEthernet.ClientID %>");
          var activateWiFiNetwork = document.getElementById("<%= ActivateWiFiNetwork.ClientID %>");
          var activateWiFiHotSpot = document.getElementById("<%= ActivateWiFiHotSpot.ClientID %>");
          
          if (activateWiFiHotSpot.checked)
          {
              $("#<%= WiFiName.ClientID %>").prop("disabled", true);
              $("#<%= WiFiPass.ClientID %>").prop("disabled", true);
              $("#<%= HotSpotName.ClientID %>").prop("disabled", false);
              $("#<%= HotSpotPass.ClientID %>").prop("disabled", false);
              
              activateEthernet.checked = false;
              activateWiFiNetwork.checked = false;
          }
          
          updateComputerFields();
        }
        
        function updateComputerFields()
        {
        
          var activateWiFiNetwork = document.getElementById("<%= ActivateWiFiNetwork.ClientID %>");
          var activateWiFiHotSpot = document.getElementById("<%= ActivateWiFiHotSpot.ClientID %>");
        
          for (i = 0; i < computerConnectionTypeFields.length; i++) { 
            if (activateWiFiHotSpot.checked)
            {
              $("#" + computerConnectionTypeFields[i]).val("WiFi");
            }
            
            if (!activateWiFiHotSpot.checked && !activateWiFiNetwork.checked)
            {
              $("#" + computerConnectionTypeFields[i]).val("Ethernet");
            }
          }
        }
        </script>
        <h4 class="mb"><i class="fa fa-angle-right"></i> Ethernet</h4>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Activate:</label>
            <div class="col-sm-10">
              <asp:CheckBox runat="server" id="ActivateEthernet" CssClass="form-control" OnClick="ethernetActivateChanged();"></asp:CheckBox>
            </div>
          </div>
        </div>
        <h4 class="mb"><i class="fa fa-angle-right"></i> WiFi Network</h4>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Activate:</label>
            <div class="col-sm-10">
              <asp:CheckBox runat="server" id="ActivateWiFiNetwork" CssClass="form-control" OnClick="wifiNetworkActivateChanged();"></asp:CheckBox>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">WiFi Name:</label>
            <div class="col-sm-10">
              <asp:TextBox runat="server" id="WiFiName" CssClass="form-control"></asp:TextBox>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">WiFi Pass:</label>
            <div class="col-sm-10">
              <asp:TextBox runat="server" id="WiFiPass" CssClass="form-control"></asp:TextBox>
            </div>
          </div>
        </div>
        <h4 class="mb"><i class="fa fa-angle-right"></i> WiFi HotSpot</h4>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Activate:</label>
            <div class="col-sm-10">
              <asp:CheckBox runat="server" id="ActivateWiFiHotSpot" CssClass="form-control" OnClick="activateWiFiHotSpotChanged();"></asp:CheckBox>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">HotSpot Name:</label>
            <div class="col-sm-10">
              <asp:TextBox runat="server" id="HotSpotName" CssClass="form-control"></asp:TextBox>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">HotSpot Pass:</label>
            <div class="col-sm-10">
              <asp:TextBox runat="server" id="HotSpotPass" CssClass="form-control"></asp:TextBox>
            </div>
          </div>
        </div>
        <% if (RemoteComputersExist) { %>
        <h4 class="mb"><i class="fa fa-angle-right"></i> Computers</h4>
        <div class="form-horizontal style-form">
        <asp:PlaceHolder runat="server" id="ComputerFieldsHolder">
        </asp:PlaceHolder>
        </div>
        <% } %>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <div class="col-lg-offset-2 col-lg-10">
              <asp:Button runat="server" CssClass="btn btn-theme" type="submit" Text="Save" OnClick="Save_Click" />
              <button class="btn btn-theme04" type="button" onclick="location.href='NetworkSettings.aspx'">Cancel</button>
            </div>
          </div>
        </div>
        <script language="javascript">
        $( document ).ready(function() {
          ethernetActivateChanged();
          wifiNetworkActivateChanged();
          activateWiFiHotSpotChanged();
        });
        </script>
      <% } else if (Stage == 2) { %>
        <% if (ConnectionType == NetworkConnectionType.Ethernet) { %>
        <h4 class="mb"><i class="fa fa-angle-right"></i> Ethernet</h4>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Activate:</label>
            <div class="col-sm-10">
              <%= ActivateEthernet.Checked %>
            </div>
          </div>
        </div>
        <% } else if (ConnectionType == NetworkConnectionType.WiFi) { %>
        <h4 class="mb"><i class="fa fa-angle-right"></i> WiFi</h4>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Activate:</label>
            <div class="col-sm-10">
              <%= ActivateWiFiNetwork.Checked %>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">WiFi Name:</label>
            <div class="col-sm-10">
              <%= WiFiName.Text %>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">WiFi Pass:</label>
            <div class="col-sm-10">
              <%= WiFiPass.Text %>
            </div>
          </div>
        </div>
        <% } else if (ConnectionType == NetworkConnectionType.WiFiHotSpot) { %>
        <h4 class="mb"><i class="fa fa-angle-right"></i> WiFi HotSpot</h4>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Activate:</label>
            <div class="col-sm-10">
              <%= ActivateWiFiHotSpot.Checked %>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">HotSpot Name:</label>
            <div class="col-sm-10">
              <%= HotSpotName.Text %>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">HotSpot Pass:</label>
            <div class="col-sm-10">
              <%= HotSpotPass.Text %>
            </div>
          </div>
        </div>
        <% } %>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <div class="col-lg-offset-2 col-lg-10">
              <p>The network settings have been updated. Please confirm that they are correct before continuing.</p>
              <p>Click "Apply" below to set up the specified connection.</p>
            </div>
          </div>
        </div>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <div class="col-lg-offset-2 col-lg-10">
              <asp:Button runat="server" CssClass="btn btn-info" type="submit" Text="&laquo; Back" OnClick="Back_Click" />
              <asp:Button runat="server" CssClass="btn btn-theme" type="submit" Text="Apply" OnClick="Apply_Click" />
              <button class="btn btn-theme04" type="button" onclick="location.href='NetworkSettings.aspx'">Cancel</button>
            </div>
          </div>
        </div>
      <% } else if (Stage == 3) { %>
        <h4 class="mb"><i class="fa fa-angle-right"></i> Network</h4>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Connection:</label>
            <div class="col-sm-10">
              <%= ConnectionType.ToString() %>
            </div>
          </div>
        </div>
        <% if (ConnectionType == NetworkConnectionType.WiFi) { %>
        <h4 class="mb"><i class="fa fa-angle-right"></i> WiFi</h4>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">WiFi Name:</label>
            <div class="col-sm-10">
              <%= WiFiName.Text %>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">WiFi Pass:</label>
            <div class="col-sm-10">
              <%= WiFiPass.Text %>
            </div>
          </div>
        </div>
        <% } else if (ConnectionType == NetworkConnectionType.WiFiHotSpot) { %>
        <h4 class="mb"><i class="fa fa-angle-right"></i> WiFi HotSpot</h4>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">HotSpot Name:</label>
            <div class="col-sm-10">
              <%= HotSpotName.Text %>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">HotSpot Pass:</label>
            <div class="col-sm-10">
              <%= HotSpotPass.Text %>
            </div>
          </div>
        </div>
        <% } %>
        <h4 class="mb"><i class="fa fa-angle-right"></i> Connection</h4>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Status:</label>
            <div class="col-sm-10">
              <div class="label label-info label-mini" id="Connecting">Connecting</div>
              <div class="label label-success label-mini" id="Connected" style="display:none;">Connected</div>
              <div class="label label-danger label-mini" id="Failed" style="display:none;">Failed</div>
            </div>
          </div>
        </div>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Result:</label>
            <div class="col-sm-10" id="Result">
              Pending... (please wait)
            </div>
          </div>
        </div>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Service output:</label>
            <div class="col-sm-10">
              <div class="btn btn-info btn-xs" onclick="showServiceOutput();" id="ShowServiceOutput"><i class="fa fa-plus-square"></i></div>
            </div>
            <div class="col-sm-10" id="ServiceOutput" style="display:none;">
              Pending...
            </div>
          </div>
        </div>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Internet connection:</label>
            <div class="col-sm-10">
              <div class="label label-info label-mini" id="InternetPending">Pending</div>
              <div class="label label-success label-mini" id="InternetOnline" style="display:none;">Online</div>
              <div class="label label-danger label-mini" id="InternetOffline" style="display:none;">Offline</div>
            </div>
          </div>
        </div>
        <div class="form-horizontal style-form" id="ManualReconnectDeviceHolder" style="display:none;">
          <div class="form-group">
            <div class="col-lg-offset-2 col-lg-10">
              Please manually connect your current device to the specified network, then wait for connection to complete, before clicking continue.
            </div>
          </div>
        </div>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <div class="col-lg-offset-2 col-lg-10">
              <button class="btn btn-theme" type="button" onclick="location.href='Default.aspx'" id="ContinueButton" disabled="true">Continue</button>
            </div>
          </div>
        </div>
        <script language="javascript">
        var isFinished = false;

        function loadResultLoop()
        {
          const interval = setTimeout(function() {

            var manualReconnectDeviceHolder = document.getElementById("ManualReconnectDeviceHolder");
            var continueButton = document.getElementById("ContinueButton");
            var connecting = document.getElementById("Connecting");
            var connected = document.getElementById("Connected");
            var failed = document.getElementById("Failed");
            
            $('#Result').load('NetworkSetupStatus.aspx #Result', "", function(responseText, textStatus, XMLHttpRequest) {
             switch (XMLHttpRequest.status) {
              case 200: break;
              case 404:
               manualReconnectDeviceHolder.style.display = "block";
               break;
              //default:
              // $('#results').html('<p>' + XMLHttpRequest.status + ': ' + XMLHttpRequest.statusText + '. Please contact the club and let them know.</p>');
              // break;
              default:
              //alert(responseText);
               break;
             }
            });
            
            var result = document.getElementById("Result").innerText;
            //alert(result);
            if (result.includes("connected") || result.includes("failed") || result.includes("not yet supported"))
            {
              continueButton.disabled = false;
              
              isFinished = true;
            }
            
            if (result.includes("connected"))
            {
              connecting.style.display = "none";
              connected.style.display = "inline";
            }
            
            if (result.includes("failed"))
            {
              connecting.style.display = "none";
              failed.style.display = "inline";
            }
            
            if (result.includes("not yet supported"))
            {
              connecting.style.display = "none";
              failed.style.display = "inline";
            }
            
            $.get('NetworkSetupStatus.aspx', function(result){
              var internetStatusText = $(result).find('#InternetStatus').text();
              
              if (internetStatusText.includes("Online")){
                document.getElementById("InternetPending").style.display = "none";
                document.getElementById("InternetOnline").style.display = "inline";
                document.getElementById("InternetOffline").style.display = "none";
              }else{
                document.getElementById("InternetPending").style.display = "none";
                document.getElementById("InternetOnline").style.display = "none";
                document.getElementById("InternetOffline").style.display = "inline";
              }
              
              var serviceOutputText = $(result).find('#ServiceOutput').innerHtml();
              $('#ServiceOutput').innerHtml(serviceOutputText);
            });
            
            if (!isFinished)
              loadResultLoop();
            
          }, 2000);
        }
        
        function showServiceOutput()
        {
          var showServiceOutput = document.getElementById("ShowServiceOutput");
          var serviceOutput = document.getElementById("ServiceOutput");
            
          showServiceOutput.style.display = "none";
          serviceOutput.style.display = "inline";
        }

        $( document ).ready(function() {
          loadResultLoop();
        });
        </script>
      <% } %>
      </div>
    </div>
  </div>
</asp:Content>


