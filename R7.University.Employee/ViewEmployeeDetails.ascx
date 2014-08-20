<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewEmployeeDetails.ascx.cs" Inherits="R7.University.Employee.ViewEmployeeDetails" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnJsInclude runat="server" FilePath="dnn.jquery.js" PathNameAlias="SharedScripts" />

<script type="text/javascript">
	$(function() { $( "#employeeTabs" ).dnnTabs( <%= (!IsPostBack)? "{selected: 0}" : "" %>  ); });
</script>

<div class="dnnForm dnnClear EmployeeDetails">
	
	<asp:Image id="imagePhoto" runat="server" CssClass="_photo" />	
	
	<div id="employeeTabs" class="dnnForm dnnClear _tabs">
		<ul class="dnnAdminTabNav dnnClear">
		    <li><a href="#employeeCommon"><%= LocalizeString("CommonTab.Text") %></a></li>
			<li><asp:HyperLink id="linkExperience" runat="server" href="#employeeExperience"><%= LocalizeString("ExperienceTab.Text") %></asp:HyperLink></li>
			<li><asp:HyperLink id="linkAchievements" runat="server" href="#employeeAchievements" Visible="false"><%= LocalizeString("AchievementsTab.Text") %></asp:HyperLink></li>
		  	<li><asp:HyperLink id="linkAbout" runat="server" href="#employeeAbout"><%= LocalizeString("AboutTab.Text") %></asp:HyperLink></li>
			<li><a href="#employeeBarcode"><%= LocalizeString("BarcodeTab.Text") %></a></li>
		</ul>

		<div id="employeeCommon" class="_tab">

			<asp:Label id="labelAcademicDegreeAndTitle" runat="server" />

			<asp:Repeater id="repeaterPositions" runat="server" OnItemDataBound="repeaterPositions_ItemDataBound">
				<HeaderTemplate><ul class="_positions"></HeaderTemplate>
				<ItemTemplate>
					<li>
						<asp:Label id="labelPosition" runat="server" />
						<asp:Label id="labelDivision" runat="server" />
						<asp:HyperLink id="linkDivision" runat="server" />
					</li>
				</ItemTemplate>
				<FooterTemplate></ul></FooterTemplate>
			</asp:Repeater>
						
			<div class="_section">
				<asp:HyperLink id="linkEmail" runat="server" CssClass="email _email" />
				<asp:HyperLink id="linkSecondaryEmail" runat="server" CssClass="email _email" />
				<asp:HyperLink id="linkWebSite" runat="server" Target="_blank" CssClass="_website" />
			</div>

			<div class="_section">
				<asp:Label id="labelMessenger" runat="server" CssClass="_label" />
			</div>
			
			<div class="_section">
				<asp:Label id="labelPhone" runat="server" CssClass="_label" />
				<asp:Label id="labelFax" runat="server" CssClass="_label" />
				<asp:Label id="labelCellPhone" runat="server" CssClass="_label" />
			</div>

			<div class="_section">
				<asp:Label id="labelWorkingPlaceAndHours" runat="server" CssClass="_label" />
			</div>

		</div>

		<div id="employeeExperience" class="_tab">	
			<div class="_section" style="margin-bottom:10px">
				<asp:GridView id="gridExperience" runat="server" AutoGenerateColumns="true" 
						OnRowDataBound="gridExperience_RowDataBound" GridLines="None">
						<HeaderStyle CssClass="dnnGridHeader" horizontalalign="Left" />
				        <RowStyle CssClass="dnnGridItem" horizontalalign="Left" />
				        <AlternatingRowStyle CssClass="dnnGridAltItem" />
				        <SelectedRowStyle CssClass="dnnFormError" />
				        <FooterStyle CssClass="dnnGridFooter" />
				    </asp:GridView>
				</div>
			<asp:Label id="labelExperienceYears" runat="server" CssClass="_label" />
		</div>

		<div id="employeeAchievements" class="_tab">		
		</div>

		<div id="employeeAbout" class="_tab">
			<asp:Literal id="litAbout" runat="server" />
		</div>
		
		<div id="employeeBarcode" class="_tab">
			<asp:Label runat="server" resourcekey="BarcodeScan.Text" CssClass="dnnFormMessage" />
			<asp:Image id="imageBarcode" runat="server" CssClass="_barcode" />
		</div>

	</div>

	<ul class="dnnActions dnnClear">
		<hr />
		<li><asp:HyperLink id="linkReturn" runat="server" CssClass="dnnPrimaryAction" ResourceKey="cmdReturn" /></li>
		<li><asp:HyperLink id="linkUserProfile" runat="server" CssClass="dnnSecondaryAction" ResourceKey="VisitProfile.Text" /></li>
		<li><asp:HyperLink id="linkVCard" runat="server" CssClass="dnnSecondaryAction" ResourceKey="VCard.Action" Visible="false" /></li>
	</ul>
</div>
