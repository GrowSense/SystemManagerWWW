<%@ Page Language="C#" Inherits="GrowSense.SystemManager.DevicesPage" MasterPageFile="~/Master.master" %>
<%@ MasterType VirtualPath="~/Master.master" %>
<%@ Import Namespace="GrowSense.SystemManager.Web" %>
<asp:Content ContentPlaceHolderID="contentPlaceHolder" ID="contentPlaceHolderContent" runat="server">
  <section id="main-content">
    <section class="wrapper">
      <h3><i class="fa fa-angle-right"></i> Garden </h3>
      <div class="row mt">
        <div class="col-md-12">
          <div class="content-panel">
            <table class="table table-striped table-advance table-hover">
              <h4><i class="fa fa-angle-right"></i> Devices</h4>
              <hr>
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
                    <button class="btn btn-primary btn-xs"><i class="fa fa-pencil"></i></button>
                    <button class="btn btn-danger btn-xs"><i class="fa fa-trash-o "></i></button>
                  </td>
                </tr>
                <% } %>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </section>
  </section>
</asp:Content>


