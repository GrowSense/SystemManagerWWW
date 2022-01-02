<%@ Page Language="C#" Inherits="GrowSense.SystemManager.WWW.CalibrateSoilMoistureSensor" MasterPageFile="~/Master.master" %>
<%@ MasterType VirtualPath="~/Master.master" %>
<%@ Import Namespace="GrowSense.SystemManager.Web" %>
<asp:Content ContentPlaceHolderID="contentPlaceHolder" ID="contentPlaceHolderContent" runat="server">
  <script>
    function updateValues()
    {
      console.log("Updating values");
        
      $.getJSON( "DevicesJson.aspx?Name=<%= Device.Name %>", function( device ) {
        //$.each( data, function( key, device ) {
          updateDevice(device);
        //});
      });  
    }
    
    function updateDevice(device)
    {
      if (device.Group == "irrigator")
      {
        var width = 100 / 1024 * device.Data["R"];
        $("#rawBar").attr('aria-valuenow', device.Data["R"]).css('width', width + "%");
      }
    }
    
    function setDry()
    {
      console.log("Setting dry value");
      
      var rawValue = $("#rawBar").attr('aria-valuenow');
      
      var width = 100 / 1024 * rawValue;
      $("#dryBar").attr('aria-valuenow', rawValue).css('width', width + "%");
      $("#DryValue").val(rawValue);
    }
    
    function setWet()
    {
      console.log("Setting wet value");
    
      var rawValue = $("#rawBar").attr('aria-valuenow');
      
      var width = 100 / 1024 * rawValue;
      $("#wetBar").attr('aria-valuenow', rawValue).css('width', width + "%");
      $("#WetValue").val(rawValue);
    }
    
    $( document ).ready(function() {
      updateValues();
      setInterval(updateValues, 1000);
    });
  </script>
  <h3><i class="fa fa-angle-right"></i> Garden</h3>
  <div class="row mt">
    <div class="col-lg-12">
      <% if (!String.IsNullOrEmpty(Result)){ %>
      <div class="alert alert-<%= (IsSuccess ? "success" : "danger") %> "><%= Result %></div>
      <% } %>
      <div class="form-panel">
        <h4 class="mb"><i class="fa fa-angle-right"></i> Calibrate Soil Moisture Sensor</h4>
        <div class="form-horizontal style-form">
          <div class="form-group">
            <div class="col-sm-10">
              Calibration is used to convert a raw sensor reading into a user friendly percentage.
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 col-sm-2 control-label">Device:</label>
            <div class="col-sm-10">
              <%= Device.Name %>
            </div>
          </div>
          <div class="form-group" id="rawMoistureValue">
            <label class="col-sm-2 col-sm-2 control-label">Raw:</label>
            <div class="col-sm-10 form-inline">
              <div class="progress form-inline" style="width: 200px;">
                <%= GenerateRawProgressBar() %>
              </div>
            </div>
          </div>
          <div class="form-group">
            <div class="col-sm-10">
              <strong>Step 1:</strong> Insert the soil moisture sensor into dry soil, wait for the raw sensor reading (above) to update, and click "Set Dry".
            </div>
            <div class="col-sm-10">
              <strong>Step 2:</strong> Insert the soil moisture sensor into wet soil, wait for the raw sensor reading (above) to update, and click "Set Wet".
            </div>
          </div>
          <div class="form-group" id="dryMoistureValue">
            <label class="col-sm-2 col-sm-2 control-label">Dry:</label>
            <div class="col-sm-10 form-inline">
              <div class="progress form-inline" style="width: 200px;">
                <%= GenerateDryProgressBar() %>
              </div>
              <button class="btn btn-warning btn-xs" type="button" onclick="setDry();">Set Dry</button>
              <input type="hidden" id="DryValue" name="DryValue" value='<%= Utility.GetDeviceData (Device.Name, "D") %>' />
            </div>
          </div>
          <div class="form-group" id="wetMoistureValue">
            <label class="col-sm-2 col-sm-2 control-label">Wet:</label>
            <div class="col-sm-10 form-inline">
              <div class="progress form-inline" style="width: 200px;">
                <%= GenerateWetProgressBar() %>
              </div>
              <button class="btn btn-primary btn-xs" type="button" onclick="setWet();">Set Wet</button>
              <input type="hidden" id="WetValue" name="WetValue" value='<%= Utility.GetDeviceData (Device.Name, "W") %>' />
            </div>
          </div>
          <div class="form-group">
            <div class="col-lg-offset-2 col-lg-10">
              <asp:Button runat="server" CssClass="btn btn-theme" OnClick="SaveButton_Click" type="submit" Text="Save" />
              <button class="btn btn-theme04" type="button" onclick="location.href='Devices.aspx'">Cancel</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</asp:Content>


