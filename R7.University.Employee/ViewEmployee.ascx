<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewEmployee.ascx.cs" Inherits="R7.University.Employee.ViewEmployee" %>

<asp:Panel id="panelEmployee" runat="server">

	<div class="Employee_PhotoContainer">
		<asp:Image id="imagePhoto" runat="server" CssClass="Employee_Photo" />
	</div>

	<div class="Employee_Name">
		<asp:Label id="labelFullName" runat="server" CssClass="Employee_FullName" />
		<asp:Label id="labelAcademicDegreeAndTitle" runat="server" />
	</div>

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

	<div class="Employee_OnlineContacts">
		<asp:HyperLink id="linkEmail" runat="server" CssClass="email Employee_Email" />
		<asp:HyperLink id="linkSecondaryEmail" runat="server" CssClass="email Employee_Email" />
		<asp:HyperLink id="linkWebSite" runat="server" Target="_blank" CssClass="Employee_Label" />
		<asp:Label id="labelMessenger" runat="server" CssClass="Employee_Label" />
	</div>

	<div class="Employee_OfflineContacts">
		<asp:Label id="labelPhone" runat="server" CssClass="Employee_Label" />
		<asp:Label id="labelFax" runat="server" CssClass="Employee_Label" />
		<asp:Label id="labelCellPhone" runat="server" CssClass="Employee_Label" />
		<asp:Label id="labelWorkingPlaceAndHours" runat="server" CssClass="Employee_Label" />
	</div>

	<asp:HyperLink id="linkUserProfile" runat="server" CssClass="more Employee_UserProfile" />
</asp:Panel>