<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewDivision.ascx.cs" Inherits="R7.University.Division.ViewDivision" %>

<asp:Panel id="panelDivision" runat="server">
	<asp:Label id="labelTitle" runat="server" CssClass="Division_Title" />
	<asp:HyperLink id="linkHomePage" runat="server" CssClass="Division_Title" />

	<asp:HyperLink id="linkSearchByTerm" runat="server" resourcekey="linkSearchByTerm" CssClass="Division_Section tag" />

	<div class="Division_Section">
		<asp:Label id="labelPhone" runat="server" CssClass="Division_Label" />
		<asp:Label id="labelFax" runat="server" CssClass="Division_Label" />
	</div>

	<div class="Division_Section">
		<asp:HyperLink id="linkEmail" runat="server" CssClass="Division_Label email" />
		<asp:HyperLink id="linkSecondaryEmail" runat="server" CssClass="Division_Label email" />
	</div>

	<div class="Division_Section">
		<asp:Label id="labelLocation" runat="server" CssClass="Division_Label" />
		<asp:Label id="labelWorkingHours" runat="server" CssClass="Division_Label" />
	</div>

	<asp:Repeater id="repeatSubDivisions" runat="server" OnItemDataBound="repeaterSubDivisions_ItemDataBound">
		<HeaderTemplate><ul class="Division_Section"></HeaderTemplate>
		<ItemTemplate>
			<li>
				<asp:HyperLink id="linkSubDivision" runat="server" />
				<asp:Label id="labelSubDivision" runat="server" />
			</li>
		</ItemTemplate>
		<FooterTemplate></ul></FooterTemplate>
	</asp:Repeater>

	<asp:Image id="imageBarcode" runat="server" resourcekey="imageBarcode" CssClass="Division_Barcode" />
</asp:Panel>