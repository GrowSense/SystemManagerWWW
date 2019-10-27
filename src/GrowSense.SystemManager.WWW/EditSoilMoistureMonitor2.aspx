<%@ Page Language="C#" Inherits="GrowSense.SystemManager.WWW.EditSoilMoistureMonitor" MasterPageFile="~/Master.master" %>
<asp:Content ContentPlaceHolderID="contentPlaceHolder" ID="contentPlaceHolderContent" runat="server">
  <h3><i class="fa fa-angle-right"></i> Garden</h3>
  <!-- BASIC FORM ELELEMNTS -->
  <div class="row mt">
    <div class="col-lg-12">
      <div class="form-panel">
        <h4 class="mb"><i class="fa fa-angle-right"></i> Edit Soil Moisture Monitor</h4>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Label:</label>
            <div class="col-sm-10">
              <asp:TextBox runat="server" id="Label" CssClass="form-control"></asp:TextBox>
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
            <label class="col-sm-2 col-sm-2 control-label">Calibration:</label>
            <div class="col-sm-10 form-inline">
              <asp:DropDownList runat="server" id="DryCalibration" CssClass="form-control" Style="width: 100px;">
              </asp:DropDownList>
              <asp:DropDownList runat="server" id="WetCalibration" CssClass="form-control" Style="width: 100px;">
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
    <!-- col-lg-12-->
  </div>
</asp:Content>


