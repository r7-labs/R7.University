<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewDivision.ascx.cs" Inherits="Division.ViewDivision" %>
<asp:DataList ID="lstContent" DataKeyField="DivisionID" runat="server" CssClass="Division_ContentList" OnItemDataBound="lstContent_ItemDataBound">
	<ItemTemplate>
		<asp:HyperLink ID="linkEdit" runat="server">
			<asp:Image ID="imageEdit" runat="server" ImageUrl="~/images/edit.gif" AlternateText="Edit" ResourceKey="Edit" />
		</asp:HyperLink>
		<asp:Label ID="lblUserName" runat="server" CssClass="Division_UserName" />
		<asp:Label ID="lblCreatedOnDate" runat="server" CssClass="Division_CreatedOnDate" /> 
		<asp:Label ID="lblContent" runat="server" CssClass="Division_Content" />
	</ItemTemplate>
	<ItemStyle CssClass="Division_ContentListItem" />
</asp:DataList>

