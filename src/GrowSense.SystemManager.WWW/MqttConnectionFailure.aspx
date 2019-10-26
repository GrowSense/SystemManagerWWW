<%@ Page Language="C#" Inherits="GrowSense.SystemManager.WWW.MqttConnectionFailure" MasterPageFile="~/Master.master" %>
<%@ MasterType VirtualPath="~/Master.master" %>
<asp:Content ContentPlaceHolderID="contentPlaceHolder" ID="contentPlaceHolderContent" runat="server">
  <h3><i class="fa fa-angle-right"></i> Garden</h3>
  <!-- BASIC FORM ELELEMNTS -->
  <div class="row mt">
    <div class="col-lg-12">
      <div class="form-panel">
        <h4 class="mb"><i class="fa fa-angle-right"></i> MQTT Connection Failed</h4>
        <p>Failed to connect to the MQTT broker...</p>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Host:</label>
            <div>
              <input type="text" value="<%= MqttHost %>" />
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Username:</label>
            <div>
              <input type="text" value="<%= MqttUsername %>" />
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Port:</label>
            <div>
              <input type="text" value="<%= MqttPort %>" />
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</asp:Content>


