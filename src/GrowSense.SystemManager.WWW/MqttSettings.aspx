<%@ Page Language="C#" Inherits="GrowSense.SystemManager.WWW.MqttSettings" enableEventValidation="false" MasterPageFile="~/Master.master" %>
<%@ MasterType VirtualPath="~/Master.master" %>
<%@ Import Namespace="GrowSense.SystemManager.Computers" %>
<asp:Content ContentPlaceHolderID="contentPlaceHolder" ID="contentPlaceHolderContent" runat="server">
  <h3><i class="fa fa-angle-right"></i> Settings</h3>
  <div class="row mt">
    <div class="col-lg-12">
      <% if (!String.IsNullOrEmpty(Result)){ %>
      <div class="alert alert-<%= (!IsSuccess ? "danger" : "success") %> "><%= Result %></div>
      <% } %>
      <div class="form-panel">
        <script language="javascript">
        </script>
        <h4 class="mb"><i class="fa fa-angle-right"></i> MQTT</h4>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Is Running Locally:</label>
            <div class="col-sm-10">
              <asp:CheckBox runat="server" id="IsRunningLocally" CssClass="form-control"></asp:CheckBox>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">MQTT Host:</label>
            <div class="col-sm-10">
              <asp:TextBox runat="server" id="MqttHost" CssClass="form-control"></asp:TextBox>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">MQTT Username:</label>
            <div class="col-sm-10">
              <asp:TextBox runat="server" id="MqttUsername" CssClass="form-control"></asp:TextBox>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">MQTT Password:</label>
            <div class="col-sm-10">
              <asp:TextBox runat="server" id="MqttPassword" CssClass="form-control"></asp:TextBox>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">MQTT Port:</label>
            <div class="col-sm-10">
              <asp:TextBox runat="server" id="MqttPort" CssClass="form-control"></asp:TextBox>
            </div>
          </div>
        </div>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <div class="col-lg-offset-2 col-lg-10">
              <asp:Button runat="server" CssClass="btn btn-theme" type="submit" Text="Save" OnClick="Save_Click" />
              <button class="btn btn-theme04" type="button" onclick="location.href='Settings.aspx'">Cancel</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</asp:Content>


