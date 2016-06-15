<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ViewLaunchpad.ascx.cs" Inherits="R7.University.Launchpad.ViewLaunchpad" %>

<div class="dnnForm dnnClear university-launchpad">
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
    <asp:Panel runat="server" CssClass="dnnFormItem" DefaultButton="buttonSearch">
        <asp:HyperLink id="linkAddItem" runat="server" CssClass="dnnSecondaryAction link-add-item" resourcekey="AddItem.Text" />
        <asp:TextBox id="textSearch" runat="server" />
        <asp:LinkButton id="buttonSearch" runat="server" CssClass="dnnPrimaryAction" resourcekey="Search.Text" OnClick="buttonSearch_Click" />
        <asp:LinkButton id="buttonResetSearch" runat="server" CssClass="dnnSecondaryAction" resourcekey="ResetSearch.Text" OnClick="buttonResetSearch_Click" />
    </asp:Panel>
	<asp:MultiView id="multiView" runat="server" OnActiveViewChanged="multiView_ActiveViewChanged">
		<asp:View id="viewPositions" runat="server">
			<div id="positions" style="overflow:auto">
					<asp:GridView id="gridPositions" runat="server" AutoGenerateColumns="false" CssClass="dnnGrid"
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
                            <asp:BoundField DataField="PositionID" SortExpression="PositionID" HeaderText="PositionID" />
                            <asp:BoundField DataField="Title" SortExpression="Title" HeaderText="Title" />
                            <asp:BoundField DataField="ShortTitle" SortExpression="ShortTitle" HeaderText="ShortTitle" />
                            <asp:BoundField DataField="Weight" SortExpression="Weight" HeaderText="Weight" />
                            <asp:BoundField DataField="IsTeacher" SortExpression="IsTeacher" HeaderText="IsTeacher" />
						</Columns>
						<PagerSettings Mode="NumericFirstLast" Visible="true" />
			        </asp:GridView>
			        <br />
				</div>
		</asp:View>
		<asp:View id="viewDivisions" runat="server"> 
			<div id="divisions" style="overflow:auto">
				<asp:GridView id="gridDivisions" runat="server" AutoGenerateColumns="false" CssClass="dnnGrid"
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
                        <asp:BoundField DataField="DivisionID" SortExpression="DivisionID" HeaderText="DivisionID" />
                        <asp:BoundField DataField="ParentDivisionID" SortExpression="ParentDivisionID" HeaderText="ParentDivisionID" />
                        <asp:BoundField DataField="Title" SortExpression="Title" HeaderText="Title" />
                        <asp:BoundField DataField="ShortTitle" SortExpression="ShortTitle" HeaderText="ShortTitle" />
                        <asp:BoundField DataField="IsVirtual" SortExpression="IsVirtual" HeaderText="IsVirtual" />
                        <asp:BoundField DataField="DocumentUrl" SortExpression="DocumentUrl" HeaderText="DocumentUrl" />
                        <asp:BoundField DataField="HomePage" SortExpression="HomePage" HeaderText="HomePage" />
                        <asp:BoundField DataField="Location" SortExpression="Location" HeaderText="Location" />
                        <asp:BoundField DataField="Phone" SortExpression="Phone" HeaderText="Phone" />
                        <asp:BoundField DataField="Fax" SortExpression="Fax" HeaderText="Fax" />
                        <asp:BoundField DataField="Email" SortExpression="Email" HeaderText="Email" />
                        <asp:BoundField DataField="SecondaryEmail" SortExpression="SecondaryEmail" HeaderText="SecondaryEmail" />
                        <asp:BoundField DataField="WebSite" SortExpression="WebSite" HeaderText="WebSite" />
                        <asp:BoundField DataField="WorkingHours" SortExpression="WorkingHours" HeaderText="WorkingHours" />
                        <asp:BoundField DataField="StartDate" SortExpression="StartDate" HeaderText="StartDate" />
                        <asp:BoundField DataField="EndDate" SortExpression="EndDate" HeaderText="EndDate" />
                        <asp:BoundField DataField="DivisionTermID" SortExpression="DivisionTermID" HeaderText="DivisionTermID" />
                        <asp:BoundField DataField="HeadPositionID" SortExpression="HeadPositionID" HeaderText="HeadPositionID" />
                        <asp:BoundField DataField="CreatedByUserID" SortExpression="CreatedByUserID" HeaderText="CreatedByUserID" />
                        <asp:BoundField DataField="CreatedOnDate" SortExpression="CreatedOnDate" HeaderText="CreatedOnDate" />
                        <asp:BoundField DataField="LastModifiedByUserID" SortExpression="LastModifiedByUserID" HeaderText="LastModifiedByUserID" />
                        <asp:BoundField DataField="LastModifiedOnDate" SortExpression="LastModifiedOnDate" HeaderText="LastModifiedOnDate" />
					</Columns>
					<PagerSettings Mode="NumericFirstLast" Visible="true" />
		        </asp:GridView>
		        <br />
		    </div>
		</asp:View>
		<asp:View id="viewEmployees" runat="server">
			<div id="employees" style="overflow:auto">
                <asp:GridView id="gridEmployees" runat="server" AutoGenerateColumns="false" CssClass="dnnGrid"
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
                        <asp:BoundField DataField="EmployeeID" SortExpression="EmployeeID" HeaderText="EmployeeID" />
                        <asp:BoundField DataField="LastName" SortExpression="LastName" HeaderText="LastName" />
                        <asp:BoundField DataField="FirstName" SortExpression="FirstName" HeaderText="FirstName" />
                        <asp:BoundField DataField="OtherName" SortExpression="OtherName" HeaderText="OtherName" />
                        <asp:BoundField DataField="UserID" SortExpression="UserID" HeaderText="UserID" />
                        <asp:BoundField DataField="PhotoFileID" SortExpression="PhotoFileID" HeaderText="PhotoFileID" />
                        <asp:BoundField DataField="Phone" SortExpression="Phone" HeaderText="Phone" />
                        <asp:BoundField DataField="CellPhone" SortExpression="CellPhone" HeaderText="CellPhone" />
                        <asp:BoundField DataField="Fax" SortExpression="Fax" HeaderText="Fax" />
                        <asp:BoundField DataField="Email" SortExpression="Email" HeaderText="Email" />
                        <asp:BoundField DataField="SecondaryEmail" SortExpression="SecondaryEmail" HeaderText="SecondaryEmail" />
                        <asp:BoundField DataField="WebSite_String" SortExpression="WebSite_String" HeaderText="WebSite" />
                        <asp:BoundField DataField="Messenger" SortExpression="Messenger" HeaderText="Messenger" />
                        <asp:BoundField DataField="WorkingPlace" SortExpression="WorkingPlace" HeaderText="WorkingPlace" />
                        <asp:BoundField DataField="WorkingHours" SortExpression="WorkingHours" HeaderText="WorkingHours" />
                        <asp:BoundField DataField="Biography_String" SortExpression="Biography_String" HeaderText="Biography" />
                        <asp:BoundField DataField="ExperienceYears" SortExpression="ExperienceYears" HeaderText="ExperienceYears" />
                        <asp:BoundField DataField="ExperienceYearsBySpec" SortExpression="ExperienceYearsBySpec" HeaderText="ExperienceYearsBySpec" />
                        <asp:BoundField DataField="StartDate" SortExpression="StartDate" HeaderText="StartDate" />
                        <asp:BoundField DataField="EndDate" SortExpression="EndDate" HeaderText="EndDate" />
                        <asp:BoundField DataField="CreatedByUserID" SortExpression="CreatedByUserID" HeaderText="CreatedByUserID" />
                        <asp:BoundField DataField="CreatedOnDate" SortExpression="CreatedOnDate" HeaderText="CreatedOnDate" />
                        <asp:BoundField DataField="LastModifiedByUserID" SortExpression="LastModifiedByUserID" HeaderText="LastModifiedByUserID" />
                        <asp:BoundField DataField="LastModifiedOnDate" SortExpression="LastModifiedOnDate" HeaderText="LastModifiedOnDate" />
					</Columns>
					<PagerSettings Mode="NumericFirstLast" Visible="true" />
		        </asp:GridView>
		        <br />
		    </div>
		</asp:View>
		<asp:View id="viewAchievements" runat="server">
			<div id="achievements" style="overflow:auto">
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
                        <asp:BoundField DataField="ParentEduLevelID" SortExpression="ParentEduLevelID" HeaderText="ParentEduLevelID" />
                        <asp:BoundField DataField="Title" SortExpression="Title" HeaderText="Title" ConvertEmptyStringToNull="false" />
                        <asp:BoundField DataField="ShortTitle" SortExpression="ShortTitle" HeaderText="ShortTitle" ConvertEmptyStringToNull="false" />
                        <asp:BoundField DataField="SortIndex" SortExpression="SortIndex" HeaderText="SortIndex" />
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" Visible="true" />
                </asp:GridView>
                <br />
            </div>
        </asp:View>
        <asp:View id="viewEduPrograms" runat="server">
            <div id="eduprograms" style="overflow:auto">
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
                        <asp:BoundField DataField="Generation" SortExpression="Generation" HeaderText="Generation" ConvertEmptyStringToNull="false" />
                        <asp:BoundField DataField="StartDate" SortExpression="StartDate" HeaderText="StartDate" ConvertEmptyStringToNull="false" />
                        <asp:BoundField DataField="EndDate" SortExpression="EndDate" HeaderText="EndDate" ConvertEmptyStringToNull="false" />
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" Visible="true" />
                </asp:GridView>
                <br />
            </div>
        </asp:View>
        <asp:View id="viewEduProgramProfiles" runat="server">
            <div id="eduprogramprofiles" style="overflow:auto">
                <asp:GridView id="gridEduProgramProfiles" runat="server" AutoGenerateColumns="false" CssClass="dnnGrid"
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
                        <asp:BoundField DataField="EduProgramProfileID" SortExpression="EduProgramProfileID" HeaderText="EduProgramProfileID" />
                        <asp:BoundField DataField="EduProgramID" SortExpression="EduProgramID" HeaderText="EduProgramID" />
                        <asp:BoundField DataField="EduLevelId" SortExpression="EduProgramId" HeaderText="EduLevelId" />
                        <asp:BoundField DataField="Code" SortExpression="Code" HeaderText="Code" ConvertEmptyStringToNull="false" />
                        <asp:BoundField DataField="Title" SortExpression="Title" HeaderText="Title" ConvertEmptyStringToNull="false" />
                        <asp:BoundField DataField="ProfileCode" SortExpression="ProfileCode" HeaderText="ProfileCode" ConvertEmptyStringToNull="false" />
                        <asp:BoundField DataField="ProfileTitle" SortExpression="ProfileTitle" HeaderText="ProfileTitle" ConvertEmptyStringToNull="false" />
                        <asp:BoundField DataField="Languages" SortExpression="Languages" HeaderText="Languages" />
                        <asp:BoundField DataField="StartDate" SortExpression="StartDate" HeaderText="StartDate" ConvertEmptyStringToNull="false" />
                        <asp:BoundField DataField="EndDate" SortExpression="EndDate" HeaderText="EndDate" ConvertEmptyStringToNull="false" />
                        <asp:BoundField DataField="AccreditedToDate" SortExpression="AccreditedToDate" HeaderText="AccreditedToDate" />
                        <asp:BoundField DataField="CommunityAccreditedToDate" SortExpression="CommunityAccreditedToDate" HeaderText="CommunityAccreditedToDate" />
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" Visible="true" />
                </asp:GridView>
                <br />
            </div>
        </asp:View>
        <asp:View id="viewDocumentTypes" runat="server">
            <div id="documenttypes" style="overflow:auto">
                <asp:GridView id="gridDocumentTypes" runat="server" AutoGenerateColumns="false" CssClass="dnnGrid"
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
                        <asp:BoundField DataField="DocumentTypeID" SortExpression="DocumentTypeID" HeaderText="DocumentTypeID" />
                        <asp:BoundField DataField="Type" SortExpression="Type" HeaderText="Type" />
                        <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description" ConvertEmptyStringToNull="false" />
                        <asp:BoundField DataField="IsSystem" SortExpression="IsSystem" HeaderText="IsSystem" />
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" Visible="true" />
                </asp:GridView>
                <br />
            </div>
        </asp:View>
        <asp:View id="viewDocuments" runat="server">
            <div id="documents" style="overflow:auto">
                <asp:GridView id="gridDocuments" runat="server" AutoGenerateColumns="false" CssClass="dnnGrid"
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
                        <asp:BoundField DataField="DocumentID" SortExpression="DocumentID" HeaderText="DocumentID" />
                        <asp:BoundField DataField="DocumentTypeID" SortExpression="DocumentTypeID" HeaderText="DocumentTypeID" />
                        <asp:BoundField DataField="ItemID" SortExpression="ItemID" HeaderText="ItemID" />
                        <asp:BoundField DataField="Title" SortExpression="Title" HeaderText="Title" />
                        <asp:BoundField DataField="Url" SortExpression="Url" HeaderText="Url" />
                        <asp:BoundField DataField="SortIndex" SortExpression="SortIndex" HeaderText="SortIndex" />
                        <asp:BoundField DataField="StartDate" SortExpression="StartDate" HeaderText="StartDate" />
                        <asp:BoundField DataField="EndDate" SortExpression="EndDate" HeaderText="EndDate" />
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" Visible="true" />
                </asp:GridView>
                <br />
            </div>
        </asp:View>
        <asp:View id="viewEduForms" runat="server">
            <div id="eduforms" style="overflow:auto">
                <asp:GridView id="gridEduForms" runat="server" AutoGenerateColumns="false" CssClass="dnnGrid"
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
                        <asp:BoundField DataField="EduFormID" SortExpression="EduFormID" HeaderText="EduFormID" />
                        <asp:BoundField DataField="Title" SortExpression="Title" HeaderText="Title" />
                        <asp:BoundField DataField="ShortTitle" SortExpression="ShortTitle" HeaderText="ShortTitle" />
                        <asp:BoundField DataField="IsSystem" SortExpression="IsSystem" HeaderText="IsSystem" />
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" Visible="true" />
                </asp:GridView>
                <br />
            </div>
        </asp:View>
	</asp:MultiView>
</div>