<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditEduProgramProfile.ascx.cs" Inherits="R7.University.EduProgramProfiles.EditEduProgramProfile" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Audit" Src="~/controls/ModuleAuditControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="EditEduFormYears" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/EditEduFormYears.ascx" %>
<%@ Register TagPrefix="controls" TagName="EditDivisions" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/EditDivisions.ascx" %>
<%@ Register TagPrefix="controls" TagName="AgplSignature" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/AgplSignature.ascx" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/assets/css/module.css" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/assets/css/admin.css" Priority="200" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University.EduProgramProfiles/admin.css" />

<div class="dnnForm dnnClear u8y-edit-eduprogramprofile">
    <div id="eduprogramprofile-tabs">
        <ul class="dnnAdminTabNav dnnClear">
            <li><a href="#eduprogramprofile-common-tab"><%= LocalizeString ("Common.Tab") %></a></li>
            <li><a href="#eduprogramprofile-eduformyears-tab"><%= LocalizeString ("EduFormYears.Tab") %></a></li>
			<li><a href="#eduprogramprofile-divisions-tab"><%= LocalizeString ("Divisions.Tab") %></a></li>
            <li><a href="#eduprogramprofile-audit-tab"><%= LocalizeString ("Audit.Tab") %></a></li>
        </ul>
        <div id="eduprogramprofile-common-tab">
        	<fieldset>
                <div class="dnnFormItem">
                    <dnn:Label id="labelEduProgramLevel" runat="server" ControlName="comboEduProgramLevel" />
                    <asp:DropDownList id="comboEduProgramLevel" runat="server"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="comboEduProgramLevel_SelectedIndexChanged"
                        DataValueField="EduLevelID"
                        DataTextField="Title" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelEduProgram" runat="server" ControlName="comboEduProgram" />
                    <asp:DropDownList id="comboEduProgram" runat="server" CssClass="dnn-select2"
                        DataValueField="Value"
                        DataTextField="Text" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelEduLevel" runat="server" ControlName="comboEduLevel" />
                    <asp:DropDownList id="comboEduLevel" runat="server"
                        DataValueField="EduLevelID"
                        DataTextField="Title" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="labelProfileCode" runat="server" ControlName="textProfileCode" />
                    <asp:TextBox ID="textProfileCode" runat="server" MaxLength="64" />
                </div>
                <div class="dnnFormItem">
        			<dnn:Label ID="labelProfileTitle" runat="server" ControlName="textProfileTitle" />
        			<asp:TextBox ID="textProfileTitle" runat="server" MaxLength="250" />
                </div>
                <div class="dnnFormItem dnnFormRequired">
                    <dnn:Label id="labelLanguages" runat="server" ControlName="textLanguages" />
                    <asp:TextBox id="textLanguages" runat="server" MaxLength="250" />
					<asp:RequiredFieldValidator runat="server" ControlToValidate="textLanguages" ValidationGroup="EduProgramProfile"
						Display="Dynamic" CssClass="dnnFormMessage dnnFormError" resourcekey="Languages.Required" />
				</div>
				<div class="checkbox-group">
    				<div class="dnnFormItem">
                        <dnn:Label id="labelIsAdopted" runat="server" ControlName="checkIsAdopted" />
                        <asp:CheckBox id="checkIsAdopted" runat="server" />
                    </div>
    				<div class="dnnFormItem">
                        <dnn:Label id="labelELearning" runat="server" ControlName="checkELearning" />
                        <asp:CheckBox id="checkELearning" runat="server" />
                    </div>
    				<div class="dnnFormItem">
                        <dnn:Label id="labelDistanceEducation" runat="server" ControlName="checkDistanceEducation" />
                        <asp:CheckBox id="checkDistanceEducation" runat="server" />
                    </div>
				</div>
				<div class="dnnFormItem">
                    <dnn:Label ID="labelAccreditedToDate" runat="server" ControlName="dateAccreditedToDate" />
                    <dnn:DnnDatePicker id="dateAccreditedToDate" runat="server" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="labelCommunityAccreditedToDate" runat="server" ControlName="dateCommunityAccreditedToDate" />
                    <dnn:DnnDatePicker id="dateCommunityAccreditedToDate" runat="server" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="labelStartDate" runat="server" ControlName="datetimeStartDate" />
                    <dnn:DnnDateTimePicker id="datetimeStartDate" runat="server" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="labelEndDate" runat="server" ControlName="datetimeEndDate" />
                    <dnn:DnnDateTimePicker id="datetimeEndDate" runat="server" />
                </div>
        	</fieldset>
        </div>
		<div id="eduprogramprofile-eduformyears-tab">
            <controls:EditEduFormYears id="formEditEduFormYears" runat="server" />
        </div>
		<div id="eduprogramprofile-divisions-tab">
			<controls:EditDivisions id="formEditDivisions" runat="server" ForModel="EduProgramProfile" />
		</div>
        <div id="eduprogramprofile-audit-tab">
            <fieldset>
                <div class="dnnFormItem">
                    <dnn:Label id="labelAudit" runat="server" ControlName="auditControl" />
                    <dnn:Audit id="auditControl" runat="server" />
                </div>
            </fieldset>
		</div>
    </div>
    <ul class="dnnActions dnnClear">
		<li><asp:LinkButton id="buttonUpdate" runat="server" CssClass="dnnPrimaryAction" ResourceKey="cmdUpdate" CausesValidation="true" ValidationGroup="EduProgramProfile" /></li>
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
	    $("#eduprogramprofile-tabs").dnnTabs({selected: document.getElementById("hiddenSelectedTab").value});
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
