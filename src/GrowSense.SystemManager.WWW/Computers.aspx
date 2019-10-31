<%@ Page Language="C#" Inherits="GrowSense.SystemManager.ComputersPage" MasterPageFile="~/Master.master" %>
<%@ MasterType VirtualPath="~/Master.master" %>
<asp:Content ContentPlaceHolderID="contentPlaceHolder" ID="contentPlaceHolderContent" runat="server">
  <h3><i class="fa fa-angle-right"></i> Garden </h3>
  <div class="row mt">
    <div class="col-md-12">
      <div class="content-panel">
        <table class="table table-striped table-advance table-hover">
          <h4><i class="fa fa-angle-right"></i> Computers</h4>
          <hr>
          <thead>
            <tr>
              <th><i class="fa fa-desktop"></i> Computer</th>
              <th><i class="fa fa-external-link"></i> Path</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            <% foreach (var computerInfo in ComputersInfo) { %>
            <tr>
              <td>
                <div><a href="basic_table.html#"><%= computerInfo.Name %></a></div>
              </td>
              <td>
                <div><a href="basic_table.html#"><%= computerInfo.Host %></a></div>
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
</asp:Content>


