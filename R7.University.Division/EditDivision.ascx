<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditDivision.ascx.cs" Inherits="R7.University.Division.EditDivision" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Audit" Src="~/controls/ModuleAuditControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Url" Src="~/controls/DnnUrlControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="AgplSignature" Src="~/DesktopModules/R7.University/R7.University/Controls/AgplSignature.ascx" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University.Division/admin.css" Priority="200" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/module.css" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/admin.css" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/dnn-ac-combobox.css" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/js/dnn-ac-combobox.js" />

<div class="dnnForm dnnClear">
    <div id="division-tabs">
        <ul class="dnnAdminTabNav dnnClear">
            <li><a href="#division-common-tab"><%= LocalizeString("Common.Tab") %></a></li>
            <li><a href="#division-contacts-tab"><%= LocalizeString("Contacts.Tab") %></a></li>
            <li><a href="#division-documents-tab"><%= LocalizeString("Documents.Tab") %></a></li>
            <li><a href="#division-bindings-tab"><%= LocalizeString("Bindings.Tab") %></a></li>
        </ul>
        <div id="division-common-tab">
        	<fieldset>	
        		<div class="dnnFormItem">
        			<dnn:Label id="lblTitle" runat="server" ControlName="txtTitle" />
        			<asp:TextBox id="txtTitle" runat="server" MaxLength="128" />
        		</div>
        		<div class="dnnFormItem">
        			<dnn:Label id="lblShortTitle" runat="server" ControlName="txtShortTitle" />
        			<asp:TextBox id="txtShortTitle" runat="server" MaxLength="64" />
        		</div>
        		<div class="dnnFormItem">
        			<dnn:Label id="lblParentDivision" runat="server" ControlName="treeParentDivisions" />
                    <dnn:DnnTreeView id="treeParentDivisions" runat="server"
                        DataFieldID="DivisionID"
                        DataFieldParentID="ParentDivisionID"
                        DataValueField="DivisionID"
                        DataTextField="Title"
                    />
        		</div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelHeadPosition" runat="server" ControlName="comboHeadPosition" />
                    <asp:DropDownList id="comboHeadPosition" runat="server" CssClass="dnn-ac-combobox"
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
                    <dnn:Label id="labelIsVirtual" runat="server" ControlName="checkIsVirtual" />
                    <asp:CheckBox id="checkIsVirtual" runat="server" />
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
                    <dnn:Label id="lblLocation" runat="server" ControlName="txtLocation" />
                    <asp:TextBox id="txtLocation" runat="server" MaxLength="128" />
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
                    <asp:TextBox id="textWorkingHours" runat="server" Style="width:300px" />
                    <asp:CheckBox id="checkAddToVocabulary" runat="server" resourcekey="checkAddToVocabulary" />
                </div>
            </fieldset>
        </div>
        <div id="division-documents-tab">
            <fieldset>
                <div class="dnnFormItem">
                    <dnn:Label id="labelDocumentUrl" runat="server" ControlName="urlDocumentUrl" />
                    <dnn:Url id="urlDocumentUrl" runat="server" UrlType="N" 
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
                <div class="dnnFormItem">
                    <dnn:Label id="lblDivisionTerm" runat="server" ControlName="treeDivisionTerms" />
                    <dnn:DnnTreeView ID="treeDivisionTerms" runat="server" 
                        DataFieldID="TermId"
                        DataFieldParentID="ParentTermId"
                        DataTextField="Name"
                        DataValueField="TermId"
                    />
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
	<controls:AgplSignature runat="server" />
</div>
<script type="text/javascript">
(function($, Sys) {
    function setupModule() {
        dnnAcCombobox_Init($);
        $(".dnn-ac-combobox").combobox();
    };
    $(document).ready(function() {
        $("#division-tabs").dnnTabs({selected: <%= (int)SelectedTab %>});
        setupModule();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function() {
            setupModule();
        });
    });
} (jQuery, window.Sys));
</script>