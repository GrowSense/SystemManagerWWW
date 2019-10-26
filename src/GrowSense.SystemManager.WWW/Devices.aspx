<%@ Page Language="C#" Inherits="GrowSense.SystemManager.DevicesPage" MasterPageFile="~/Master.master" EnableViewState="false" %>
<%@ MasterType VirtualPath="~/Master.master" %>
<%@ Import Namespace="GrowSense.SystemManager.Web" %>
<asp:Content ContentPlaceHolderID="contentPlaceHolder" ID="contentPlaceHolderContent" runat="server">
  <h3><i class="fa fa-angle-right"></i> Garden </h3>
  <div class="row mt">
    <div class="col-md-12">
      <div class="content-panel">
        <table class="table table-striped table-advance table-hover">
          <h4><i class="fa fa-angle-right"></i> Devices</h4>
          <hr>
          <% if (DevicesInfo.Length == 0) { %>
          <thead>
            <tr>
              <th><i class="fa fa-bullhorn"></i> Device</th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td>
                <div>No devices detected.</div>
              </td>
            </tr>
          </tbody>
          <% } else { %>
          <thead>
            <tr>
              <th><i class="fa fa-bullhorn"></i> Device</th>
              <th><i class=" fa fa-edit"></i> Data</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            <% foreach (var deviceInfo in DevicesInfo) { %>
            <tr>
              <td>
                <div><a href="basic_table.html#"><%= deviceInfo.Label %></a></div>
                <%= GenerateDeviceStatusIcon(deviceInfo.Name) %>
              </td>
              <td>
                <%= GenerateDeviceProgressBars(deviceInfo) %>
              </td>
              <td>
                <div class="btn btn-primary btn-xs"><i class="fa fa-pencil" onclick="location.href='<%= GetDeviceEditLink(deviceInfo) %>'"></i></div>
                <button class="btn btn-danger btn-xs"><i class="fa fa-trash-o "></i></button>
              </td>
            </tr>
            <% } %>
            <% } %>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</asp:Content>


