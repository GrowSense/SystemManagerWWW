<%@ Page Language="C#" Inherits="GrowSense.SystemManager.WWW.MqttConnectionFailure" MasterPageFile="~/Master.master" %>
<%@ MasterType VirtualPath="~/Master.master" %>
<asp:Content ContentPlaceHolderID="contentPlaceHolder" ID="contentPlaceHolderContent" runat="server">
  <h3><i class="fa fa-angle-right"></i> Garden</h3>
  <!-- BASIC FORM ELELEMNTS -->
  <div class="row mt">
    <div class="col-lg-12">
      <div class="form-panel">
        <h4 class="mb"><i class="fa fa-angle-right"></i> MQTT Connection Failed</h4>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <label>Failed to connect to the MQTT broker...</label>
          </div>
        </div><div class="form-horizontal style-form">
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Host:</label>
            <div class="col-sm-10">
              <%= MqttHost %>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</asp:Content>


