<%@ Page Language="C#" Inherits="GrowSense.SystemManager.WWW.Messages" MasterPageFile="~/Master.master" %>
<%@ MasterType VirtualPath="~/Master.master" %>
<asp:Content ContentPlaceHolderID="contentPlaceHolder" ID="contentPlaceHolderContent" runat="server">
  <script language="javascript">
    var selectedMessageId = '';
    
    function selectMessage(messageId)
    {
      selectedMessageId = messageId;
    }
    
    function removeMessage()
    {
      location.href='RemoveMessage.aspx?MessageId=' + selectedMessageId;
    }
    
    function removeAllMessages()
    {
      location.href='RemoveMessage.aspx?MessageId=all';
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
              <th><div class="btn btn-danger btn-xs" data-toggle="modal" data-target="#removeAllModal" onclick="selectMessage('all');"><i class="fa fa-trash-o "></i></div></th>
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
                <div class="btn btn-danger btn-xs" data-toggle="modal" data-target="#removeModal" onclick="selectMessage('<%= messageInfo.Id %>');"><i class="fa fa-trash-o "></i></div>
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
                Are you sure you want to remove this message?
              </div>
              <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                <button type="button" class="btn btn-primary" onclick="javascript:removeMessage()">Remove Message</button>
              </div>
            </div>
          </div>
        </div>
        <div class="modal fade" id="removeAllModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
          <div class="modal-dialog">
            <div class="modal-content">
              <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title" id="myModalLabel">Confirm Removal</h4>
              </div>
              <div class="modal-body">
                Are you sure you want to remove all messages?
              </div>
              <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                <button type="button" class="btn btn-primary" onclick="javascript:removeAllMessages()">Remove All Messages</button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</asp:Content>


