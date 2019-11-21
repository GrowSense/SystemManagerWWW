<%@ Control Language="C#" Inherits="GrowSense.SystemManager.LeftMenu" %>
<!--sidebar start-->
<aside>
  <div id="sidebar" class="nav-collapse ">
    <!-- sidebar menu start-->
    <ul class="sidebar-menu" id="nav-accordion">
      <li class="mt">
        <a<%= GetActiveClassAttribute("Default") %> href="Default.aspx">
          <i class="fa fa-dashboard"></i>
          <span>Dashboard</span>
          </a>
      </li>
      <li class="sub-menu">
        <a href="javascript:;">
          <i class="fa fa-desktop"></i>
          <span>Garden</span>
        </a>
        <ul class="sub">
          <li<%= GetActiveClassAttribute("Devices") %>><a href="Devices.aspx">Devices</a></li>
          <li<%= GetActiveClassAttribute("Computers") %>><a href="Computers.aspx">Computers</a></li>
          <li<%= GetActiveClassAttribute("Messages") %>><a href="Messages.aspx">Messages</a></li>
        </ul>
      </li>
    </ul>
    <!-- sidebar menu end-->
  </div>
</aside>
 <!--sidebar end-->