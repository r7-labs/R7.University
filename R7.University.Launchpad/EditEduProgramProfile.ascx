<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditEduProgramProfile.ascx.cs" Inherits="R7.University.Launchpad.EditEduProgramProfile" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Audit" Src="~/controls/ModuleAuditControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="EditEduForms" Src="~/DesktopModules/R7.University/R7.University/Controls/EditEduForms.ascx" %>
<%@ Register TagPrefix="controls" TagName="EditDocuments" Src="~/DesktopModules/R7.University/R7.University/Controls/EditDocuments.ascx" %>
<%@ Register TagPrefix="controls" TagName="AgplFooter" Src="~/DesktopModules/R7.University/R7.University/Controls/AgplFooter.ascx" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/module.css" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/admin.css" Priority="200" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/dnn-ac-combobox.css" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/js/dnn-ac-combobox.js" />

<div class="dnnForm dnnClear university-edit-eduprogramprofile">
    <div id="eduprogramprofile-tabs">
        <ul class="dnnAdminTabNav dnnClear">
            <li><a href="#eduprogramprofile-common-tab"><%= LocalizeString ("Common.Tab") %></a></li>
            <li><a href="#eduprogramprofile-bindings-tab"><%= LocalizeString ("Bindings.Tab") %></a></li>
            <li><a href="#eduprogramprofile-eduforms-tab"><%= LocalizeString ("EduForms.Tab") %></a></li>
            <li><a href="#eduprogramprofile-documents-tab"><%= LocalizeString ("Documents.Tab") %></a></li>
        </ul>
        <div id="eduprogramprofile-common-tab" class="dnnForm dnnClear">
        	<fieldset>
                <asp:UpdatePanel id="updatePanelEduProgram" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
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
                            <asp:DropDownList id="comboEduProgram" runat="server" CssClass="dnn-ac-combobox"
                                DataValueField="EduProgramID"
                                DataTextField="EduProgramString" />
                            <asp:LinkButton id="linkEditEduProgram" runat="server" resourcekey="linkEditEduProgram.Text"
                                OnClick="linkEditEduProgram_Click" CssClass="edit-button-right">
                                <img src="<%= DotNetNuke.Entities.Icons.IconController.IconURL ("Edit") %>" />
                            </asp:LinkButton>
                        </div>
                        <div class="dnnFormItem">
                            <dnn:Label id="labelEduLevel" runat="server" ControlName="comboEduLevel" />
                            <asp:DropDownList id="comboEduLevel" runat="server"
                                DataValueField="EduLevelID"
                                DataTextField="Title" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="dnnFormItem">
                    <dnn:Label ID="labelProfileCode" runat="server" ControlName="textProfileCode" />
                    <asp:TextBox ID="textProfileCode" runat="server" MaxLength="64" />
                </div>
                <div class="dnnFormItem">
        			<dnn:Label ID="labelProfileTitle" runat="server" ControlName="textProfileTitle" />
        			<asp:TextBox ID="textProfileTitle" runat="server" MaxLength="250" />  
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelLanguages" runat="server" ControlName="textLanguages" />
                    <asp:TextBox id="textLanguages" runat="server" MaxLength="250" />
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
        <div id="eduprogramprofile-bindings-tab">
            <fieldset>
                <div class="dnnFormItem">
                    <dnn:Label id="labelDivision" runat="server" ControlName="treeDivision" />
                    <dnn:DnnTreeView id="treeDivision" runat="server"
                        DataFieldID="DivisionID"
                        DataFieldParentID="ParentDivisionID"
                        DataValueField="DivisionID"
                        DataTextField="Title"
                    />
                </div>
            </fieldset>
        </div>
        <div id="eduprogramprofile-eduforms-tab">
            <controls:EditEduForms id="formEditEduForms" runat="server" />
        </div>
        <div id="eduprogramprofile-documents-tab">
            <controls:EditDocuments id="formEditDocuments" runat="server" ForModel="EduProgramProfile" />
        </div>
    </div>
   	<ul class="dnnActions dnnClear">
		<li><asp:LinkButton id="buttonUpdate" runat="server" CssClass="dnnPrimaryAction" ResourceKey="cmdUpdate" CausesValidation="true" ValidationGroup="EduProgramProfile" /></li>
		<li><asp:LinkButton id="buttonDelete" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdDelete" /></li>
		<li><asp:HyperLink id="linkCancel" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdCancel" /></li>
	</ul>
    <hr />
    <dnn:Audit id="auditControl" runat="server" />
	<controls:AgplFooter id="agplFooter" runat="server" />
</div>
<script type="text/javascript">
(function($, Sys) {
    function setupModule() {
        dnnAcCombobox_Init($);
        $(".dnn-ac-combobox").combobox();
    };
    $(document).ready(function() {
        $("#eduprogramprofile-tabs").dnnTabs({selected: <%= SelectedTab %>});
        setupModule();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function() {
            setupModule();
        });
    });
} (jQuery, window.Sys));
</script>