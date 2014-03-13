<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewEmployeeList.ascx.cs" Inherits="R7.University.EmployeeList.ViewEmployeeList" %>
<asp:DataList ID="listEmployees" DataKeyField="EmployeeID" runat="server" CssClass="EmployeeList_ContentList dnnGrid" 
	OnItemDataBound="listEmployees_ItemDataBound">
	<ItemTemplate>
		<asp:HyperLink id="linkEdit" runat="server" CssClass="EmployeeList_EditLink">
			<asp:Image id="imageEdit" runat="server" ImageUrl="~/images/edit.gif" AlternateText="Edit" ResourceKey="Edit" />
		</asp:HyperLink>
		<asp:Image id="imagePhoto" runat="server" CssClass="EmployeeList_Photo" />
		<asp:Label id="labelFullName" runat="server" CssClass="EmployeeList_FullName"  />
		<asp:Label id="labelAcademicDegreeAndTitle" runat="server" CssClass="EmployeeList_AcademicDegreeAndTitle" />
		<asp:Label id="labelPositions" runat="server" CssClass="EmployeeList_Positions" />
		<asp:HyperLink id="linkEmail" runat="server" CssClass="EmployeeList_Email email" />
		<asp:HyperLink id="linkWebSite" runat="server" CssClass="EmployeeList_WebSite" />
		<asp:Label id="labelPhones" runat="server" CssClass="EmployeeList_Phones" />
	</ItemTemplate>
	<ItemStyle CssClass="EmployeeList_ContentListItem dnnGridItem" />
	<AlternatingItemStyle CssClass="EmployeeList_ContentListItem dnnGridAltItem" />
</asp:DataList>

