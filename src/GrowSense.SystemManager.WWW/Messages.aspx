<%@ Page Language="C#" Inherits="GrowSense.SystemManager.WWW.Messages" MasterPageFile="~/Master.master" %>
<%@ MasterType VirtualPath="~/Master.master" %>
<asp:Content ContentPlaceHolderID="contentPlaceHolder" ID="contentPlaceHolderContent" runat="server">
  <h3><i class="fa fa-angle-right"></i> Garden </h3> 
  <div class="row mt">
    <div class="col-md-12">
      <% if (!String.IsNullOrEmpty(Request.QueryString["Result"])){ %>
      <div class="alert alert-<%= (Request.QueryString["IsSuccess"] == "false" ? "danger" : "success") %> "><%= Request.QueryString["Result"] %></div>
      <% } %>
      <div class="content-panel">
        <table class="table table-striped table-advance table-hover">
          <h4><i class="fa fa-angle-right"></i> Messages</h4>
          <hr>
          <% if (MessagesInfo.Length == 0) { %>
          <thead>
            <tr>
              <th><i class="fa fa-bullhorn"></i> Message</th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td>
                <div>No messages found.</div>
              </td>
            </tr>
          </tbody>
          <% } else { %>
          <thead>
            <tr>
              <th><i class="fa fa-clock-o"></i> Time</th>
              <th><i class="fa fa-tag"></i> Type</th>
              <th><i class="fa fa-bullhorn"></i> Message</th>
              <th><i class="fa fa-desktop"></i> Source</th>
              <th><div class="btn btn-danger btn-xs" onclick="location.href='RemoveMessage.aspx?MessageId=all'"><i class="fa fa-trash-o "></i></div></th>
            </tr>
          </thead>
          <tbody>
            <% foreach (var messageInfo in MessagesInfo) { %>
            <tr>
              <td>
                <div><%= messageInfo.Timestamp.ToString() %></div>
              </td>
              <td>
                <div><%= GenerateMessageTypeIcon(messageInfo.Type) %></div>
              </td>
              <td>
                <div><%= messageInfo.Text %></div>
              </td>
              <td>
                <div><%= messageInfo.Host %></div>
              </td>
              <td>
                <div class="btn btn-danger btn-xs" onclick="location.href='RemoveMessage.aspx?MessageId=<%= messageInfo.Id %>'"><i class="fa fa-trash-o "></i></div>
              </td>
            </tr>
            <% } %>
          </tbody>
          <% } %>
        </table>
      </div>
    </div>
  </div>
</asp:Content>


