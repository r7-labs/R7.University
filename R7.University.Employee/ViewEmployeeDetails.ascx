<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewEmployeeDetails.ascx.cs" Inherits="R7.University.Employee.ViewEmployeeDetails" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnJsInclude runat="server" FilePath="dnn.jquery.js" PathNameAlias="SharedScripts" />

<script type="text/javascript">
	$(function() { $( "#employeeTabs" ).dnnTabs( <%= (!IsPostBack)? "{selected: 0}" : "" %>  ); });
</script>

<div class="dnnForm dnnClear">
	
	<div id="employeeTabs" class="dnnForm dnnClear">
		<ul class="dnnAdminTabNav dnnClear">
		    <li><a href="#employeeCommon">Common</a></li>
		    <li><a href="#employeeAbout">About</a></li>
		</ul>
	
		<div id="employeeCommon">
			<asp:Image id="imagePhoto" runat="server" />
		</div>
		<div id="employeeAbout">
		</div>

	</div>

	<ul class="dnnActions dnnClear">
		<hr />
		<li><asp:HyperLink id="linkReturn" runat="server" CssClass="dnnPrimaryAction" ResourceKey="cmdReturn" /></li>
		<li><asp:HyperLink id="linkVCard" runat="server" CssClass="dnnSecondaryAction" ResourceKey="VCard.Action" /></li>
	</ul>
</div>
