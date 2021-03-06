<%@ Page Language="C#" Inherits="GrowSense.SystemManager.WWW.EditIlluminator" MasterPageFile="~/Master.master" %>
<%@ MasterType VirtualPath="~/Master.master" %>
<asp:Content ContentPlaceHolderID="contentPlaceHolder" ID="contentPlaceHolderContent" runat="server">
  <script language="javascript">
  function modeChanged()
  {
    showHideFields();
  }
  
  function showHideFields()
  {
    var m = document.getElementById("<%= LightMode.ClientID %>");
    var lightMode = m.options[m.selectedIndex].value;
    switch(lightMode) {
      case "0": // Off
      case "1": // On
        $("#TimerHolder").css("display","none");
        $("#ClockHolder").css("display","none");
        $("#ThresholdHolder").css("display","none");
        break;
      case "2": // Timer
        $("#TimerHolder").css("display","block");
        $("#ClockHolder").css("display","block");
        $("#ThresholdHolder").css("display","none");
        break;
      case "3": // Above Threshold
      case "4": // Below Threshold
        $("#TimerHolder").css("display","none");
        $("#ClockHolder").css("display","none");
        $("#ThresholdHolder").css("display","block");
        break;
    }
  }
  
  function enableSetClockChanged()
  {
    var isEnabled = $("#<%= EnableSetClock.ClientID %>").is(":checked");

    $("#<%= ClockHour.ClientID %>").prop("disabled", !isEnabled);
    $("#<%= ClockMinute.ClientID %>").prop("disabled", !isEnabled);
  }
  </script>
  <h3><i class="fa fa-angle-right"></i> Garden</h3>
  <div class="row mt">
    <div class="col-lg-12">
      <div class="form-panel">
        <h4 class="mb"><i class="fa fa-angle-right"></i> Edit Illuminator</h4>
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
              <asp:TextBox runat="server" id="ReadingIntervalQuantity" CssClass="form-control" Style="width: 60px;"></asp:TextBox>
              <asp:DropDownList runat="server" id="ReadingIntervalType" CssClass="form-control" Style="width: 100px;">
                 <asp:ListItem Enabled="true" Text="Seconds" Value="Seconds"></asp:ListItem>
                 <asp:ListItem Enabled="true" Text="Minutes" Value="Minutes"></asp:ListItem>
                 <asp:ListItem Enabled="true" Text="Hours" Value="Hours"></asp:ListItem>
              </asp:DropDownList>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Light Mode:</label>
            <div class="col-sm-10">
              <asp:DropDownList runat="server" id="LightMode" CssClass="form-control" Style="width: 200px;" onchange="modeChanged()">
                 <asp:ListItem Enabled="true" Text="Off" Value="0"></asp:ListItem>
                 <asp:ListItem Enabled="true" Text="On" Value="1"></asp:ListItem>
                 <asp:ListItem Enabled="true" Text="Timer" Value="2"></asp:ListItem>
                 <asp:ListItem Enabled="true" Text="Above Threshold" Value="3"></asp:ListItem>
                 <asp:ListItem Enabled="true" Text="Below Threshold" Value="4"></asp:ListItem>
              </asp:DropDownList>
            </div>
          </div>
          <div class="form-group form-inline" id="TimerHolder">
            <label class="col-sm-2 col-sm-2 control-label">Timer:</label>
            <div class="col-sm-10 form-inline">
              <div>
              On&nbsp;
                <asp:DropDownList runat="server" id="TimerStartHour" CssClass="form-control" Style="width: 80px;">
                </asp:DropDownList> :
                <asp:DropDownList runat="server" id="TimerStartMinute" CssClass="form-control" Style="width: 80px;">
                </asp:DropDownList>
              </div>
              <div>
                Off&nbsp;
                <asp:DropDownList runat="server" id="TimerStopHour" CssClass="form-control" Style="width: 80px;">
                </asp:DropDownList> :
                <asp:DropDownList runat="server" id="TimerStopMinute" CssClass="form-control" Style="width: 80px;">
                </asp:DropDownList>
              </div>
            </div>
          </div>
          <div class="form-group form-inline" id="ClockHolder">
            <label class="col-sm-2 col-sm-2 control-label">Clock:</label>
            <div class="col-sm-10 form-inline">
              <asp:CheckBox runat="server" id="EnableSetClock" onclick="enableSetClockChanged()"></asp:CheckBox>
              <asp:DropDownList runat="server" id="ClockHour" CssClass="form-control" Style="width: 90px;">
              </asp:DropDownList> :
              <asp:DropDownList runat="server" id="ClockMinute" CssClass="form-control" Style="width: 80px;">
              </asp:DropDownList>
            </div>
          </div>
          <div class="form-group" id="ThresholdHolder">
            <label class="col-sm-2 col-sm-2 control-label">Threshold:</label>
            <div class="col-sm-10">
              <asp:DropDownList runat="server" id="Threshold" CssClass="form-control" Style="width: 100px;">
              </asp:DropDownList>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Calibration:</label>
            <div class="col-sm-10 form-inline">
              <div>
                Dark&nbsp;&nbsp;&nbsp;
                <asp:DropDownList runat="server" id="DarkCalibration" CssClass="form-control" Style="width: 80px;">
                </asp:DropDownList>
              </div>
              <div>
                Bright&nbsp;
                <asp:DropDownList runat="server" id="BrightCalibration" CssClass="form-control" Style="width: 80px;">
                </asp:DropDownList>
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
  <script language="javascript">
  $( document ).ready(function() {
    showHideFields();
    $("#<%= ClockHour.ClientID %>").prop("disabled", "true");
    $("#<%= ClockMinute.ClientID %>").prop("disabled", "true");
  });
  </script>
</asp:Content>


