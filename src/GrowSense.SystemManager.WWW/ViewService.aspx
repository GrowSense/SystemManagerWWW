<%@ Page Language="C#" Inherits="GrowSense.SystemManager.WWW.ViewService" MasterPageFile="~/Master.master" %>
<%@ MasterType VirtualPath="~/Master.master" %>
<asp:Content ContentPlaceHolderID="contentPlaceHolder" ID="contentPlaceHolderContent" runat="server">
  <h3><i class="fa fa-angle-right"></i> Garden</h3>
  <div class="row mt">
    <div class="col-lg-12">
      <div class="form-panel">
        <h4 class="mb"><i class="fa fa-angle-right"></i> Service</h4>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Computer:</label>
            <div class="col-md-9">
              <%= ComputerName %>
            </div>
          </div>
        </div>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Service:</label>
            <div class="col-md-9">
              <%= ServiceName %>
            </div>
          </div>
        </div>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Status output:</label>
            <div class="col-md-9">
              <%= StatusOutput.Replace("\n", "<br/>") %>
            </div>
          </div>
        </div>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Log output:</label>
            <div class="col-md-9">
              <%= LogOutput.Replace("\n", "<br/>") %>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</asp:Content>


