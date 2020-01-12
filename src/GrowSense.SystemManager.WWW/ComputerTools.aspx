<%@ Page Language="C#" Inherits="GrowSense.SystemManager.WWW.ComputerTools" MasterPageFile="~/Master.master" %>
<%@ MasterType VirtualPath="~/Master.master" %>
<asp:Content ContentPlaceHolderID="contentPlaceHolder" ID="contentPlaceHolderContent" runat="server">
  <h3><i class="fa fa-angle-right"></i> Computer Tools: <%= Computer.Name %> </h3>
  <div class="row mt">
    <div class="col-md-12">
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
              </td>
            </tr>
            <% } %>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</asp:Content>


