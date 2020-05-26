<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditDivision.ascx.cs" Inherits="R7.University.Divisions.EditDivision" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Audit" Src="~/controls/ModuleAuditControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Url" Src="~/controls/DnnUrlControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="AgplSignature" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/AgplSignature.ascx" %>
<%@ Register TagPrefix="controls" TagName="DivisionSelector" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/DivisionSelector.ascx" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University.Divisions/admin.css" Priority="200" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/assets/css/module.css" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/assets/css/admin.css" />

<div class="dnnForm dnnClear">
    <div id="division-tabs">
        <ul class="dnnAdminTabNav dnnClear">
            <li><a href="#division-common-tab"><%= LocalizeString("Common.Tab") %></a></li>
            <li><a href="#division-contacts-tab"><%= LocalizeString("Contacts.Tab") %></a></li>
            <li><a href="#division-documents-tab"><%= LocalizeString("Documents.Tab") %></a></li>
            <li><a href="#division-bindings-tab"><%= LocalizeString("Bindings.Tab") %></a></li>
			<li><a href="#division-audit-tab"><%= LocalizeString("Audit.Tab") %></a></li>
        </ul>
        <div id="division-common-tab">
        	<fieldset>
        		<div class="dnnFormItem dnnFormRequired">
        			<dnn:Label id="lblTitle" runat="server" ControlName="txtTitle" />
        			<asp:TextBox id="txtTitle" runat="server" MaxLength="128" />
					<asp:RequiredFieldValidator runat="server" resourcekey="Title.Required"
						ControlToValidate="txtTitle" Display="Dynamic"
                        CssClass="dnnFormMessage dnnFormError" />
        		</div>
        		<div class="dnnFormItem">
        			<dnn:Label id="lblShortTitle" runat="server" ControlName="txtShortTitle" />
        			<asp:TextBox id="txtShortTitle" runat="server" MaxLength="64" />
        		</div>
        		<div class="dnnFormItem">
        			<dnn:Label id="lblParentDivision" runat="server" ControlName="parentDivisionSelector" />
					<controls:DivisionSelector id="parentDivisionSelector" runat="server" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelHeadPosition" runat="server" ControlName="comboHeadPosition" />
                    <asp:DropDownList id="comboHeadPosition" runat="server" CssClass="dnn-select2"
                        DataValueField="PositionID"
                        DataTextField="Title" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelStartDate" runat="server" ControlName="datetimeStartDate" />
                    <dnn:DnnDateTimePicker id="datetimeStartDate" runat="server" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelEndDate" runat="server" ControlName="datetimeEndDate" />
                    <dnn:DnnDateTimePicker id="datetimeEndDate" runat="server" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelIsGoverning" runat="server" ControlName="checkIsGoverning" />
                    <asp:CheckBox id="checkIsGoverning" runat="server" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelIsSingleEntity" runat="server" ControlName="checkIsSingleEntity" />
                    <asp:CheckBox id="checkIsSingleEntity" runat="server" />
                </div>
				<div class="dnnFormItem">
                    <dnn:Label id="labelIsInformal" runat="server" ControlName="checkIsInformal" />
                    <asp:CheckBox id="checkIsInformal" runat="server" />
                </div>
            </fieldset>
        </div>
        <div id="division-contacts-tab">
            <fieldset>
        		<div class="dnnFormItem">
        			<dnn:Label id="lblWebSite" runat="server" ControlName="txtWebSite" />
        			<asp:TextBox id="txtWebSite" runat="server" MaxLength="128" />
        		</div>
        		<div class="dnnFormItem">
        			<dnn:Label id="labelWebSiteLabel" runat="server" ControlName="textWebSiteLabel" />
        			<asp:TextBox id="textWebSiteLabel" runat="server" MaxLength="64" />
        		</div>
        		<div class="dnnFormItem">
        			<dnn:Label id="lblPhone" runat="server" ControlName="txtPhone" />
        			<asp:TextBox id="txtPhone" runat="server" MaxLength="64" />
        		</div>
        		<div class="dnnFormItem">
        			<dnn:Label id="lblFax" runat="server" ControlName="txtFax" />
        			<asp:TextBox id="txtFax" runat="server" MaxLength="50" />
        		</div>
        		<div class="dnnFormItem">
        			<dnn:Label id="lblEmail" runat="server" ControlName="txtEmail" />
        			<asp:TextBox id="txtEmail" runat="server" MaxLength="250" />
        		</div>
        		<div class="dnnFormItem">
        			<dnn:Label id="lblSecondaryEmail" runat="server" ControlName="txtSecondaryEmail" />
        			<asp:TextBox id="txtSecondaryEmail" runat="server" MaxLength="250" />
        		</div>
				<div class="dnnFormItem">
                    <dnn:Label id="labelAddress" runat="server" ControlName="textAddress" />
                    <asp:TextBox id="textAddress" runat="server" MaxLength="250" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="lblLocation" runat="server" ControlName="txtLocation" />
                    <asp:TextBox id="txtLocation" runat="server" MaxLength="128" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelWorkingHours" runat="server" ControlName="comboWorkingHours" />
                    <asp:DropDownList id="comboWorkingHours" runat="server"
                                DataTextField="Name"
                                DataValueField="TermId" />
					<button class="btn btn-sm btn-outline-secondary" type="button" onclick="btnCopyWorkingHours_Click()"><%: LocalizeString ("Copy") %></button>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelCustomWorkingHours" runat="server" ControlName="textWorkingHours" />
                    <asp:TextBox id="textWorkingHours" runat="server" MaxLength="100" Style="width:300px" />
                    <asp:CheckBox id="checkAddToVocabulary" runat="server" resourcekey="checkAddToVocabulary" />
                </div>
            </fieldset>
        </div>
        <div id="division-documents-tab">
            <fieldset>
                <div class="dnnFormItem">
                    <dnn:Label id="labelDocumentUrl" runat="server" ControlName="urlDocumentUrl" />
                    <dnn:Url id="urlDocumentUrl" runat="server" UrlType="F"
                            IncludeActiveTab="true"
                            ShowFiles="true" ShowTabs="true"
                            ShowUrls="true" ShowUsers="false"
                            ShowLog="false" ShowTrack="false"
                            ShowNone="true" ShowNewWindow="false" />
                </div>
            </fieldset>
        </div>
        <div id="division-bindings-tab">
            <fieldset>
                <div class="dnnFormItem">
                    <dnn:Label id="lblHomePage" runat="server" ControlName="urlHomePage" />
                    <dnn:Url id="urlHomePage" runat="server" UrlType="T"
                            IncludeActiveTab="true"
                            ShowFiles="false" ShowTabs="true"
                            ShowUrls="true" ShowUsers="false"
                            ShowLog="false" ShowTrack="false"
                            ShowNone="true" ShowNewWindow="false" />
                </div>
            </fieldset>
        </div>
		<div id="division-audit-tab">
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
function btnCopyWorkingHours_Click() {
	var workingHours = $("#<%= comboWorkingHours.ClientID %> option:selected").text();
	$("#<%= textWorkingHours.ClientID %>").val(workingHours);
}
(function($, Sys) {
    function setupModule() {
	    $("#division-tabs").dnnTabs({selected: document.getElementById("hiddenSelectedTab").value});
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
