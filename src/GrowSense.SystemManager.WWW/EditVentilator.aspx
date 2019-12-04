<%@ Page Language="C#" Inherits="GrowSense.SystemManager.WWW.EditVentilator" MasterPageFile="~/Master.master" %>
<%@ MasterType VirtualPath="~/Master.master" %>
<asp:Content ContentPlaceHolderID="contentPlaceHolder" ID="contentPlaceHolderContent" runat="server">
<h3><i class="fa fa-angle-right"></i> Garden</h3>
  <div class="row mt">
    <div class="col-lg-12">
      <div class="form-panel">
        <h4 class="mb"><i class="fa fa-angle-right"></i> Edit Irrigator</h4>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Label:</label>
            <div class="col-sm-10">
              <asp:TextBox runat="server" id="Label" CssClass="form-control"></asp:TextBox>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Name:</label>
            <div class="col-sm-10">
              <asp:TextBox runat="server" id="Name" CssClass="form-control"></asp:TextBox>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Reading Interval:</label>
            <div class="col-sm-10 form-inline">
              <asp:TextBox runat="server" id="ReadingIntervalQuantity" CssClass="form-control" Style="width: 100px;"></asp:TextBox>
              <asp:DropDownList runat="server" id="ReadingIntervalType" CssClass="form-control" Style="width: 100px;">
                 <asp:ListItem Enabled="true" Text="Seconds" Value="Seconds"></asp:ListItem>
                 <asp:ListItem Enabled="true" Text="Minutes" Value="Minutes"></asp:ListItem>
                 <asp:ListItem Enabled="true" Text="Hours" Value="Hours"></asp:ListItem>
              </asp:DropDownList>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Fan Mode:</label>
            <div class="col-sm-10">
              <asp:DropDownList runat="server" id="FanMode" CssClass="form-control" Style="width: 100px;">
                 <asp:ListItem Enabled="true" Text="Off" Value="0"></asp:ListItem>
                 <asp:ListItem Enabled="true" Text="On" Value="1"></asp:ListItem>
                 <asp:ListItem Enabled="true" Text="Auto" Value="2"></asp:ListItem>
              </asp:DropDownList>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Temperature:</label>
            <div class="col-sm-10 form-inline">
              Minimum
              <asp:DropDownList runat="server" id="MinimumTemperature" CssClass="form-control" Style="width: 100px;">
              </asp:DropDownList>
              &nbsp;&nbsp;&nbsp;
              Maximum
              <asp:DropDownList runat="server" id="MaximumTemperature" CssClass="form-control" Style="width: 100px;">
              </asp:DropDownList>
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Humidity:</label>
            <div class="col-sm-10 form-inline">
              Minimum
              <asp:DropDownList runat="server" id="MinimumHumidity" CssClass="form-control" Style="width: 100px;">
              </asp:DropDownList>
              &nbsp;&nbsp;&nbsp;
              Maximum
              <asp:DropDownList runat="server" id="MaximumHumidity" CssClass="form-control" Style="width: 100px;">
              </asp:DropDownList>
            </div>
          </div>
          <div class="form-group">
            <div class="col-lg-offset-2 col-lg-10">
              <asp:Button runat="server" CssClass="btn btn-theme" type="submit" Text="Save" />
              <button class="btn btn-theme04" type="button" onclick="location.href='Devices.aspx'">Cancel</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</asp:Content>


