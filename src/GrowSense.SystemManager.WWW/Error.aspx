<%@ Page Language="C#" Inherits="GrowSense.SystemManager.WWW.Error" MasterPageFile="~/Master.master" %>
<%@ MasterType VirtualPath="~/Master.master" %>
<asp:Content ContentPlaceHolderID="contentPlaceHolder" ID="contentPlaceHolderContent" runat="server">
  <h3><i class="fa fa-angle-right"></i> Garden</h3>
  <div class="row mt">
    <div class="col-lg-12">
      <div class="form-panel">
        <h4 class="mb"><i class="fa fa-angle-right"></i> Error</h4>
        <div class="alert alert-danger"><%= ErrorMessage.Replace(Environment.NewLine, "<br/>") %></div>
        <% if (!String.IsNullOrEmpty(ErrorDetails)) { %>
        <h4 class="mb">Details</h4>
        <div><%= FormatDetails(ErrorDetails) %></div>
        <% } %>
      </div>
    </div>
  </div>
</asp:Content>


