<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewEmployeeList.ascx.cs" Inherits="R7.University.EmployeeList.ViewEmployeeList" %>
<asp:DataList ID="listEmployees" DataKeyField="EmployeeID" runat="server" CssClass="dnnGrid EmployeeList" 
	OnItemDataBound="listEmployees_ItemDataBound">
	<ItemTemplate>
		<div class="_listitemwrapper">
			<asp:HyperLink id="linkDetails" runat="server" CssClass="_detailslink">	
				<asp:Image id="imagePhoto" runat="server" CssClass="_photo" />
			</asp:HyperLink>
			<div class="_info">	
				<asp:HyperLink id="linkEdit" runat="server" CssClass="_editlink">
					<asp:Image id="imageEdit" runat="server" ImageUrl="~/images/edit.gif" AlternateText="Edit" ResourceKey="Edit" />
				</asp:HyperLink>
				<asp:Label id="labelFullName" runat="server" CssClass="_fullname"  />
				<asp:Label id="labelAcademicDegreeAndTitle" runat="server" />
				<asp:Label id="labelPositions" runat="server" CssClass="_positions" />
				<asp:HyperLink id="linkEmail" runat="server" CssClass="_email email" />
				<asp:HyperLink id="linkSecondaryEmail" runat="server" CssClass="_email email" />
				<asp:HyperLink id="linkWebSite" runat="server" CssClass="_website" />
				<asp:HyperLink id="linkUserProfile" runat="server" CssClass="more _userprofile" />
				<asp:Label id="labelPhones" runat="server" CssClass="_phones" />
			</div>
		</div>
	</ItemTemplate>
	<ItemStyle CssClass="_listitem dnnGridItem" />
	<AlternatingItemStyle CssClass="_listitem dnnGridAltItem" />
</asp:DataList>

