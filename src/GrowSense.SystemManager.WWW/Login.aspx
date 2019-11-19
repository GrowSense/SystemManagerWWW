<%@ Page Language="C#" Inherits="GrowSense.SystemManager.WWW.Login" MasterPageFile="~/Master.master" %>
<%@ MasterType VirtualPath="~/Master.master" %>
<asp:Content ContentPlaceHolderID="contentPlaceHolder" ID="contentPlaceHolderContent" runat="server">
  <h2 class="form-login-heading">Log In Now</h2>
  <% if (IsInvalid){ %>
      <div class="alert alert-danger"><b>Error:</b> Invalid username or password!</div>
  <% } %>
  <div class="login-wrap">
    <asp:TextBox runat="server" CssClass="form-control" id="Username"></asp:TextBox>
    <br />
    <asp:TextBox runat="server" TextMode="password" CssClass="form-control" id="Password"></asp:TextBox>
    <label class="checkbox">
      <asp:CheckBox runat="server" id="RememberMe" /> Remember me
    </label>
    <asp:Button CssClass="btn btn-theme btn-block" runat="server" text="LOG IN" id="LoginButton" OnClick="LogInButton_Click"></asp:Button>
  </div>
</asp:Content>


