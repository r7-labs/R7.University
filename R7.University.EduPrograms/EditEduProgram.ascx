﻿<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditEduProgram.ascx.cs" Inherits="R7.University.EduPrograms.EditEduProgram" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Audit" Src="~/controls/ModuleAuditControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Url" Src="~/controls/DnnUrlControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="EditDocuments" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/EditDocuments.ascx" %>
<%@ Register TagPrefix="controls" TagName="EditDivisions" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/EditDivisions.ascx" %>
<%@ Register TagPrefix="controls" TagName="AgplSignature" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/AgplSignature.ascx" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/assets/css/module.css" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/assets/css/admin.css" Priority="200" />

<div class="dnnForm dnnClear">
    <div id="eduprogram-tabs" class="dnnForm dnnClear">
        <ul class="dnnAdminTabNav dnnClear">
            <li><a href="#eduprogram-common-tab"><%= LocalizeString ("Common.Tab") %></a></li>
			<li><a href="#eduprogram-profiles-tab"><%= LocalizeString ("EduProgramProfiles.Tab") %></a></li>
			<li><a href="#eduprogram-divisions-tab"><%= LocalizeString ("Divisions.Tab") %></a></li>
            <li><a href="#eduprogram-bindings-tab"><%= LocalizeString ("Bindings.Tab") %></a></li>
            <li><a href="#eduprogram-documents-tab"><%= LocalizeString ("Documents.Tab") %></a></li>
			<li><a href="#eduprogram-audit-tab"><%= LocalizeString ("Audit.Tab") %></a></li>
        </ul>
        <div id="eduprogram-common-tab">
        	<fieldset>
        		<div class="dnnFormItem dnnFormRequired">
                    <dnn:Label ID="labelCode" runat="server" ControlName="textCode" />
                    <asp:TextBox ID="textCode" runat="server" MaxLength="64" />
                    <asp:RequiredFieldValidator runat="server" resourcekey="Code.Required"
                        ControlToValidate="textCode" ValidationGroup="EduProgram"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
                <div class="dnnFormItem dnnFormRequired">
        			<dnn:Label ID="labelTitle" runat="server" ControlName="textTitle" />
        			<asp:TextBox ID="textTitle" runat="server" MaxLength="250" />
                    <asp:RequiredFieldValidator runat="server" resourcekey="Title.Required"
                        ControlToValidate="textTitle" ValidationGroup="EduProgram"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
        		</div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelEduLevel" runat="server" ControlName="comboEduLevel" />
                    <asp:DropDownList id="comboEduLevel" runat="server"
                        DataTextField="Title"
                        DataValueField="EduLevelID"
                    />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="labelGeneration" runat="server" ControlName="textGeneration" />
                    <asp:TextBox ID="textGeneration" runat="server" MaxLength="16" />
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
		<div id="eduprogram-profiles-tab">
            <fieldset>
                <div class="dnnFormItem">
					<asp:GridView id="gridEduProfiles" runat="server" AutoGenerateColumns="false"
							OnRowDataBound="gridEduProfiles_RowDataBound"
                            GridLines="None" CssClass="dnnGrid" Style="width:100%;margin-bottom:30px">
                        <HeaderStyle CssClass="dnnGridHeader" HorizontalAlign="Left" />
                        <RowStyle CssClass="dnnGridItem" HorizontalAlign="Left" />
                        <AlternatingRowStyle CssClass="dnnGridAltItem" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HyperLink runat="server"
										ImageUrl="<%# R7.University.Components.UniversityIcons.Edit %>"
										NavigateUrl='<%# Eval ("Edit_Url") %>'
										Visible="<%# SecurityContext.IsAdmin %>"
										ToolTip='<%# LocalizeString ("EditEduProgramProfile.Action") %>'
										Style="margin-right:.5em"
									/>
									<asp:HyperLink runat="server"
										ImageUrl="<%# R7.University.Components.UniversityIcons.EditDocuments %>"
										NavigateUrl='<%# Eval ("EditDocuments_Url") %>'
										ToolTip='<%# LocalizeString ("EditEduProgramProfileDocuments.Action") %>'
									/>
								</ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="EduProgramProfile_String" HeaderText="EduProgramProfile.Column" />
                            <asp:BoundField DataField="EduLevel_String" HeaderText="EduLevel.Column" />
							<asp:BoundField DataField="StartDate" HeaderText="StartDate.Column" DataFormatString="{0:d}" />
							<asp:BoundField DataField="EndDate" HeaderText="EndDate.Column" DataFormatString="{0:d}" />
                        </Columns>
                    </asp:GridView>
			    </div>
				<div class="dnnFormItem">
					<label class="dnnLabel" />
					<asp:HyperLink id="linkAddEduProgramProfile" runat="server" CssClass="dnnPrimaryAction" resourcekey="AddEduProgramProfile.Text" />
                </div>
				<asp:Panel id="panelAddDefaultProfile" runat="server" CssClass="dnnFormItem">
                    <dnn:Label id="labelAddDefaultProfile" runat="server" ControlName="checkAddDefaultProfile" />
                    <asp:CheckBox id="checkAddDefaultProfile" runat="server" Checked="true" />
                </asp:Panel>
			</fieldset>
        </div>
        <div id="eduprogram-divisions-tab">
			<controls:EditDivisions id="formEditDivisions" runat="server" ForModel="EduProgram" />
		</div>
        <div id="eduprogram-bindings-tab">
            <fieldset>
                <div class="dnnFormItem">
                    <dnn:Label id="labelHomePage" runat="server" ControlName="urlHomePage" />
                    <dnn:Url id="urlHomePage" runat="server" UrlType="T"
                            IncludeActiveTab="true"
                            ShowFiles="false" ShowTabs="true"
                            ShowUrls="false" ShowUsers="false"
                            ShowLog="false" ShowTrack="false"
                            ShowNone="true" ShowNewWindow="false" />
                </div>
				<div class="dnnFormItem">
					<dnn:Label id="lblUseCurrentPageAsHomePage" runat="server" ControlName="chkUseCurrentPageAsHomePage" />
					<asp:CheckBox id="chkUseCurrentPageAsHomePage" runat="server" />
				</div>
            </fieldset>
        </div>
        <div id="eduprogram-documents-tab">
            <controls:EditDocuments id="formEditDocuments" runat="server" ForModel="EduProgram" />
        </div>
		<div id="eduprogram-audit-tab">
            <fieldset>
                <div class="dnnFormItem">
                    <dnn:Label id="labelAudit" runat="server" ControlName="auditControl" />
                    <dnn:Audit id="auditControl" runat="server" />
                </div>
            </fieldset>
        </div>
		<ul class="dnnActions dnnClear">
            <li><asp:LinkButton id="buttonUpdate" runat="server" CssClass="dnnPrimaryAction" ResourceKey="cmdUpdate" CausesValidation="true" ValidationGroup="EduProgram" /></li>
			<li>&nbsp;</li>
            <li><asp:LinkButton id="buttonDelete" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdDelete" /></li>
			<li>&nbsp;</li>
            <li><asp:HyperLink id="linkCancel" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdCancel" /></li>
        </ul>
		<controls:AgplSignature runat="server" ShowRule="false" />
    </div>
</div>
<input id="hiddenSelectedTab" type="hidden" value="<%= (int) SelectedTab %>" />
<script type="text/javascript">
(function($, Sys) {
    function setupModule() {
        var selectedTab = document.getElementById("hiddenSelectedTab").value;
        $("#eduprogram-tabs").dnnTabs({selected: selectedTab});
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
