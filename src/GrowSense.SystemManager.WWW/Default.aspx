<%@ Page Language="C#" Inherits="GrowSense.SystemManager.Default" MasterPageFile="~/Master.master" EnableViewState="false" %>
<%@ MasterType VirtualPath="~/Master.master" %>
<asp:Content ContentPlaceHolderID="contentPlaceHolder" ID="contentPlaceHolderContent" runat="server">
  <h3><i class="fa fa-angle-right"></i> Overview</h3>
  <div class="row mt">
    <div class="col-lg-12">
      <div class="row">
        <div class="col-md-4 col-sm-4 mb">
          <a href="Devices.aspx">
            <div class="darkblue-panel pn">
              <div class="darkblue-header">
                <h2>Devices</h2>
              </div>
              <h1 class="mt"><i class="fa fa-gears fa-3x"></i></h1>
              <footer>
                <div class="centered">
                  <h5><%= TotalDevices %></h5>
                </div>
              </footer>
            </div>
          </a>
        </div>
        <div class="col-md-4 col-sm-4 mb">
          <a href="Computers.aspx">
            <div class="darkblue-panel pn">
              <div class="darkblue-header">
                <h2>Computers</h2>
              </div>
              <h1 class="mt"><i class="fa fa-desktop fa-3x"></i></h1>
              <footer>
                <div class="centered">
                  <h5><%= TotalComputers %></h5>
                </div>
              </footer>
            </div>
          </a>
        </div>
      </div>
      <div class="row">
        <div class="col-md-4 col-sm-4 mb">
          <a href="Messages.aspx">
            <div class="darkblue-panel pn">
              <div class="darkblue-header">
                <h2>Messages</h2>
              </div>
              <h1 class="mt"><i class="fa fa-comments fa-3x"></i></h1>
              <footer>
                <div class="centered">
                  <h5><%= TotalMessages %></h5>
                </div>
              </footer>
            </div>
          </a>
        </div>
        <div class="col-md-4 col-sm-4 mb">
          <a href="Messages.aspx">
            <div class="darkblue-panel pn">
              <div class="darkblue-header">
                <h2>Alerts</h2>
              </div>
              <h1 class="mt"><i class="fa fa-warning fa-3x"></i></h1>
              <footer>
                <div class="centered">
                  <h5><%= TotalAlerts %></h5>
                </div>
              </footer>
            </div>
          </a>
        </div>
      </div>
    </div>
  </div>
</asp:Content>


