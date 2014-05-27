<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewEmployeeDetails.ascx.cs" Inherits="R7.University.Employee.ViewEmployeeDetails" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnJsInclude runat="server" FilePath="dnn.jquery.js" PathNameAlias="SharedScripts" />

<script type="text/javascript">
	$(function() { $( "#employeeTabs" ).dnnTabs( <%= (!IsPostBack)? "{selected: 0}" : "" %>  ); });
</script>

<div class="dnnForm dnnClear">
	
	<asp:Image id="imagePhoto" runat="server" Style="vertical-align:top;margin-right:10px;margin-top:2px" />	
	
	<div id="employeeTabs" class="dnnForm dnnClear" style="display:inline-block;width:auto">
		<ul class="dnnAdminTabNav dnnClear">
		    <li><a href="#employeeCommon">Common</a></li>
			<li><a href="#employeeAchivements">Achivements</a></li>
		    <li><a href="#employeeAbout">About</a></li>
			<li><a href="#employeeBarcode">QR-code</a></li>
		</ul>

		<div id="employeeCommon">

			<asp:Label id="labelAcademicDegreeAndTitle" runat="server" />

			<asp:Repeater id="repeaterPositions" runat="server" OnItemDataBound="repeaterPositions_ItemDataBound">
				<HeaderTemplate><ul class="Employee_Positions"></HeaderTemplate>
				<ItemTemplate>
					<li>
						<asp:Label id="labelPosition" runat="server" />
						<asp:Label id="labelDivision" runat="server" />
						<asp:HyperLink id="linkDivision" runat="server" />
					</li>
				</ItemTemplate>
				<FooterTemplate></ul></FooterTemplate>
			</asp:Repeater>
						
			<div class="EmployeeDetails_Block">
				<asp:HyperLink id="linkEmail" runat="server" CssClass="email EmployeeDetails_Email" />
				<asp:HyperLink id="linkSecondaryEmail" runat="server" CssClass="email EmployeeDetails_Email" />
				<asp:HyperLink id="linkWebSite" runat="server" Target="_blank" CssClass="EmployeeDetails_WebSite" />
			</div>

			<div class="EmployeeDetails_Block">
				<asp:Label id="labelMessenger" runat="server" CssClass="EmployeeDetails_Label" />
			</div>
			
			<div class="EmployeeDetails_Block">
				
			</div>

			<div class="EmployeeDetails_Block">
				<asp:Label id="labelPhone" runat="server" CssClass="EmployeeDetails_Label" />
				<asp:Label id="labelFax" runat="server" CssClass="EmployeeDetails_Label" />
				<asp:Label id="labelCellPhone" runat="server" CssClass="EmployeeDetails_Label" />
			</div>

			<div class="EmployeeDetails_Block">
				<asp:Label id="labelWorkingPlaceAndHours" runat="server" CssClass="EmployeeDetails_Label" />
			</div>
			

</div>

		<div id="employeeAchivements">		
		</div>

		<div id="employeeAbout">
			<asp:Literal id="litAbout" runat="server" />
		</div>
		
		<div id="employeeBarcode">
			<asp:Label runat="server" resourcekey="BarcodeScan.Text" CssClass="dnnFormMessage" />
			<asp:Image id="imageBarcode" runat="server" CssClass="EmployeeDetails_Barcode" />
		</div>

	</div>

	<ul class="dnnActions dnnClear">
		<hr />
		<li><asp:HyperLink id="linkReturn" runat="server" CssClass="dnnPrimaryAction" ResourceKey="cmdReturn" /></li>
		<li><asp:HyperLink id="linkUserProfile" runat="server" CssClass="dnnSecondaryAction" ResourceKey="VisitProfile.Text" /></li>
		<li><asp:HyperLink id="linkVCard" runat="server" CssClass="dnnSecondaryAction" ResourceKey="VCard.Action" /></li>
	</ul>
</div>
