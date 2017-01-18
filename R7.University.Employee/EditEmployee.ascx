<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditEmployee.ascx.cs" Inherits="R7.University.Employee.EditEmployee" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Audit" Src="~/controls/ModuleAuditControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Url" Src="~/controls/DnnUrlControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Picker" Src="~/controls/filepickeruploader.ascx" %> 
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="AgplSignature" Src="~/DesktopModules/R7.University/R7.University/Controls/AgplSignature.ascx" %>
<%@ Register TagPrefix="controls" TagName="DivisionSelector" Src="~/DesktopModules/R7.University/R7.University/Controls/DivisionSelector.ascx" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University.Employee/admin.css" Priority="200" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/admin.css" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/dnn-ac-combobox.css" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/module.css" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/js/dnn-ac-combobox.js" />

<div class="dnnForm dnnClear">
	<div id="employee-tabs">
		<ul class="dnnAdminTabNav dnnClear">
		    <li><a href="#employee-common-tab"><%= LocalizeString("Common.Tab") %></a></li>
		    <li><a href="#employee-positions-tab"><%= LocalizeString("Positions.Tab") %></a></li>
		    <li><a href="#employee-achievements-tab"><%= LocalizeString("Achievements.Tab") %></a></li>
            <li><a href="#employee-disciplines-tab"><%= LocalizeString("Disciplines.Tab") %></a></li>
		    <li><a href="#employee-about-tab"><%= LocalizeString("About.Tab") %></a></li>
		</ul>
		<asp:ValidationSummary runat="server" CssClass="dnnFormMessage dnnFormError" />
		<div id="employee-common-tab">
			<fieldset>
                <div class="dnnFormItem dnnFormRequired">
                    <dnn:Label id="labelLastName" runat="server" ControlName="textLastName" />
                    <asp:TextBox id="textLastName" runat="server" MaxLength="50" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="textLastName" Display="Dynamic"
                        CssClass="dnnFormMessage dnnFormError" resourcekey="LastName.Required" />
                </div>
                <div class="dnnFormItem dnnFormRequired">
                    <dnn:Label id="labelFirstName" runat="server" ControlName="textFirstName" />
                    <asp:TextBox id="textFirstName" runat="server" MaxLength="50" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="textFirstName" Display="Dynamic"
                        CssClass="dnnFormMessage dnnFormError" resourcekey="FirstName.Required" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelOtherName" runat="server" ControlName="textOtherName" />
                    <asp:TextBox id="textOtherName" runat="server" MaxLength="50" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelPhotoLookup" runat="server" ControlName="buttonPhotoLookup" />
                    <asp:LinkButton id="buttonPhotoLookup" runat="server" resourcekey="buttonPhotoLookup"
                        CssClass="dnnSecondaryAction" OnClick="buttonPhotoLookup_Click" />
                </div>
            	<div class="dnnFormItem">
					<dnn:Label id="labelPhoto" runat="server" ControlName="pickerPhoto" />
                    <dnn:Picker id="pickerPhoto" runat="server" Required="true" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelUser" runat="server" ControlName="comboUsers" />
					<asp:DropDownList id="comboUsers" runat="server" />
				</div>
                <div class="dnnFormItem">
					<dnn:Label id="labelUserLookup" runat="server" ControlName="textUserLookup" />
                    <div style="float:left;width:45%;margin-bottom:1em">
                        <asp:TextBox id="textUserLookup" runat="server" CssClass="dnn-form-control" Style="display:block;width:100%" />
                        <asp:CheckBox id="checkIncludeDeletedUsers" runat="server" resourcekey="checkIncludeDeletedUsers" />
                        <asp:LinkButton id="buttonUserLookup" runat="server" resourcekey="buttonUserLookup" 
                            CssClass="dnnSecondaryAction" Style="margin-left:1em" OnClick="buttonUserLookup_Click" CausesValidation="false" />
                    </div>
				</div>
                <div class="dnnFormItem">
					<dnn:Label id="labelPhone" runat="server" ControlName="textPhone" />
					<asp:TextBox id="textPhone" runat="server" MaxLength="64" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelCellPhone" runat="server" ControlName="textCellPhone" />
					<asp:TextBox id="textCellPhone" runat="server" MaxLength="64"/>
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelFax" runat="server" ControlName="textFax" />
					<asp:TextBox id="textFax" runat="server" MaxLength="30" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelEmail" runat="server" ControlName="textEmail" />
					<asp:TextBox id="textEmail" runat="server" MaxLength="250" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelSecondaryEmail" runat="server" ControlName="textSecondaryEmail" />
					<asp:TextBox id="textSecondaryEmail" runat="server" MaxLength="250" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelWebSite" runat="server" ControlName="textWebSite" />
					<asp:TextBox id="textWebSite" runat="server" MaxLength="250" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelWebSiteLabel" runat="server" ControlName="textWebSiteLabel" />
					<asp:TextBox id="textWebSiteLabel" runat="server" MaxLength="64" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelMessenger" runat="server" ControlName="textMessenger" />
					<asp:TextBox id="textMessenger" runat="server" MaxLength="250" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelWorkingPlace" runat="server" ControlName="textWorkingPlace" />
					<asp:TextBox id="textWorkingPlace" runat="server" MaxLength="50" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelWorkingHours" runat="server" ControlName="textWorkingHours" />
					<asp:DropDownList id="comboWorkingHours" runat="server"
								DataTextField="Name"
								DataValueField="TermId"
						 />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelCustomWorkingHours" runat="server" ControlName="textWorkingHours" />
					<asp:TextBox id="textWorkingHours" runat="server" MaxLength="100" Style="margin-bottom:0" />
				</div>
                <div class="dnnFormItem">
                    <div class="dnnLabel"></div>
                    <asp:CheckBox id="checkAddToVocabulary" runat="server" resourcekey="checkAddToVocabulary" />
				</div>
                <div class="dnnFormItem">
					<dnn:Label id="labelExperienceYears" runat="server" ControlName="textExperienceYears" />
					<asp:TextBox id="textExperienceYears" runat="server" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelExperienceYearsBySpec" runat="server" ControlName="textExperienceYearsBySpec" />
					<asp:TextBox id="textExperienceYearsBySpec" runat="server" />
				</div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelShowBarcode" runat="server" ControlName="checkShowBarcode" />
                    <asp:CheckBox id="checkShowBarcode" runat="server" CssClass="dnn-form-control" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelStartDate" runat="server" ControlName="datetimeStartDate" />
                    <dnn:DnnDateTimePicker id="datetimeStartDate" runat="server" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelEndDate" runat="server" ControlName="datetimeEndDate" />
                    <dnn:DnnDateTimePicker id="datetimeEndDate" runat="server" />
                </div>
			</fieldset>
		</div>
		<div id="employee-positions-tab">
			<fieldset>
				<div class="dnnFormItem">
					<asp:GridView id="gridOccupiedPositions" runat="server" AutoGenerateColumns="false" CssClass="dnnGrid"
						GridLines="None" OnRowDataBound="gridOccupiedPositions_RowDataBound" Style="width:100%;margin-bottom:30px">
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
										<span style="white-space:nowrap">
											<asp:LinkButton id="linkEdit" runat="server" OnCommand="linkEditOccupiedPosition_Command" >
					                			<asp:Image runat="server" ImageUrl="<%# EditIconUrl %>" />
					                		</asp:LinkButton>
					                		<asp:LinkButton id="linkDelete" runat="server" OnCommand="linkDeleteOccupiedPosition_Command" >
					                			<asp:Image runat="server" ImageUrl="<%# DeleteIconUrl %>" />
					                		</asp:LinkButton>
										</span>
					               </ItemTemplate>
					        	</asp:TemplateField>
                                <asp:BoundField DataField="ItemID" />
                                <asp:BoundField DataField="DivisionShortTitle" HeaderText="Division" />
                                <asp:BoundField DataField="PositionShortTitleWithSuffix" HeaderText="Position" />
                                <asp:CheckBoxField DataField="IsPrime" HeaderText="IsPrime" />
							</Columns>
				        </asp:GridView>
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelDivisions" runat="server" ControlName="divisionSelector" />
					<controls:DivisionSelector id="divisionSelector" runat="server" />
				</div>
				<div class="dnnFormItem">
    				<dnn:Label id="labelPositions" runat="server" ControlName="comboPositions" />
                    <asp:DropDownList id="comboPositions" runat="server" CssClass="dnn-ac-combobox"
                        DataValueField="PositionID"
                        DataTextField="Title" />
                </div>
				<div class="dnnFormItem">
					<dnn:Label id="labelPositionTitleSuffix" runat="server" ControlName="textPositionTitleSuffix" />
					<asp:TextBox id="textPositionTitleSuffix" runat="server" MaxLength="100" />
				</div>
				<div class="dnnFormItem" style="margin-bottom:10px">
					<dnn:Label id="labelIsPrime" runat="server" ControlName="checkIsPrime" />
					<asp:CheckBox id="checkIsPrime" runat="server" />
				</div>
				<div class="dnnFormItem">
					<div class="dnnLabel"></div>
					<asp:LinkButton id="buttonAddPosition" runat="server" resourcekey="buttonAddPosition" 
						CssClass="dnnPrimaryAction" OnCommand="buttonAddPosition_Command"  CommandArgument="Add" />
					<asp:LinkButton id="buttonUpdatePosition" runat="server" resourcekey="buttonUpdatePosition" 
						CssClass="dnnPrimaryAction" OnCommand="buttonAddPosition_Command" Visible="false" CommandArgument="Update" />
					<asp:LinkButton id="buttonCancelEditPosition" runat="server" resourcekey="buttonCancelEditPosition" 
								CssClass="dnnSecondaryAction" OnClick="buttonCancelEditPosition_Click" />
				</div>
				<asp:HiddenField id="hiddenOccupiedPositionItemID" runat="server" />
			</fieldset>
		</div>
		<div id="employee-achievements-tab">
			<fieldset>
				<div class="dnnFormItem">
					<asp:GridView id="gridAchievements" runat="server" AutoGenerateColumns="false" CssClass="dnnGrid"
						GridLines="None" OnRowDataBound="gridAchievements_RowDataBound" Style="width:100%;margin-bottom:30px">
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
										<span style="white-space:nowrap">
					                		<asp:LinkButton id="linkEdit" runat="server" OnCommand="linkEditAchievement_Command" >
					                			<asp:Image runat="server" ImageUrl="<%# EditIconUrl %>" />
					                		</asp:LinkButton>
											<asp:LinkButton id="linkDelete" runat="server" OnCommand="linkDeleteAchievement_Command" >
					                			<asp:Image runat="server" ImageUrl="<%# DeleteIconUrl %>" />
					                		</asp:LinkButton>
										</span>
					               </ItemTemplate>
					        	</asp:TemplateField>
                                <asp:BoundField DataField="ItemID" />
                                <asp:BoundField DataField="ViewYears" HeaderText="Years" />
                                <asp:BoundField DataField="ViewTitle" HeaderText="Title" />
                                <asp:BoundField DataField="ViewAchievementType" HeaderText="AchievementType" />
                                <asp:CheckBoxField DataField="IsTitle" HeaderText="IsTitle" />
                                <asp:BoundField DataField="Description" HeaderText="AchievementType" />
                                <asp:BoundField DataField="DocumentUrl" HeaderText="DocumentUrl" />
							</Columns>
				    </asp:GridView>
				</div>
      			<div class="dnnFormItem">
					<dnn:Label id="labelAchievements" runat="server" ControlName="comboAchievements" />
                    <asp:DropDownList id="comboAchievement" runat="server" CssClass="dnn-ac-combobox"
                        DataValueField="AchievementID"
                        DataTextField="Title"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="comboAchievement_SelectedIndexChanged" />
                </div>
                <asp:Panel id="panelAchievementTypes" runat="server" class="dnnFormItem">
                    <dnn:Label id="labelAchievementTypes" runat="server" ControlName="comboAchievementTypes" />
                    <asp:DropDownList id="comboAchievementTypes" runat="server" 
                        DataTextField="LocalizedAchivementType"
                        DataValueField="AchievementType" />
                </asp:Panel>
				<asp:Panel id="panelAchievementTitle" runat="server" class="dnnFormItem">
					<dnn:Label id="labelAchievementTitle" runat="server" ControlName="textAchievementTitle" />
					<asp:TextBox id="textAchievementTitle" runat="server" TextMode="MultiLine" Rows="3" />
                    <asp:RegularExpressionValidator runat="server"
                        CssClass="dnnFormMessage dnnFormError" resourcekey="AchievementTitle.MaxLength"
                        ControlToValidate="textAchievementTitle" Display="Dynamic"
                        ValidationExpression="[\s\S]{0,250}" ValidationGroup="Achievements">
                    </asp:RegularExpressionValidator>
				</asp:Panel>
                <div class="dnnFormItem">
					<dnn:Label id="labelAchievementDescription" runat="server" ControlName="textAchievementDescription" />
					<asp:TextBox id="textAchievementDescription" runat="server" TextMode="MultiLine" Rows="3" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelYears" runat="server" ControlName="textYearBegin" />
					<div class="dnn-form-control-group">
                        <asp:TextBox id="textYearBegin" runat="server" CssClass="dnn-form-control-half-width" />
                        &ndash;
                        <asp:TextBox id="textYearEnd" runat="server" CssClass="dnn-form-control-half-width" />
                    </div>
				</div>
                <div class="dnnFormItem">
					<dnn:Label id="labelIsTitle" runat="server" ControlName="checkIsTitle" />
					<asp:CheckBox id="checkIsTitle" runat="server" CssClass="dnn-form-control" />
				</div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelDocumentURL" runat="server" ControlName="urlDocumentURL" />
                    <dnn:Url id="urlDocumentURL" runat="server" UrlType="N" 
                        IncludeActiveTab="true"
                        ShowFiles="true" ShowTabs="true"
                        ShowUrls="true" ShowUsers="true"
                        ShowLog="false" ShowTrack="false"
                        ShowNone="true" ShowNewWindow="false" />      
                </div>
                <h2 class="dnnFormSectionHead dnnClear"><a href="#"><%: LocalizeString ("sectionAdvancedAchievementProperties.Text") %></a></h2>
                <fieldset>
                    <asp:Panel id="panelAchievementShortTitle" runat="server" class="dnnFormItem">
                        <dnn:Label id="labelAchievementShortTitle" runat="server" ControlName="textAchievementShortTitle" />
                        <asp:TextBox id="textAchievementShortTitle" runat="server" MaxLength="64" />
                    </asp:Panel>
                    <div class="dnnFormItem">
                        <dnn:Label id="labelAchievementTitleSuffix" runat="server" ControlName="textAchievementTitleSuffix" />
                        <asp:TextBox id="textAchievementTitleSuffix" runat="server" MaxLength="100" />
                    </div>
                </fieldset>
				<div class="dnnFormItem">
					<div class="dnnLabel"></div>
					<asp:LinkButton id="buttonAddAchievement" runat="server" resourcekey="buttonAddAchievement" 
						CssClass="dnnPrimaryAction" OnCommand="buttonAddAchievement_Command" CommandArgument="Add" 
                        CausesValidation="true" ValidationGroup="Achievements" />
					<asp:LinkButton id="buttonUpdateAchievement" runat="server" resourcekey="buttonUpdateAchievement" 
						CssClass="dnnPrimaryAction" OnCommand="buttonAddAchievement_Command" Visible="false"
                        CommandArgument="Update" CausesValidation="true" ValidationGroup="Achievements" />
					<asp:LinkButton id="buttonCancelEditAchievement" runat="server" resourcekey="buttonCancelEditAchievement" 
						CssClass="dnnSecondaryAction" OnClick="buttonCancelEditAchievement_Click" />
				</div>
				<asp:HiddenField id="hiddenAchievementItemID" runat="server" />
			</fieldset>
		</div>
		<div id="employee-disciplines-tab">
			<fieldset>
                <asp:ValidationSummary runat="server" ValidationGroup="Disciplines" 
                    DisplayMode="SingleParagraph" CssClass="dnnFormMessage dnnFormWarning" />
                <div class="dnnFormItem">
                    <asp:GridView id="gridDisciplines" runat="server" AutoGenerateColumns="false" CssClass="dnnGrid"
                            GridLines="None" OnRowDataBound="gridDisciplines_RowDataBound" Style="width:100%;margin-bottom:30px">
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
                                    <span style="white-space:nowrap">
                                        <asp:LinkButton id="linkEdit" runat="server" OnCommand="linkEditDisciplines_Command" >
                                            <asp:Image runat="server" ImageUrl="<%# EditIconUrl %>" />
                                        </asp:LinkButton>
                                        <asp:LinkButton id="linkDelete" runat="server" OnCommand="linkDeleteDisciplines_Command" >
                                            <asp:Image runat="server" ImageUrl="<%# DeleteIconUrl %>" />
                                        </asp:LinkButton>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ItemID" />
                            <asp:BoundField DataField="EduProgramProfile_String" HeaderText="EduProgramProfile" />
							<asp:BoundField DataField="EduLevel_String" HeaderText="EduLevel" />
                            <asp:BoundField DataField="Disciplines" HeaderText="Disciplines" />
                        </Columns>
                    </asp:GridView>
                    <asp:HiddenField id="hiddenDisciplinesItemID" runat="server" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelEduLevel" runat="server" ControlName="comboEduLevel" />
                    <asp:DropDownList id="comboEduLevel" runat="server"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="comboEduLevel_SelectedIndexChanged"
                        DataValueField="EduLevelID"
                        DataTextField="Title" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelEduProgramProfile" runat="server" ControlName="comboEduProgramProfile" />
                    <asp:DropDownList id="comboEduProgramProfile" runat="server" CssClass="dnn-ac-combobox"
                        DataValueField="EduProgramProfileID"
                        DataTextField="Title_String" />
                    <asp:CustomValidator id="valEduProgramProfile" runat="server" ControlToValidate="comboEduProgramProfile"
                        Display="None" EnableClientScript="false" ValidationGroup="Disciplines" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelDisciplines" runat="server" ControlName="textDisciplines" />
                    <asp:TextBox id="textDisciplines" runat="server" TextMode="MultiLine" Rows="7" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="textDisciplines" Display="Dynamic"
                        CssClass="dnnFormMessage dnnFormError" ValidationGroup="Disciplines" resourcekey="Disciplines.Required" />
                </div>
                <div class="dnnFormItem">
                    <div class="dnnLabel"></div>
                    <asp:LinkButton id="buttonAddDisciplines" runat="server" resourcekey="buttonAddDisciplines" 
                        CssClass="dnnPrimaryAction" OnCommand="buttonAddDisciplines_Command" CommandArgument="Add" 
                        CausesValidation="true" ValidationGroup="Disciplines" />
                    <asp:LinkButton id="buttonUpdateDisciplines" runat="server" resourcekey="buttonUpdateDisciplines" 
                        CssClass="dnnPrimaryAction" OnCommand="buttonAddDisciplines_Command" Visible="false" CommandArgument="Update" 
                        CausesValidation="true" ValidationGroup="Disciplines" />
                    <asp:LinkButton id="buttonCancelEditDisciplines" runat="server" resourcekey="buttonCancelEditDisciplines" 
                        CssClass="dnnSecondaryAction" OnClick="buttonCancelEditDisciplines_Click" 
                        CausesValidation="false" />
                </div>
            </fieldset>
		</div>
        <div id="employee-about-tab">
			<fieldset>
				<div class="dnnFormItem">
					<div style="margin-right:20px">
						<dnn:TextEditor id="textBiography" runat="server" Width="100%" Height="300px" />
					</div>
				</div>
			</fieldset>
		</div>
	</div>
	<ul class="dnnActions dnnClear">
		<li><asp:LinkButton id="buttonUpdate" runat="server" CssClass="dnnPrimaryAction" ResourceKey="cmdUpdate" CausesValidation="true" OnClick="buttonUpdate_Click" /></li>
		<li><asp:LinkButton id="buttonDelete" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdDelete" OnClick="buttonDelete_Click" /></li>
		<li><asp:HyperLink id="linkCancel" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdCancel" /></li>
	</ul>
    <controls:AgplSignature runat="server" ShowRule="false" />
	<hr />
	<dnn:Audit id="ctlAudit" runat="server" />
</div>
<input id="hiddenSelectedTab" type="hidden" value="<%= (int) SelectedTab %>" />
<script type="text/javascript">
(function($, Sys) {
    function setupModule() {
        var selectedTab = document.getElementById("hiddenSelectedTab").value;
        $("#employee-tabs").dnnTabs({selected: selectedTab});
        $("#employee-achievements-tab").dnnPanels({defaultState: "closed"});
        dnnAcCombobox_Init($);
        $(".dnn-ac-combobox").combobox();
    };
    $(document).ready(function() {
        setupModule();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function() {
            setupModule();
        });
    });
} (jQuery, window.Sys));
</script>