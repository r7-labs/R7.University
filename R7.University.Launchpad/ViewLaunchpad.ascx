<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewLaunchpad.ascx.cs" Inherits="R7.University.Launchpad.ViewLaunchpad" %>
<%-- <asp:DataList ID="lstContent" DataKeyField="LaunchpadID" runat="server" CssClass="Launchpad_ContentList" OnItemDataBound="lstContent_ItemDataBound">
	<ItemTemplate>
		<asp:HyperLink ID="linkEdit" runat="server">
			<asp:Image ID="imageEdit" runat="server" ImageUrl="~/images/edit.gif" AlternateText="Edit" ResourceKey="Edit" />
		</asp:HyperLink>
		<asp:Label ID="lblUserName" runat="server" CssClass="Launchpad_UserName" />
		<asp:Label ID="lblCreatedOnDate" runat="server" CssClass="Launchpad_CreatedOnDate" /> 
		<asp:Label ID="lblContent" runat="server" CssClass="Launchpad_Content" />
	</ItemTemplate>
	<ItemStyle CssClass="Launchpad_ContentListItem" />
</asp:DataList>
<!-- Label for debug info -->
<asp:Label ID="lblDebug" runat="server" /> --%>

<%-- <dnn:DnnJsInclude ID="DnnJsInclude" runat="server" FilePath="~/DesktopModules/R7.University.Launchpad/js/module.js" /> --%>
	<%--
<script type="text/javascript">
$(function() {
    $( "#tabs" ).tabs();
});
</script>

<div id="tabs" class="dnnForm dnnClear">
<ul class="dnnAdminTabNav dnnClear">
    <li><a href="#positions">Positions</a></li>
    <li><a href="#divisions">Divisions</a></li>
    <li><a href="#employees">Employees</a></li>
</ul>
 --%>

<div class="dnnForm">
	<ul class="dnnAdminTabNav dnnClear">
		<li id="liPositions" runat="server" class="ui-tabs-active"><asp:LinkButton id="linkPositions" runat="server" OnClick="linkTab_Clicked">Positions</asp:LinkButton></li>
		<li id="liDivisions" runat="server"><asp:LinkButton id="linkDivisions" runat="server" OnClick="linkTab_Clicked">Divisions</asp:LinkButton></li>
		<li id="liEmployees" runat="server"><asp:LinkButton id="linkEmployees" runat="server" OnClick="linkTab_Clicked">Employees</asp:LinkButton></li>
	</ul>

	<asp:MultiView id="multiView" runat="server" ActiveViewIndex="0">
		<asp:View runat="server">
			<div id="positions" style="overflow:auto">

					<asp:HyperLink runat="server" id="buttonAddPosition" CssClass="dnnPrimaryAction">Add position</asp:HyperLink><br />
					<asp:GridView id="gridPositions" runat="server" AutoGenerateColumns="true"
					PageSize="15" AllowPaging="true" AllowSorting="true" GridLines="None"
					OnSorting="gridView_Sorting" 
					OnPageIndexChanging="gridView_PageIndexChanging"
					OnRowDataBound="gridView_RowDataBound" CssClass="dnnGrid">
						<HeaderStyle CssClass="dnnGridHeader" horizontalalign="Left" />
				        <RowStyle CssClass="dnnGridItem" horizontalalign="Left" />
				        <AlternatingRowStyle CssClass="dnnGridAltItem" />
				        <EditRowStyle CssClass="dnnFormInput" />
				        <SelectedRowStyle CssClass="dnnFormError" />
				        <FooterStyle CssClass="dnnGridFooter" />
				        <PagerStyle CssClass="dnnGridPager" />
						<Columns>
							<asp:TemplateField>
								<ItemTemplate>
					                	<asp:HyperLink id="linkEdit" runat="server">
					                		<asp:Image runat="server" ImageUrl="~/images/edit.gif" />
					                	</asp:HyperLink>
					            </ItemTemplate>
					        </asp:TemplateField>
						</Columns>
						<PagerSettings Mode="Numeric" Visible="true" />
			        </asp:GridView>
			        <br />
				</div>
		</asp:View>
		<asp:View runat="server"> 
			<div id="divisions" style="overflow:auto">
				<asp:HyperLink runat="server" id="buttonAddDivision" CssClass="dnnPrimaryAction">Add division</asp:HyperLink><br />
				<asp:GridView id="gridDivisions" runat="server" AutoGenerateColumns="true" 
				PageSize="15" AllowPaging="true" AllowSorting="true" GridLines="None"
				OnSorting="gridView_Sorting" 
				OnPageIndexChanging="gridView_PageIndexChanging"
				OnRowDataBound="gridView_RowDataBound">
					<HeaderStyle CssClass="dnnGridHeader" horizontalalign="Left" />
			        <RowStyle CssClass="dnnGridItem" horizontalalign="Left" />
			        <AlternatingRowStyle CssClass="dnnGridAltItem" />
			        <SelectedRowStyle CssClass="dnnFormError" />
			        <EditRowStyle CssClass="dnnFormInput" />
			        <FooterStyle CssClass="dnnGridFooter" />
			        <PagerStyle CssClass="dnnGridPager" />
					<Columns>
						<asp:TemplateField>
			               <ItemTemplate>
			                	<asp:HyperLink id="linkEdit" runat="server">
			                		<asp:Image runat="server" ImageUrl="~/images/edit.gif" />
			                	</asp:HyperLink>
			               </ItemTemplate>
			        	</asp:TemplateField>
					</Columns>
					<PagerSettings Mode="Numeric" Visible="true" />
		        </asp:GridView>
		        <br />
		    </div>
		</asp:View>
		<asp:View runat="server">
			<div id="employees" style="overflow:auto">
				<asp:HyperLink runat="server" id="buttonAddEmployee" CssClass="dnnPrimaryAction">Add employee</asp:HyperLink><br />
				<asp:GridView id="gridEmployees" runat="server" AutoGenerateColumns="true" 
				PageSize="15" AllowPaging="true" AllowSorting="true" GridLines="None" 
				OnSorting="gridView_Sorting" 
				OnPageIndexChanging="gridView_PageIndexChanging"
				OnRowDataBound="gridView_RowDataBound">
					<HeaderStyle CssClass="dnnGridHeader" horizontalalign="Left" />
			        <RowStyle CssClass="dnnGridItem" horizontalalign="Left" />
			        <AlternatingRowStyle CssClass="dnnGridAltItem" />
			        <SelectedRowStyle CssClass="dnnFormError" />
			        <EditRowStyle CssClass="dnnFormInput" />
			        <FooterStyle CssClass="dnnGridFooter" />
			        <PagerStyle CssClass="dnnGridPager" />
			        <Columns>
						<asp:TemplateField>
			               <ItemTemplate>
			                	<asp:HyperLink id="linkEdit" runat="server">
			                		<asp:Image runat="server" ImageUrl="~/images/edit.gif" />
			                	</asp:HyperLink>
			               </ItemTemplate>
			        	</asp:TemplateField>
					</Columns>
					<PagerSettings Mode="Numeric" Visible="true" />
		        </asp:GridView>
		        <br />
		    </div>
		</asp:View>
	</asp:MultiView>
</div>