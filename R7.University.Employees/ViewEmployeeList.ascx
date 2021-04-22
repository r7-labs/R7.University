<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="ViewEmployeeList.ascx.cs" Inherits="R7.University.Employees.ViewEmployeeList" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="AgplSignature" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/AgplSignature.ascx" %>
<%@ Import Namespace="R7.University.Dnn" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/assets/css/module.css" />

<asp:DataList ID="listEmployees" DataKeyField="EmployeeID" runat="server" RepeatLayout="Flow" CssClass="u8y-employee-list"
	OnItemDataBound="listEmployees_ItemDataBound">
	<ItemTemplate>
		<div class="row">
			<div class="col-md-2">
				<asp:HyperLink id="linkDetails" runat="server">
					<asp:Image id="imagePhoto" runat="server" CssClass="img-fluid" />
				</asp:HyperLink>
			</div>
			<div class="col-md-10">
				<asp:HyperLink id="linkEdit" runat="server" CssClass="_editlink">
					<asp:Image id="imageEdit" runat="server" ImageUrl="<%# UniversityIcons.Edit %>" AlternateText="Edit" ResourceKey="Edit" />
				</asp:HyperLink>
				<asp:HyperLink id="linkFullName" runat="server" CssClass="_fullname" />
				<asp:Label id="labelAcademicDegreeAndTitle" runat="server" />
				<asp:Label id="labelPositions" runat="server" CssClass="_positions" />
				<asp:HyperLink id="linkEmail" runat="server" CssClass="_email email" />
				<asp:HyperLink id="linkSecondaryEmail" runat="server" CssClass="_email email" />
				<asp:HyperLink id="linkWebSite" runat="server" CssClass="_website" Target="_blank" />
				<asp:HyperLink id="linkUserProfile" runat="server" CssClass="more _userprofile" Target="_blank" />
				<asp:Label id="labelPhones" runat="server" CssClass="_phones" />
			</div>
		</div>
	</ItemTemplate>
	<ItemStyle CssClass="u8y-employee-list-item" />
	<AlternatingItemStyle CssClass="u8y-employee-list-item" />
</asp:DataList>
<controls:AgplSignature runat="server" />
