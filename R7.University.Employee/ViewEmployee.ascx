<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewEmployee.ascx.cs" Inherits="R7.University.Employee.ViewEmployee" %>

<asp:Panel id="panelEmployee" runat="server" CssClass="Employee">
	<div class="_photo">
        <asp:HyperLink id="linkPhoto" runat="server" >
			<asp:Image id="imagePhoto" runat="server" />
		</asp:HyperLink>
	</div>
	<div class="_name">
		<asp:Label id="labelFullName" runat="server" CssClass="_fullname" />
		<asp:Label id="labelAcademicDegreeAndTitle" runat="server" />
	</div>
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
		<asp:HyperLink id="linkWebSite" runat="server" Target="_blank" CssClass="_label" />
		<asp:Label id="labelMessenger" runat="server" CssClass="_label" />
	</div>
    <div class="_section">
		<asp:Label id="labelPhone" runat="server" CssClass="_label" />
		<asp:Label id="labelFax" runat="server" CssClass="_label" />
		<asp:Label id="labelCellPhone" runat="server" CssClass="_label" />
		<asp:Label id="labelWorkingPlaceAndHours" runat="server" CssClass="_label" />
	</div>
	<asp:HyperLink id="linkUserProfile" runat="server" resourcekey="VisitProfile.Text" CssClass="_userprofile more" />
</asp:Panel>
