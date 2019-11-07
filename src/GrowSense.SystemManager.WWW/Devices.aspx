  <%@ Page Language="C#" Inherits="GrowSense.SystemManager.DevicesPage" MasterPageFile="~/Master.master" EnableViewState="false" %>
<%@ MasterType VirtualPath="~/Master.master" %>
<%@ Import Namespace="GrowSense.SystemManager.Web" %>
<asp:Content ContentPlaceHolderID="contentPlaceHolder" ID="contentPlaceHolderContent" runat="server">
  <script language="javascript">
    var selectedDevice = '';
    
    function selectDevice(deviceName)
    {
      selectedDevice = deviceName;
    }
    
    function removeDevice()
    {
      location.href='RemoveDevice.aspx?DeviceName=' + selectedDevice;
    }
  </script>
  <h3><i class="fa fa-angle-right"></i> Garden </h3>
  <div class="row mt">
    <div class="col-md-12">
      <% if (!String.IsNullOrEmpty(Request.QueryString["Result"])){ %>
      <div class="alert alert-<%= (Request.QueryString["IsSuccess"] == "false" ? "danger" : "success") %> "><%= Request.QueryString["Result"] %></div>
      <% } %>
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
                <div class="btn btn-primary btn-xs" onclick="location.href='<%= GetDeviceEditLink(deviceInfo) %>'"><i class="fa fa-pencil"></i></div>
                <div class="btn btn-danger btn-xs" data-toggle="modal" data-target="#removeModal" onclick="selectDevice('<%= deviceInfo.Name %>');"><i class="fa fa-trash-o"></i></div>
              </td>
            </tr>
            <% } %>
          </tbody>
          <% } %>
        </table>
        <div class="modal fade" id="removeModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
          <div class="modal-dialog">
            <div class="modal-content">
              <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title" id="myModalLabel">Confirm Removal</h4>
              </div>
              <div class="modal-body">
                Are you sure you want to remove this device?
              </div>
              <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                <button type="button" class="btn btn-primary" onclick="javascript:removeDevice()">Remove Device</button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</asp:Content>


