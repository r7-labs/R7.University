<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewDivision.ascx.cs" Inherits="R7.University.Division.ViewDivision" %>

<asp:Panel id="panelDivision" runat="server">
	<asp:Label id="labelTitle" runat="server" CssClass="Division_Label" />
	<asp:HyperLink id="linkHomePage" runat="server" CssClass="Division_Label" />
	<asp:HyperLink id="linkEmail" runat="server" CssClass="Division_Label email" />
	<asp:HyperLink id="linkSecondaryEmail" runat="server" CssClass="Division_Label email" />
	<asp:Label id="labelPhone" runat="server" CssClass="Division_Label" />
	<asp:Label id="labelFax" runat="server" CssClass="Division_Label" />
	<asp:Label id="labelLocation" runat="server" CssClass="Division_Label" />
	<asp:Label id="labelWorkingHours" runat="server" CssClass="Division_Label" />
	<div class="Division_BarCodeContainer">
		<asp:Image id="imageBarcode" runat="server" />
	</div>
	<asp:Repeater id="repeatSubDivisions" runat="server">
		<HeaderTemplate><ul></HeaderTemplate>
		<ItemTemplate>
			<li>
				<asp:HyperLink id="linkSubDivision" runat="server"></asp:HyperLink>
			</li>
		</ItemTemplate>
		<FooterTemplate></ul></FooterTemplate>
	</asp:Repeater>
	<asp:Label id="labelSearchByTerm" runat="server" CssClass="Division_LabelRight" resourcekey="labelSearchByTerm" />
	<asp:HyperLink id="linkTerm" runat="server" CssClass="Division_LabelRight" />
</asp:Panel>