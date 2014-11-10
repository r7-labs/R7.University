<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditEmployee.ascx.cs" Inherits="R7.University.Employee.EditEmployee" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Audit" Src="~/controls/ModuleAuditControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Url" Src="~/controls/URLControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Picker" Src="~/controls/filepickeruploader.ascx" %> 
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>

<script type="text/javascript">
	$(function() { $("#employeeTabs").dnnTabs({selected: <%= (int)SelectedTab %>}); });
</script>

<div class="dnnForm dnnClear">
	<div id="employeeTabs" class="dnnForm dnnClear">
		<ul class="dnnAdminTabNav dnnClear">
		    <li><a href="#employeeCommon">Common</a></li>
		    <li><a href="#employeePositions">Positions</a></li>
		    <li><a href="#employeeAchievements">Achievements</a></li>
		    <li><a href="#employeeDisciplines">Disciplines</a></li>
		    <li><a href="#employeeAbout">About</a></li>
		</ul>
		<asp:ValidationSummary runat="server" CssClass="dnnFormMessage dnnFormError" />
		<div id="employeeCommon">
			<fieldset>
                <div class="dnnFormItem dnnFormRequired">
                    <dnn:Label id="labelLastName" runat="server" ControlName="textLastName" Suffix=":" />
                    <asp:TextBox id="textLastName" runat="server" MaxLength="50" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="textLastName" Display="Dynamic"
                        CssClass="dnnFormMessage dnnFormError" resourcekey="LastName.Required" />
                </div>
                <div class="dnnFormItem dnnFormRequired">
                    <dnn:Label id="labelFirstName" runat="server" ControlName="textFirstName" Suffix=":" />
                    <asp:TextBox id="textFirstName" runat="server" MaxLength="50" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="textFirstName" Display="Dynamic"
                        CssClass="dnnFormMessage dnnFormError" resourcekey="FirstName.Required" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelOtherName" runat="server" ControlName="textOtherName" Suffix=":" />
                    <asp:TextBox id="textOtherName" runat="server" MaxLength="50" />
                </div>
				<div class="dnnFormItem">
					<dnn:Label id="labelPhoto" runat="server" ControlName="pickerPhoto" Suffix=":" />
                    <asp:LinkButton id="buttonPhotoLookup" runat="server" resourcekey="buttonPhotoLookup"
                            CssClass="dnnSecondaryAction" OnClick="buttonPhotoLookup_Click" />
                    <dnn:Picker id="pickerPhoto" runat="server" Required="true" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelUser" runat="server" ControlName="comboUsers" Suffix=":" />
					<dnn:DnnComboBox id="comboUsers" runat="server" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelUserLookup" runat="server" ControlName="textUserLookup" Suffix=":" /> 
                    <asp:TextBox id="textUserLookup" runat="server" Style="margin-bottom:0" />
                    <asp:LinkButton id="buttonUserLookup" runat="server" resourcekey="buttonUserLookup" 
                        CssClass="dnnSecondaryAction" OnClick="buttonUserLookup_Click" Style="margin-bottom:0"  />
				</div>
                <div class="dnnFormItem">
                    <div class="dnnLabel"></div>
                    <asp:CheckBox id="checkIncludeDeletedUsers" runat="server" 
                        resourcekey="checkIncludeDeletedUsers" Style="margin-bottom:1em" />
                </div>
                <div class="dnnFormItem">
					<dnn:Label id="labelAcademicDegree" runat="server" ControlName="textAcademicDegree" Suffix=":" />
					<asp:TextBox id="textAcademicDegree" runat="server" MaxLength="250" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelAcademicTitle" runat="server" ControlName="textAcademicTitle" Suffix=":" />
					<asp:TextBox id="textAcademicTitle" runat="server" MaxLength="250" />
				</div>
				<%-- <div class="dnnFormItem">
					<dnn:Label id="labelNamePrefix" runat="server" ControlName="textNamePrefix" Suffix=":" />
					<asp:TextBox id="textNamePrefix" runat="server" />
				</div> --%>
				<div class="dnnFormItem">
					<dnn:Label id="labelPhone" runat="server" ControlName="textPhone" Suffix=":" />
					<asp:TextBox id="textPhone" runat="server" MaxLength="64" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelCellPhone" runat="server" ControlName="textCellPhone" Suffix=":" />
					<asp:TextBox id="textCellPhone" runat="server" MaxLength="64"/>
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelFax" runat="server" ControlName="textFax" Suffix=":" />
					<asp:TextBox id="textFax" runat="server" MaxLength="30" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelEmail" runat="server" ControlName="textEmail" Suffix=":" />
					<asp:TextBox id="textEmail" runat="server" MaxLength="250" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelSecondaryEmail" runat="server" ControlName="textSecondaryEmail" Suffix=":" />
					<asp:TextBox id="textSecondaryEmail" runat="server" MaxLength="250" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelWebSite" runat="server" ControlName="textWebSite" Suffix=":" />
					<asp:TextBox id="textWebSite" runat="server" MaxLength="250" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelWebSiteLabel" runat="server" ControlName="textWebSiteLabel" Suffix=":" />
					<asp:TextBox id="textWebSiteLabel" runat="server" MaxLength="64" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelMessenger" runat="server" ControlName="textMessenger" Suffix=":" />
					<asp:TextBox id="textMessenger" runat="server" MaxLength="250" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelWorkingPlace" runat="server" ControlName="textWorkingPlace" Suffix=":" />
					<asp:TextBox id="textWorkingPlace" runat="server" MaxLength="50" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelWorkingHours" runat="server" ControlName="textWorkingHours" Suffix=":" />
					<dnn:DnnComboBox id="comboWorkingHours" runat="server"
								DataTextField="Name"
								DataValueField="TermId"
						 />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelCustomWorkingHours" runat="server" ControlName="textWorkingHours" Suffix=":" />
					<asp:TextBox id="textWorkingHours" runat="server" Style="width:300px" MaxLength="100" />
					<asp:CheckBox id="checkAddToVocabulary" runat="server" resourcekey="checkAddToVocabulary" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelExperienceYears" runat="server" ControlName="textExperienceYears" Suffix=":" />
					<asp:TextBox id="textExperienceYears" runat="server" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelExperienceYearsBySpec" runat="server" ControlName="textExperienceYearsBySpec" Suffix=":" />
					<asp:TextBox id="textExperienceYearsBySpec" runat="server" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelIsPublished" runat="server" ControlName="checkIsPublished" Suffix=":" />
					<asp:CheckBox id="checkIsPublished" runat="server" />
				</div>
			</fieldset>
		</div>
	
		<div id="employeePositions">
			<fieldset>
				<div class="dnnFormItem">
					<asp:GridView id="gridOccupiedPositions" runat="server" AutoGenerateColumns="true" 
						GridLines="None" OnRowDataBound="gridOccupiedPositions_RowDataBound" Style="margin-bottom:30px;width:775px">
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
							</Columns>
				        </asp:GridView>
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelDivisions" runat="server" ControlName="treeDivisions" Suffix=":" />
					<dnn:DnnTreeView id="treeDivisions" runat="server" Style="float:left;display:block;margin-bottom:10px;padding:10px;background-color:#EEE"
						DataTextField="ShortTitle"
						DataValueField="DivisionID"
						DataFieldID = "DivisionID"
						DataFieldParentID="ParentDivisionID"
					/>
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelPositions" runat="server" ControlName="comboPositions" Suffix=":" />
					<dnn:DnnComboBox id="comboPositions" runat="server" 
						DataTextField="ShortTitle"
						DataValueField="PositionID"
					/>
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelPositionTitleSuffix" runat="server" ControlName="textPositionTitleSuffix" Suffix=":" />
					<asp:TextBox id="textPositionTitleSuffix" runat="server" MaxLength="100" />
				</div>
				<div class="dnnFormItem" style="margin-bottom:10px">
					<dnn:Label id="labelIsPrime" runat="server" ControlName="checkIsPrime" Suffix="?" />
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
	
		<div id="employeeAchievements">
			<fieldset>
				<div class="dnnFormItem">
					<asp:GridView id="gridAchievements" runat="server" AutoGenerateColumns="true" 
						GridLines="None" OnRowDataBound="gridAchievements_RowDataBound" Style="margin-bottom:30px;width:775px">
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
							</Columns>
				    </asp:GridView>
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelAchievements" runat="server" ControlName="comboAchievements" Suffix=":" />
					<dnn:DnnComboBox id="comboAchievements" runat="server" AutoPostBack="true"
						DataTextField="DisplayShortTitle"
						DataValueField="AchievementID"
						SelectedIndexChanged="comboAchievements_SelectedIndexChanged"
					/>
				</div>
				<asp:Panel id="panelAchievementTitle" runat="server" class="dnnFormItem">
					<dnn:Label id="labelAchievementTitle" runat="server" ControlName="textAchievementTitle" Suffix=":" />
					<asp:TextBox id="textAchievementTitle" runat="server" MaxLength="250" />
				</asp:Panel>
				<asp:Panel id="panelAchievementShortTitle" runat="server" class="dnnFormItem">
					<dnn:Label id="labelAchievementShortTitle" runat="server" ControlName="textAchievementShortTitle" Suffix=":" />
					<asp:TextBox id="textAchievementShortTitle" runat="server" MaxLength="64" />
				</asp:Panel>
				<asp:Panel id="panelAchievementTypes" runat="server" class="dnnFormItem">
					<dnn:Label id="labelAchievementTypes" runat="server" ControlName="comboAchievementTypes" Suffix=":" />
					<dnn:DnnComboBox id="comboAchievementTypes" runat="server" 
						DataTextField="LocalizedAchivementType"
						DataValueField="AchievementType"
					/>
				</asp:Panel>
				<div class="dnnFormItem">
					<dnn:Label id="labelAchievementTitleSuffix" runat="server" ControlName="textAchievementTitleSuffix" Suffix=":" />
					<asp:TextBox id="textAchievementTitleSuffix" runat="server" MaxLength="100" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelAchievementDescription" runat="server" ControlName="textAchievementDescription" Suffix=":" />
					<asp:TextBox id="textAchievementDescription" runat="server" TextMode="MultiLine" Rows="3" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelYearBegin" runat="server" ControlName="textYearBegin" Suffix=":" />
					<asp:TextBox id="textYearBegin" runat="server" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelYearEnd" runat="server" ControlName="textYearEnd" Suffix=":" />
					<asp:TextBox id="textYearEnd" runat="server" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelIsTitle" runat="server" ControlName="checkIsTitle" Suffix="?" />
					<asp:CheckBox id="checkIsTitle" runat="server" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelDocumentURL" runat="server" ControlName="urlDocumentURL" Suffix=":" />
					<dnn:Url id="urlDocumentURL" runat="server" UrlType="N" 
						IncludeActiveTab="true"
			        	ShowFiles="true" ShowTabs="true"
			        	ShowUrls="true" ShowUsers="true"
						ShowLog="false" ShowTrack="false"
						ShowNone="true" ShowNewWindow="false" />      
				</div>
				<div class="dnnFormItem">
					<div class="dnnLabel"></div>
					<asp:LinkButton id="buttonAddAchievement" runat="server" resourcekey="buttonAddAchievement" 
						CssClass="dnnPrimaryAction" OnCommand="buttonAddAchievement_Command" CommandArgument="Add" />
					<asp:LinkButton id="buttonUpdateAchievement" runat="server" resourcekey="buttonUpdateAchievement" 
						CssClass="dnnPrimaryAction" OnCommand="buttonAddAchievement_Command" Visible="false" CommandArgument="Update" />
					<asp:LinkButton id="buttonCancelEditAchievement" runat="server" resourcekey="buttonCancelEditAchievement" 
						CssClass="dnnSecondaryAction" OnClick="buttonCancelEditAchievement_Click" />
				</div>
				<asp:HiddenField id="hiddenAchievementItemID" runat="server" />
			</fieldset>
		</div>

		<div id="employeeDisciplines">
			<fieldset>
				<div class="dnnFormItem">
					<div style="margin-right:20px">
						<dnn:TextEditor id="textDisciplines" runat="server" Width="100%" Height="300px" />
					</div>
				</div>
			</fieldset>
		</div>

		<div id="employeeAbout">
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
		<li><asp:LinkButton id="buttonUpdate" runat="server" CssClass="dnnPrimaryAction" ResourceKey="cmdUpdate" CausesValidation="true" /></li>
		<li><asp:LinkButton id="buttonDelete" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdDelete" /></li>
		<li><asp:HyperLink id="linkCancel" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdCancel" /></li>
	</ul>
	<hr />
	<dnn:Audit id="ctlAudit" runat="server" />
</div>

