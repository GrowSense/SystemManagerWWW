<%@ Page Language="C#" Inherits="GrowSense.SystemManager.WWW.ComputerTools" MasterPageFile="~/Master.master" %>
<%@ MasterType VirtualPath="~/Master.master" %>
<asp:Content ContentPlaceHolderID="contentPlaceHolder" ID="contentPlaceHolderContent" runat="server">
  <h3><i class="fa fa-angle-right"></i> Computer Tools: <%= Computer.Name %> </h3>
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
              <th><i class="fa fa-check"></i> Status</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td> Plug and Play </td>
              <td>
                <div>
                  <%= GeneratePlugAndPlayServiceStatusIcon() %>
                </div>
              </td>
              <td>
                <div class="btn btn-default btn-xs" onclick="location.href='<%= GetViewServicePath("arduino-plug-and-play.service") %>'"><i class="fa fa-search"></i></div>
                <div class="btn btn-success btn-xs" onclick="location.href='<%= GetServiceActionPath("start", "arduino-plug-and-play", "Plug and play") %>'"><i class="fa fa-play"></i></div>
                <div class="btn btn-primary btn-xs" onclick="location.href='<%= GetServiceActionPath("restart", "arduino-plug-and-play", "Plug and play") %>'"><i class="fa fa-refresh"></i></div>
                <div class="btn btn-danger btn-xs" onclick="location.href='<%= GetServiceActionPath("stop", "arduino-plug-and-play", "Plug and play") %>'"><i class="fa fa-stop"></i></div>
                <div class="btn btn-warning btn-xs" onclick="location.href='<%= GetServiceActionPath("disable", "arduino-plug-and-play", "Plug and play") %>'"><i class="fa fa-times-circle"></i></div>
              </td>
            </tr>
            <tr>
              <td> Supervisor </td>
              <td>
                <div>
                  <%= GenerateSupervisorServiceStatusIcon() %>
                </div>
              </td>
              <td>
                <div class="btn btn-default btn-xs" onclick="location.href='<%= GetViewServicePath("growsense-supervisor.service") %>'"><i class="fa fa-search"></i></div>
                <div class="btn btn-success btn-xs" onclick="location.href='<%= GetServiceActionPath("start", "growsense-supervisor.service", "Supervisor") %>'"><i class="fa fa-play"></i></div>
                <div class="btn btn-primary btn-xs" onclick="location.href='<%= GetServiceActionPath("restart", "growsense-supervisor.service", "Supervisor") %>'"><i class="fa fa-refresh"></i></div>
                <div class="btn btn-danger btn-xs" onclick="location.href='<%= GetServiceActionPath("stop", "growsense-supervisor.service", "Supervisor") %>'"><i class="fa fa-stop"></i></div>
                <div class="btn btn-warning btn-xs" onclick="location.href='<%= GetServiceActionPath("disable", "growsense-supervisor.service", "Supervisor") %>'"><i class="fa fa-times-circle"></i></div>
              </td>
            </tr>
            <tr>
              <td> Mesh Manager </td>
              <td>
                <div>
                  <%= GenerateMeshManagerServiceStatusIcon() %>
                </div>
              </td>
              <td>
                <div class="btn btn-default btn-xs" onclick="location.href='<%= GetViewServicePath("growsense-mesh-manager.service") %>'"><i class="fa fa-search"></i></div>
                <div class="btn btn-success btn-xs" onclick="location.href='<%= GetServiceActionPath("start", "growsense-mesh-manaager.service", "Mesh manager") %>'"><i class="fa fa-play"></i></div>
                <div class="btn btn-primary btn-xs" onclick="location.href='<%= GetServiceActionPath("restart", "growsense-mesh-manager.service", "Mesh manager") %>'"><i class="fa fa-refresh"></i></div>
                <div class="btn btn-danger btn-xs" onclick="location.href='<%= GetServiceActionPath("stop", "growsense-mesh-manager.service", "Mesh manager") %>'"><i class="fa fa-stop"></i></div>
                <div class="btn btn-warning btn-xs" onclick="location.href='<%= GetServiceActionPath("disable", "growsense-mesh-manager.service", "Mesh manager") %>'"><i class="fa fa-times-circle"></i></div>
              </td>
            </tr>
            <tr>
              <td> Web UI </td>
              <td>
                <div>
                  <%= GenerateWebUIServiceStatusIcon() %>
                </div>
              </td>
              <td>
                <div class="btn btn-default btn-xs" onclick="location.href='<%= GetViewServicePath("growsense-www.service") %>'"><i class="fa fa-search"></i></div>
                <div class="btn btn-success btn-xs" onclick="location.href='<%= GetServiceActionPath("start", "growsense-www.service", "Web UI") %>'"><i class="fa fa-play"></i></div>
                <div class="btn btn-primary btn-xs" onclick="location.href='<%= GetServiceActionPath("restart", "growsense-www.service", "Web UI") %>'"><i class="fa fa-refresh"></i></div>
                <div class="btn btn-danger btn-xs" onclick="location.href='<%= GetServiceActionPath("stop", "growsense-www.service", "Web UI") %>'"><i class="fa fa-stop"></i></div>
                <div class="btn btn-warning btn-xs" onclick="location.href='<%= GetServiceActionPath("disable", "growsense-www.service", "Web UI") %>'"><i class="fa fa-times-circle"></i></div>
              </td>
            </tr>
            <tr>
              <td> Upgrade </td>
              <td>
                <div>
                  <%= GenerateUpgradeServiceStatusIcon() %>
                </div>
              </td>
              <td>
                <div class="btn btn-default btn-xs" onclick="location.href='<%= GetViewServicePath("growsense-upgrade.service") %>'"><i class="fa fa-search"></i></div>
                <div class="btn btn-success btn-xs" onclick="location.href='<%= GetServiceActionPath("start", "growsense-upgrade.service", "Upgrade") %>'"><i class="fa fa-play"></i></div>
                <div class="btn btn-primary btn-xs" onclick="location.href='<%= GetServiceActionPath("restart", "growsense-upgrade.service", "Upgrade") %>'"><i class="fa fa-refresh"></i></div>
                <div class="btn btn-danger btn-xs" onclick="location.href='<%= GetServiceActionPath("stop", "growsense-upgrade.service", "Upgrade") %>'"><i class="fa fa-stop"></i></div>
                <div class="btn btn-warning btn-xs" onclick="location.href='<%= GetServiceActionPath("disable", "growsense-upgrade.service", "Upgrade") %>'"><i class="fa fa-times-circle"></i></div>
              </td>
            </tr>
          </tbody>
        </table>
        <table class="table table-striped table-advance table-hover">
          <h4><i class="fa fa-angle-right"></i> Devices</h4>
          <hr>
          <thead>
            <tr>
              <th><i class="fa fa-gear"></i> Service</th>
              <th><i class="fa fa-check"></i> Status</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            <% foreach (var device in Devices) { %>
            <tr>
              <td> <%= device.Label %> </td>
              <td>
                <div>
                  <%= GenerateDeviceServiceStatusIcon(device) %>
                </div>
              </td>
              <td>
                <div class="btn btn-default btn-xs" onclick="location.href='<%= GetViewDeviceServicePath(device) %>'"><i class="fa fa-search"></i></div>
                <div class="btn btn-success btn-xs" onclick="location.href='<%= GetDeviceServiceActionPath(device, "start") %>'"><i class="fa fa-play"></i></div>
                <div class="btn btn-primary btn-xs" onclick="location.href='<%= GetDeviceServiceActionPath(device, "restart") %>'"><i class="fa fa-refresh"></i></div>
                <div class="btn btn-danger btn-xs" onclick="location.href='<%= GetDeviceServiceActionPath(device, "stop") %>'"><i class="fa fa-stop"></i></div>
                <div class="btn btn-warning btn-xs" onclick="location.href='<%= GetDeviceServiceActionPath(device, "disable") %>'"><i class="fa fa-times-circle"></i></div>
              </td>
            </tr>
            <% } %>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</asp:Content>


