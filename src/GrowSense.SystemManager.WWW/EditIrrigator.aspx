<%@ Page Language="C#" Inherits="GrowSense.SystemManager.WWW.EditIrrigator" MasterPageFile="~/Master.master" %>
<%@ MasterType VirtualPath="~/Master.master" %>
<asp:Content ContentPlaceHolderID="contentPlaceHolder" ID="contentPlaceHolderContent" runat="server">
  <script language="javascript">
  var defaultArduinoDrySoilMoistureCalibration = <%= GetConfigSetting("DefaultArduinoDrySoilMoistureCalibration") %>;
  var defaultArduinoWetSoilMoistureCalibration = <%= GetConfigSetting("DefaultArduinoWetSoilMoistureCalibration") %>;
  var defaultEspDrySoilMoistureCalibration = <%= GetConfigSetting("DefaultEspDrySoilMoistureCalibration") %>;
  var defaultEspWetSoilMoistureCalibration = <%= GetConfigSetting("DefaultEspWetSoilMoistureCalibration") %>;
  
  function expandCalibration()
  {
    var calibrationAdvanced = document.getElementById("calibrationAdvanced");
    calibrationAdvanced.style.display = "block";
    var rawMoistureValue = document.getElementById("rawMoistureValue");
    rawMoistureValue.style.display = "block";
  }
  
  function setDefaultCalibration()
  {
    var s = document.getElementById("<%= BoardType.ClientID %>");
    var boardType = s.options[s.selectedIndex].value;
    if (boardType == "arduino")
    {
      $('#<%= DryCalibration.ClientID %>').val(defaultArduinoDrySoilMoistureCalibration);
      $('#<%= WetCalibration.ClientID %>').val(defaultArduinoWetSoilMoistureCalibration);
    }
    else
    {
      $('#<%= DryCalibration.ClientID %>').val(defaultEspDrySoilMoistureCalibration);
      $('#<%= WetCalibration.ClientID %>').val(defaultEspWetSoilMoistureCalibration);
    }
  }
  </script>
  <h3><i class="fa fa-angle-right"></i> Garden</h3>
  <div class="row mt">
    <div class="col-lg-12">
      <div class="form-panel">
        <h4 class="mb"><i class="fa fa-angle-right"></i> Edit Irrigator</h4>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Label:</label>
            <div class="col-sm-10">
              <asp:TextBox runat="server" id="Label" CssClass="form-control"></asp:TextBox>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Name:</label>
            <div class="col-sm-10">
              <asp:TextBox runat="server" id="Name" CssClass="form-control"></asp:TextBox>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Reading Interval:</label>
            <div class="col-sm-10 form-inline">
              <asp:TextBox runat="server" id="ReadingIntervalQuantity" CssClass="form-control" Style="width: 100px;"></asp:TextBox>
              <asp:DropDownList runat="server" id="ReadingIntervalType" CssClass="form-control" Style="width: 100px;">
                 <asp:ListItem Enabled="true" Text="Seconds" Value="Seconds"></asp:ListItem>
                 <asp:ListItem Enabled="true" Text="Minutes" Value="Minutes"></asp:ListItem>
                 <asp:ListItem Enabled="true" Text="Hours" Value="Hours"></asp:ListItem>
              </asp:DropDownList>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Pump Mode:</label>
            <div class="col-sm-10">
              <asp:DropDownList runat="server" id="PumpMode" CssClass="form-control" Style="width: 100px;">
                 <asp:ListItem Enabled="true" Text="Off" Value="0"></asp:ListItem>
                 <asp:ListItem Enabled="true" Text="On" Value="1"></asp:ListItem>
                 <asp:ListItem Enabled="true" Text="Auto" Value="2"></asp:ListItem>
              </asp:DropDownList>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Threshold:</label>
            <div class="col-sm-10">
              <asp:DropDownList runat="server" id="Threshold" CssClass="form-control" Style="width: 100px;">
              </asp:DropDownList>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Pump Burst:</label>
            <div class="col-sm-10 form-inline">
              On for&nbsp;
              <asp:TextBox runat="server" id="BurstOnQuantity" CssClass="form-control" Style="width: 100px;"></asp:TextBox>
              <asp:DropDownList runat="server" id="BurstOnType" CssClass="form-control" Style="width: 100px;">
                 <asp:ListItem Enabled="true" Text="Seconds" Value="Seconds"></asp:ListItem>
                 <asp:ListItem Enabled="true" Text="Minutes" Value="Minutes"></asp:ListItem>
                 <asp:ListItem Enabled="true" Text="Hours" Value="Hours"></asp:ListItem>
              </asp:DropDownList>
              &nbsp;&nbsp;&nbsp;&nbsp;Off for&nbsp;
              <asp:TextBox runat="server" id="BurstOffQuantity" CssClass="form-control" Style="width: 100px;"></asp:TextBox>
              <asp:DropDownList runat="server" id="BurstOffType" CssClass="form-control" Style="width: 100px;">
                 <asp:ListItem Enabled="true" Text="Seconds" Value="Seconds"></asp:ListItem>
                 <asp:ListItem Enabled="true" Text="Minutes" Value="Minutes"></asp:ListItem>
                 <asp:ListItem Enabled="true" Text="Hours" Value="Hours"></asp:ListItem>
              </asp:DropDownList>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Calibration:</label>
            <div class="col-sm-10 form-inline">
              Dry&nbsp;
              <asp:DropDownList runat="server" id="DryCalibration" CssClass="form-control" Style="width: 100px;">
              </asp:DropDownList>
              &nbsp;&nbsp;&nbsp;&nbsp;
              Wet&nbsp;
              <asp:DropDownList runat="server" id="WetCalibration" CssClass="form-control" Style="width: 100px;">
              </asp:DropDownList>
              &nbsp;
              <div class="btn btn-info btn-xs" onclick="expandCalibration();"><i class="fa fa-plus-square"></i></div>
            </div>
          </div>
          <div class="form-group" id="calibrationAdvanced" style="display:none;">
            <label class="col-sm-2 col-sm-2 control-label">Calibration (advanced):</label>
            <div class="col-sm-10 form-inline">
              <asp:DropDownList runat="server" id="BoardType" CssClass="form-control" Style="width: 100px;">
               <asp:ListItem Enabled="true" Text="Arduino" Value="arduino"></asp:ListItem>
               <asp:ListItem Enabled="true" Text="ESP/WiFi" Value="esp"></asp:ListItem>
              </asp:DropDownList>
              <button class="btn btn-theme03" type="button" onclick="setDefaultCalibration();">Set Defaults</button>
            </div>
          </div>
          <div class="form-group" id="rawMoistureValue" style="display:none;">
            <label class="col-sm-2 col-sm-2 control-label">Raw soil moisture:</label>
            <div class="col-sm-10 form-inline">
              <%= Utility.GetDeviceData(Device.Name, "R") %>
              <div class="progress form-inline" style="width: 200px;">
                <%= GenerateRawProgressBar() %>
              </div>
            </div>
          </div>
          <div class="form-group">
            <div class="col-lg-offset-2 col-lg-10">
              <asp:Button runat="server" CssClass="btn btn-theme" type="submit" Text="Save" />
              <button class="btn btn-theme04" type="button" onclick="location.href='Devices.aspx'">Cancel</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</asp:Content>


