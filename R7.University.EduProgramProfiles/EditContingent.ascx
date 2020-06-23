<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditContingent.ascx.cs" Inherits="R7.University.EduProgramProfiles.EditContingent" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Audit" Src="~/controls/ModuleAuditControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="AgplSignature" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/AgplSignature.ascx" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/assets/css/module.css" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/assets/css/admin.css" Priority="200" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University.EduProgramProfiles/js/editEduVolume.js" ForceProvider="DnnFormBottomProvider" />

<div class="dnnForm dnnClear u8y-edit-contingent">
	<asp:ValidationSummary runat="server" EnableClientScript="true" ValidationGroup="EduVolume" CssClass="dnnFormMessage dnnFormWarning" />
    <div id="contingent-tabs">
        <ul class="dnnAdminTabNav dnnClear">
            <li id="tabActual" runat="server"><a href="#<%= panelActual.ClientID %>"><%= LocalizeString ("Actual.Tab") %></a></li>
            <li id="tabVacant" runat="server"><a href="#<%= panelVacant.ClientID %>"><%= LocalizeString ("Vacant.Tab") %></a></li>
			<li id="tabAdmission" runat="server"><a href="#<%= panelAdmission.ClientID %>"><%= LocalizeString ("Admission.Tab") %></a></li>
			<li id="tabMovement" runat="server"><a href="#<%= panelMovement.ClientID %>"><%= LocalizeString ("Movement.Tab") %></a></li>
		</ul>
        <asp:Panel id="panelActual" runat="server">
	        <fieldset>
		        <div class="dnnFormItem">
                    <dnn:Label id="labelActualFB" runat="server" ControlName="textActualFB" />
                    <asp:TextBox id="textActualFB" runat="server" />
                    <asp:RangeValidator runat="server" resourcekey="ContingentValue.Invalid"
                        ControlToValidate="textActualFB" ValidationGroup="Contingent"
                        Type="Integer" MinimumValue="0" MaximumValue="2147483647"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
				<div class="dnnFormItem">
                    <dnn:Label id="labelActualRB" runat="server" ControlName="textActualRB" />
                    <asp:TextBox id="textActualRB" runat="server" />
                    <asp:RangeValidator runat="server" resourcekey="ContingentValue.Invalid"
                        ControlToValidate="textActualRB" ValidationGroup="Contingent"
                        Type="Integer" MinimumValue="0" MaximumValue="2147483647"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
				<div class="dnnFormItem">
                    <dnn:Label id="labelActualMB" runat="server" ControlName="textActualMB" />
                    <asp:TextBox id="textActualMB" runat="server" />
                    <asp:RangeValidator runat="server" resourcekey="ContingentValue.Invalid"
                        ControlToValidate="textActualMB" ValidationGroup="Contingent"
                        Type="Integer" MinimumValue="0" MaximumValue="2147483647"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
				<div class="dnnFormItem">
                    <dnn:Label id="labelActualBC" runat="server" ControlName="textActualBC" />
                    <asp:TextBox id="textActualBC" runat="server" />
                    <asp:RangeValidator runat="server" resourcekey="ContingentValue.Invalid"
                        ControlToValidate="textActualBC" ValidationGroup="Contingent"
                        Type="Integer" MinimumValue="0" MaximumValue="2147483647"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
				<div class="dnnFormItem">
                    <dnn:Label id="lblActualForeign" runat="server" ControlName="txtActualForeign" />
                    <asp:TextBox id="txtActualForeign" runat="server" />
                    <asp:RangeValidator runat="server" resourcekey="ContingentValue.Invalid"
                        ControlToValidate="txtActualForeign" ValidationGroup="Contingent"
                        Type="Integer" MinimumValue="0" MaximumValue="2147483647"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
			</fieldset>
        </asp:Panel>
		<asp:Panel id="panelVacant" runat="server">
            <fieldset>
                <div class="dnnFormItem">
                    <dnn:Label id="labelVacantFB" runat="server" ControlName="textVacantFB" />
                    <asp:TextBox id="textVacantFB" runat="server" />
                    <asp:RangeValidator runat="server" resourcekey="ContingentValue.Invalid"
                        ControlToValidate="textVacantFB" ValidationGroup="Contingent"
                        Type="Integer" MinimumValue="0" MaximumValue="2147483647"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelVacantRB" runat="server" ControlName="textVacantRB" />
                    <asp:TextBox id="textVacantRB" runat="server" />
                    <asp:RangeValidator runat="server" resourcekey="ContingentValue.Invalid"
                        ControlToValidate="textVacantRB" ValidationGroup="Contingent"
                        Type="Integer" MinimumValue="0" MaximumValue="2147483647"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelVacantMB" runat="server" ControlName="textVacantMB" />
                    <asp:TextBox id="textVacantMB" runat="server" />
                    <asp:RangeValidator runat="server" resourcekey="ContingentValue.Invalid"
                        ControlToValidate="textVacantMB" ValidationGroup="Contingent"
                        Type="Integer" MinimumValue="0" MaximumValue="2147483647"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelVacantBC" runat="server" ControlName="textVacantBC" />
                    <asp:TextBox id="textVacantBC" runat="server" />
                    <asp:RangeValidator runat="server" resourcekey="ContingentValue.Invalid"
                        ControlToValidate="textVacantBC" ValidationGroup="Contingent"
                        Type="Integer" MinimumValue="0" MaximumValue="2147483647"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
            </fieldset>
        </asp:Panel>
		<asp:Panel id="panelAdmission" runat="server">
            <fieldset>
                <div class="dnnFormItem">
                    <dnn:Label id="labelAdmittedFB" runat="server" ControlName="textAdmittedFB" />
                    <asp:TextBox id="textAdmittedFB" runat="server" />
                    <asp:RangeValidator runat="server" resourcekey="ContingentValue.Invalid"
                        ControlToValidate="textAdmittedFB" ValidationGroup="Contingent"
                        Type="Integer" MinimumValue="0" MaximumValue="2147483647"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelAdmittedRB" runat="server" ControlName="textAdmittedRB" />
                    <asp:TextBox id="textAdmittedRB" runat="server" />
                    <asp:RangeValidator runat="server" resourcekey="ContingentValue.Invalid"
                        ControlToValidate="textAdmittedRB" ValidationGroup="Contingent"
                        Type="Integer" MinimumValue="0" MaximumValue="2147483647"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelAdmittedMB" runat="server" ControlName="textAdmittedMB" />
                    <asp:TextBox id="textAdmittedMB" runat="server" />
                    <asp:RangeValidator runat="server" resourcekey="ContingentValue.Invalid"
                        ControlToValidate="textAdmittedMB" ValidationGroup="Contingent"
                        Type="Integer" MinimumValue="0" MaximumValue="2147483647"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelAdmittedBC" runat="server" ControlName="textAdmittedBC" />
                    <asp:TextBox id="textAdmittedBC" runat="server" />
                    <asp:RangeValidator runat="server" resourcekey="ContingentValue.Invalid"
                        ControlToValidate="textAdmittedBC" ValidationGroup="Contingent"
                        Type="Integer" MinimumValue="0" MaximumValue="2147483647"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
				<hr />
				<div class="dnnFormItem">
                    <dnn:Label id="labelAvgAdmScore" runat="server" ControlName="textAvgAdmScore" />
                    <asp:TextBox id="textAvgAdmScore" runat="server" />
                    <asp:RangeValidator runat="server" resourcekey="AvgAdmScore.Invalid"
                        ControlToValidate="textAvgAdmScore" ValidationGroup="Contingent"
                        Type="Double" MinimumValue="0" MaximumValue="300"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
            </fieldset>
        </asp:Panel>
		<asp:Panel id="panelMovement" runat="server">
            <fieldset>
                <div class="dnnFormItem">
                    <dnn:Label id="labelMovedOut" runat="server" ControlName="textMovedOut" />
                    <asp:TextBox id="textMovedOut" runat="server" />
                    <asp:RangeValidator runat="server" resourcekey="ContingentValue.Invalid"
                        ControlToValidate="textMovedOut" ValidationGroup="Contingent"
                        Type="Integer" MinimumValue="0" MaximumValue="2147483647"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelMovedIn" runat="server" ControlName="textMovedIn" />
                    <asp:TextBox id="textMovedIn" runat="server" />
                    <asp:RangeValidator runat="server" resourcekey="ContingentValue.Invalid"
                        ControlToValidate="textMovedIn" ValidationGroup="Contingent"
                        Type="Integer" MinimumValue="0" MaximumValue="2147483647"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelRestored" runat="server" ControlName="textRestored" />
                    <asp:TextBox id="textRestored" runat="server" />
                    <asp:RangeValidator runat="server" resourcekey="ContingentValue.Invalid"
                        ControlToValidate="textRestored" ValidationGroup="Contingent"
                        Type="Integer" MinimumValue="0" MaximumValue="2147483647"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelExpelled" runat="server" ControlName="textExpelled" />
                    <asp:TextBox id="textExpelled" runat="server" />
                    <asp:RangeValidator runat="server" resourcekey="ContingentValue.Invalid"
                        ControlToValidate="textExpelled" ValidationGroup="Contingent"
                        Type="Integer" MinimumValue="0" MaximumValue="2147483647"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
            </fieldset>
        </asp:Panel>
	</div>
    <ul class="dnnActions dnnClear">
		<li><asp:LinkButton id="buttonUpdate" runat="server" CssClass="dnnPrimaryAction" ResourceKey="cmdUpdate" CausesValidation="true" ValidationGroup="EduVolume" /></li>
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
	    $("#contingent-tabs").dnnTabs({selected: document.getElementById("hiddenSelectedTab").value});
    };
    $(document).ready(function() {
        setupModule();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function() {
            setupModule();
        });
    });
} (jQuery, window.Sys));
</script>
