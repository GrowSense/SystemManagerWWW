<%@ Page Language="C#" Inherits="GrowSense.SystemManager.WWW.Settings" MasterPageFile="~/Master.master" %>
<%@ MasterType VirtualPath="~/Master.master" %>
<asp:Content ContentPlaceHolderID="contentPlaceHolder" ID="contentPlaceHolderContent" runat="server">
  <h3><i class="fa fa-angle-right"></i> Settings </h3>
  <div class="row mt">
    <div class="col-md-12">
      <% if (!String.IsNullOrEmpty(Request.QueryString["Result"])){ %>
      <div class="alert alert-<%= (Request.QueryString["IsSuccess"] == "false" ? "danger" : "success") %> "><%= Request.QueryString["Result"] %></div>
      <% } %>
      <div class="content-panel">
        <table class="table table-striped table-advance table-hover">
          <h4><i class="fa fa-angle-right"></i> System</h4>
          <hr>
          <thead>
            <tr>
              <th><i class="fa fa-gear"></i> Service</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td> Network </td>
              <td>
                <div class="btn btn-primary btn-xs" onclick="location.href='NetworkSettings.aspx'"><i class="fa fa-pencil"></i></div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</asp:Content>


