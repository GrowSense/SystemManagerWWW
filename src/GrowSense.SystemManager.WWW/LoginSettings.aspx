<%@ Page Language="C#" Inherits="GrowSense.SystemManager.WWW.LoginSettings" enableEventValidation="false" MasterPageFile="~/Master.master" %>
<%@ MasterType VirtualPath="~/Master.master" %>
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
        <h4 class="mb"><i class="fa fa-angle-right"></i> Login</h4>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Username:</label>
            <div class="col-sm-10">
              <asp:TextBox runat="server" id="Username" CssClass="form-control"></asp:TextBox>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Password:</label>
            <div class="col-sm-10">
              <asp:TextBox runat="server" id="Password" CssClass="form-control"></asp:TextBox>
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


