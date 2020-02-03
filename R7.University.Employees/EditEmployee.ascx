<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditEmployee.ascx.cs" Inherits="R7.University.Employees.EditEmployee" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Audit" Src="~/controls/ModuleAuditControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Url" Src="~/controls/DnnUrlControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Picker" Src="~/controls/filepickeruploader.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="dnn" TagName="JavaScriptLibraryInclude" Src="~/admin/Skins/JavaScriptLibraryInclude.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="EditPositions" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/EditPositions.ascx" %>
<%@ Register TagPrefix="controls" TagName="EditAchievements" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/EditAchievements.ascx" %>
<%@ Register TagPrefix="controls" TagName="EditDisciplines" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/EditDisciplines.ascx" %>
<%@ Register TagPrefix="controls" TagName="AgplSignature" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/AgplSignature.ascx" %>
<%@ Register TagPrefix="controls" TagName="DivisionSelector" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/DivisionSelector.ascx" %>

<dnn:JavaScriptLibraryInclude runat="server" Name="Select2" />
<dnn:DnnCssInclude runat="server" FilePath="~/Resources/Libraries/Select2/04_00_13/css/select2.min.css" />

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University.Employees/admin.css" Priority="200" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/css/admin.css" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/css/module.css" />

<div class="dnnForm dnnClear">
	<div id="employee-tabs">
		<ul class="dnnAdminTabNav dnnClear">
		    <li><a href="#employee-common-tab"><%= LocalizeString("Common.Tab") %></a></li>
			<li><a href="#employee-contacts-tab"><%= LocalizeString("Contacts.Tab") %></a></li>
			<li><a href="#employee-work-experience-tab"><%= LocalizeString("WorkExperience.Tab") %></a></li>
            <li><a href="#employee-positions-tab"><%= LocalizeString("Positions.Tab") %></a></li>
			<li><a href="#employee-achievements-tab"><%= LocalizeString("Achievements.Tab") %></a></li>
			<li><a href="#employee-disciplines-tab"><%= LocalizeString("Disciplines.Tab") %></a></li>
		    <li><a href="#employee-about-tab"><%= LocalizeString("About.Tab") %></a></li>
			<li><a href="#employee-audit-tab"><%= LocalizeString("Audit.Tab") %></a></li>
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
					<dnn:Label id="labelPhoto" runat="server" ControlName="pickerPhoto" />
                    <dnn:Picker id="pickerPhoto" runat="server" Required="false" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelAltPhoto" runat="server" ControlName="pickerAltPhoto" />
                    <dnn:Picker id="pickerAltPhoto" runat="server" Required="false" />
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
        <div id="employee-contacts-tab">
			<fieldset>
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
					<dnn:Label id="lblScienceIndexAuthorId" runat="server" ControlName="txtScienceIndexAuthorId" />
					<asp:TextBox id="txtScienceIndexAuthorId" runat="server" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelWorkingPlace" runat="server" ControlName="textWorkingPlace" />
					<asp:TextBox id="textWorkingPlace" runat="server" MaxLength="50" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelWorkingHours" runat="server" ControlName="textWorkingHours" />
					<asp:DropDownList id="comboWorkingHours" runat="server"
						DataTextField="Name"
						DataValueField="TermId" />
				</div>
				<div class="dnnFormItem">
					<dnn:Label id="labelCustomWorkingHours" runat="server" ControlName="textWorkingHours" />
					<asp:TextBox id="textWorkingHours" runat="server" MaxLength="100" Style="margin-bottom:0" />
				</div>
                <div class="dnnFormItem">
                    <div class="dnnLabel"></div>
                    <asp:CheckBox id="checkAddToVocabulary" runat="server" resourcekey="checkAddToVocabulary" />
				</div>
			</fieldset>
		</div>
		<div id="employee-work-experience-tab">
			<fieldset>
                <div class="dnnFormItem">
                    <dnn:Label id="labelExperienceYears" runat="server" ControlName="textExperienceYears" />
                    <asp:TextBox id="textExperienceYears" runat="server" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelExperienceYearsBySpec" runat="server" ControlName="textExperienceYearsBySpec" />
                    <asp:TextBox id="textExperienceYearsBySpec" runat="server" />
                </div>
            </fieldset>
		</div>
		<div id="employee-positions-tab">
            <controls:EditPositions id="formEditPositions" runat="server" />
        </div>
		<div id="employee-achievements-tab">
            <controls:EditAchievements id="formEditAchievements" runat="server" />
		</div>
		<div id="employee-disciplines-tab">
            <controls:EditDisciplines id="formEditDisciplines" runat="server" />
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
		<div id="employee-audit-tab">
			<fieldset>
				<div class="dnnFormItem">
					<dnn:Label id="labelAudit" runat="server" ControlName="ctlAudit" />
                    <dnn:Audit id="ctlAudit" runat="server" />
				</div>
			</fieldset>
        </div>
	</div>
	<ul class="dnnActions dnnClear">
		<li><asp:LinkButton id="buttonUpdate" runat="server" CssClass="dnnPrimaryAction" ResourceKey="cmdUpdate" CausesValidation="true" /></li>
		<li>&nbsp;</li>
		<li><asp:LinkButton id="buttonDelete" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdDelete" /></li>
		<li>&nbsp;</li>
		<li><asp:HyperLink id="linkCancel" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdCancel" /></li>
	</ul>
    <controls:AgplSignature runat="server" ShowRule="false" />
</div>
<input id="hiddenSelectedTab" type="hidden" value="<%= (int) SelectedTab %>" />
<script type="text/javascript">
(function($, Sys) {
    function setupModule() {
        var selectedTab = document.getElementById("hiddenSelectedTab").value;
        $("#employee-tabs").dnnTabs({selected: selectedTab});
        $("#employee-achievements-tab").dnnPanels({defaultState: "closed"});
		$(".dnn-select2").select2();
    };
    $(document).ready(function() {
        setupModule();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function() {
            setupModule();
        });
    });
} (jQuery, window.Sys));
</script>
