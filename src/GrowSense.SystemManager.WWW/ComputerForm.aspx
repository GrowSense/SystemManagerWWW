<%@ Page Language="C#" Inherits="GrowSense.SystemManager.WWW.ComputerForm" MasterPageFile="~/Master.master" %>
<%@ MasterType VirtualPath="~/Master.master" %>
<asp:Content ContentPlaceHolderID="contentPlaceHolder" ID="contentPlaceHolderContent" runat="server">
  <h3><i class="fa fa-angle-right"></i> Garden</h3>
  <div class="row mt">
    <div class="col-lg-12">
      <% if (!String.IsNullOrEmpty(ErrorMessage)){ %>
      <div class="alert alert-danger"><b>Error:</b> <%= ErrorMessage %></div>
      <% } %>
      <div class="form-panel">
        <h4 class="mb"><i class="fa fa-angle-right"></i> <%= Action %> Computer</h4>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Name:</label>
            <div class="col-sm-10">
              <asp:TextBox runat="server" id="ComputerName" CssClass="form-control" Width="200px"></asp:TextBox>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Path:</label>
            <div class="col-sm-10">
              <asp:TextBox runat="server" id="ComputerPath" CssClass="form-control" Width="200px"></asp:TextBox>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Username:</label>
            <div class="col-sm-10">
              <asp:TextBox runat="server" id="Username" CssClass="form-control" Width="200px"></asp:TextBox>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Password:</label>
            <div class="col-sm-10">
              <asp:TextBox runat="server" id="Password" CssClass="form-control" Width="200px"></asp:TextBox>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Port:</label>
            <div class="col-sm-10">
              <asp:TextBox runat="server" id="Port" CssClass="form-control" Width="100px" Text="22"></asp:TextBox>
            </div>
          </div>
          <div class="form-group">
            <div class="col-lg-offset-2 col-lg-10">
              <asp:Button runat="server" CssClass="btn btn-theme" type="submit" Text="Save" />
              <button class="btn btn-theme04" type="button" onclick="location.href='Computers.aspx'">Cancel</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</asp:Content>


