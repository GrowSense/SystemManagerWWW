<%@ Page Language="C#" Inherits="GrowSense.SystemManager.ComputersPage" MasterPageFile="~/Master.master" %>
<%@ MasterType VirtualPath="~/Master.master" %>
<asp:Content ContentPlaceHolderID="contentPlaceHolder" ID="contentPlaceHolderContent" runat="server">
  <script language="javascript">
    var selectedComputer = '';
    
    function selectComputer(computerName)
    {
      selectedComputer = computerName;
    }
    
    function removeComputer()
    {
      location.href='RemoveComputer.aspx?ComputerName=' + selectedComputer;
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
          <h4><i class="fa fa-angle-right"></i> Computers</h4>
          <hr>
          <thead>
            <tr>
              <th><i class="fa fa-check"></i> Status</th>
              <th><i class="fa fa-desktop"></i> Computer</th>
              <th><i class="fa fa-external-link"></i> Path</th>
              <th><div class="btn btn-success btn-xs" onclick="location.href='ComputerForm.aspx'"><i class="fa fa-plus-square"></i></div></th>
            </tr>
          </thead>
          <tbody>
            <% foreach (var computerInfo in ComputersInfo) { %>
            <tr>
              <td>
                <div>
                  <%= GenerateComputerStatusIcon(computerInfo) %>
                </div>
              </td>
              <td>
                <div>
                  <a href="basic_table.html#"><%= computerInfo.Name %></a>
                </div>
              </td>
              <td>
                <div><a href="basic_table.html#"><%= computerInfo.Host %></a></div>
              </td>
              <td>
                <div class="btn btn-primary btn-xs" onclick="location.href='ComputerForm.aspx?ComputerName=<%= computerInfo.Name %>'"><i class="fa fa-pencil"></i></div>
                <div class="btn btn-danger btn-xs" data-toggle="modal" data-target="#removeModal" onclick="selectComputer('<%= computerInfo.Name %>');"><i class="fa fa-trash-o"></i></div>
              </td>
            </tr>
            <% } %>
          </tbody>
        </table>
        <div class="modal fade" id="removeModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
          <div class="modal-dialog">
            <div class="modal-content">
              <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title" id="myModalLabel">Confirm Removal</h4>
              </div>
              <div class="modal-body">
                Are you sure you want to remove this computer?
              </div>
              <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                <button type="button" class="btn btn-primary" onclick="javascript:removeComputer()">Remove Computer</button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</asp:Content>


