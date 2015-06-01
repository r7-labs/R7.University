<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ViewLaunchpad.ascx.cs" Inherits="R7.University.Launchpad.ViewLaunchpad" %>

<div class="dnnForm">
	<asp:Repeater id="repeatTabs" runat="server" OnItemDataBound="repeatTabs_ItemDataBound">
		<HeaderTemplate>
			<ul class="dnnAdminTabNav dnnClear">
		</HeaderTemplate>
		<ItemTemplate>
			<li id="liTab" runat="server"><asp:LinkButton id="linkTab" runat="server" OnClick="linkTab_Clicked" /></li>
		</ItemTemplate>
		<FooterTemplate>
			</ul>
		</FooterTemplate>
	</asp:Repeater>
	<asp:MultiView id="multiView" runat="server" OnActiveViewChanged="multiView_ActiveViewChanged">
		<asp:View id="viewPositions" runat="server">
			<div id="positions" style="overflow:auto">
					<asp:HyperLink runat="server" id="buttonAddPosition" CssClass="dnnPrimaryAction">Add position</asp:HyperLink><br />
					<asp:GridView id="gridPositions" runat="server" AutoGenerateColumns="true" CssClass="dnnGrid"
					AllowPaging="true" AllowSorting="true" GridLines="None"
					OnSorting="gridView_Sorting" 
					OnPageIndexChanging="gridView_PageIndexChanging"
					OnRowDataBound="gridView_RowDataBound">
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
					                		<asp:Image runat="server" ImageUrl="<%# EditIconUrl %>" />
					                	</asp:HyperLink>
					            </ItemTemplate>
					        </asp:TemplateField>
						</Columns>
						<PagerSettings Mode="NumericFirstLast" Visible="true" />
			        </asp:GridView>
			        <br />
				</div>
		</asp:View>
		<asp:View id="viewDivisions" runat="server"> 
			<div id="divisions" style="overflow:auto">
				<asp:HyperLink runat="server" id="buttonAddDivision" CssClass="dnnPrimaryAction">Add division</asp:HyperLink><br />
				<asp:GridView id="gridDivisions" runat="server" AutoGenerateColumns="true" CssClass="dnnGrid"
				AllowPaging="true" AllowSorting="true" GridLines="None"
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
			                		<asp:Image runat="server" ImageUrl="<%# EditIconUrl %>" />
			                	</asp:HyperLink>
			               </ItemTemplate>
			        	</asp:TemplateField>
					</Columns>
					<PagerSettings Mode="NumericFirstLast" Visible="true" />
		        </asp:GridView>
		        <br />
		    </div>
		</asp:View>
		<asp:View id="viewEmployees" runat="server">
			<div id="employees" style="overflow:auto">
                <asp:Panel runat="server" DefaultButton="buttonEmployeeSearch">
                    <asp:HyperLink runat="server" id="buttonAddEmployee" CssClass="dnnPrimaryAction">Add employee</asp:HyperLink>
    				<asp:TextBox id="textEmployeeSearch" runat="server" Style="margin-left:1em" />
                    <asp:Button id="buttonEmployeeSearch" runat="server" CssClass="dnnSecondaryAction" resourcekey="Search.Text" OnClick="buttonEmployeeSearch_Click" />
                    <asp:Button id="buttonEmployeeResetSearch" runat="server" CssClass="dnnSecondaryAction" resourcekey="ResetSearch.Text" OnClick="buttonEmployeeSearch_Click" /><br />
                </asp:Panel>
                <asp:GridView id="gridEmployees" runat="server" AutoGenerateColumns="true" CssClass="dnnGrid"
				AllowPaging="true" AllowSorting="true" GridLines="None" 
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
			                		<asp:Image runat="server" ImageUrl="<%# EditIconUrl %>" />
			                	</asp:HyperLink>
			               </ItemTemplate>
			        	</asp:TemplateField>
					</Columns>
					<PagerSettings Mode="NumericFirstLast" Visible="true" />
		        </asp:GridView>
		        <br />
		    </div>
		</asp:View>
		<asp:View id="viewAchievements" runat="server">
			<div id="achievements" style="overflow:auto">
				<asp:HyperLink runat="server" id="buttonAddAchievement" CssClass="dnnPrimaryAction">Add achievement</asp:HyperLink><br />
				<asp:GridView id="gridAchievements" runat="server" AutoGenerateColumns="true" CssClass="dnnGrid"
				AllowPaging="true" AllowSorting="true" GridLines="None" 
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
			                		<asp:Image runat="server" ImageUrl="<%# EditIconUrl %>" />
			                	</asp:HyperLink>
			               </ItemTemplate>
			        	</asp:TemplateField>
					</Columns>
					<PagerSettings Mode="NumericFirstLast" Visible="true" />
		        </asp:GridView>
		        <br />
		    </div>
		</asp:View>
        <asp:View id="viewEduLevels" runat="server">
            <div id="edulevels" style="overflow:auto">
                <asp:HyperLink runat="server" id="buttonAddEduLevel" CssClass="dnnPrimaryAction">Add Education Level</asp:HyperLink><br />
                <asp:GridView id="gridEduLevels" runat="server" AutoGenerateColumns="false" CssClass="dnnGrid"
                    AllowPaging="true" AllowSorting="true" GridLines="None" 
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
                                    <asp:Image runat="server" ImageUrl="<%# EditIconUrl %>" />
                                </asp:HyperLink>
                           </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="EduLevelID" SortExpression="EduLevelID" HeaderText="EduLevelID" />
                        <asp:BoundField DataField="Title" SortExpression="Title" HeaderText="Title" ConvertEmptyStringToNull="false" />
                        <asp:BoundField DataField="ShortTitle" SortExpression="ShortTitle" HeaderText="ShortTitle" ConvertEmptyStringToNull="false" />
                        <asp:BoundField DataField="EduType" SortExpression="EduType" HeaderText="EduType" ConvertEmptyStringToNull="false" />
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" Visible="true" />
                </asp:GridView>
                <br />
            </div>
        </asp:View>
        <asp:View id="viewEduPrograms" runat="server">
            <div id="eduprograms" style="overflow:auto">
                <asp:HyperLink runat="server" id="buttonAddEduProgram" CssClass="dnnPrimaryAction">Add Educational Program</asp:HyperLink><br />
                <asp:GridView id="gridEduPrograms" runat="server" AutoGenerateColumns="false" CssClass="dnnGrid"
                    AllowPaging="true" AllowSorting="true" GridLines="None" 
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
                                    <asp:Image runat="server" ImageUrl="<%# EditIconUrl %>" />
                                </asp:HyperLink>
                           </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="EduProgramID" SortExpression="EduProgramID" HeaderText="EduProgramID" />
                        <asp:BoundField DataField="EduLevelID" SortExpression="EduLevelID" HeaderText="EduLevelID" />
                        <asp:BoundField DataField="Code" SortExpression="Code" HeaderText="Code" ConvertEmptyStringToNull="false" />
                        <asp:BoundField DataField="Title" SortExpression="Title" HeaderText="Title" ConvertEmptyStringToNull="false" />
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" Visible="true" />
                </asp:GridView>
                <br />
            </div>
        </asp:View>
	</asp:MultiView>
</div>