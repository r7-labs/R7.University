<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditEduVolume.ascx.cs" Inherits="R7.University.EduProgramProfiles.EditEduVolume" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Audit" Src="~/controls/ModuleAuditControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="AgplSignature" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/AgplSignature.ascx" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/assets/css/module.css" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/assets/css/admin.css" Priority="200" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University.EduProgramProfiles/js/editEduVolume.js" ForceProvider="DnnFormBottomProvider" />

<div class="dnnForm dnnClear u8y-edit-eduvolume">
	<asp:ValidationSummary runat="server" EnableClientScript="true" ValidationGroup="EduVolume" CssClass="dnnFormMessage dnnFormWarning" />
	<div id="eduvolume-tabs">
        <ul class="dnnAdminTabNav dnnClear">
            <li id="tabCommon" runat="server"><a href="#<%= pnlCommon.ClientID %>"><%= LocalizeString ("Common.Tab") %></a></li>
			<li id="tabYears" runat="server"><a href="#<%= pnlYears.ClientID %>"><%= LocalizeString ("Years.Tab") %></a></li>
			<li id="tabPractices" runat="server"><a href="#<%= pnlPractices.ClientID %>"><%= LocalizeString ("Practices.Tab") %></a></li>
        </ul>
	    <asp:Panel id="pnlCommon" runat="server">
			<fieldset>
		        <div class="dnnFormItem dnnFormRequired">
                    <dnn:Label id="labelTimeToLearnYears" runat="server" ControlName="textTimeToLearnYears" />
                    <asp:TextBox id="textTimeToLearnYears" runat="server" Value="0" />
                    <asp:RequiredFieldValidator runat="server" resourcekey="TimeToLearnYears.Required"
                        ControlToValidate="textTimeToLearnYears" ValidationGroup="EduVolume"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                    <asp:RangeValidator runat="server" resourcekey="TimeToLearnYears.Invalid"
                        ControlToValidate="textTimeToLearnYears" ValidationGroup="EduVolume"
                        Type="Integer" MinimumValue="0" MaximumValue="11"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                    <%--
					<asp:CustomValidator runat="server" resourcekey="TimeToLearn.Required"
                        ControlToValidate="textTimeToLearnYears"
                        ValidationGroup="EduVolume" EnableClientScript="true" ClientValidationFunction="validateTimeToLearn"
                        Display="None" CssClass="dnnFormMessage dnnFormError"  />
					--%>
                </div>
                <div class="dnnFormItem dnnFormRequired">
                    <dnn:Label id="labelTimeToLearnMonths" runat="server" ControlName="textTimeToLearnMonths" />
                    <asp:TextBox id="textTimeToLearnMonths" runat="server" Value="0" />
                    <asp:RequiredFieldValidator runat="server" resourcekey="TimeToLearnMonths.Required"
                        ControlToValidate="textTimeToLearnMonths" ValidationGroup="EduVolume"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                    <asp:RangeValidator runat="server" resourcekey="TimeToLearnMonths.Invalid"
                        ControlToValidate="textTimeToLearnMonths" ValidationGroup="EduVolume"
                        Type="Integer" MinimumValue="0" MaximumValue="11"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
                <div class="dnnFormItem dnnFormRequired">
                    <dnn:Label id="labelTimeToLearnHours" runat="server" ControlName="textTimeToLearnHours" />
                    <asp:TextBox id="textTimeToLearnHours" runat="server" Value="0" />
                    <asp:RequiredFieldValidator runat="server" resourcekey="TimeToLearnHours.Required"
                        ControlToValidate="textTimeToLearnHours" ValidationGroup="EduVolume"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                    <asp:RangeValidator runat="server" resourcekey="TimeToLearnHours.Invalid"
                        ControlToValidate="textTimeToLearnHours" ValidationGroup="EduVolume"
                        Type="Integer" MinimumValue="0" MaximumValue="2147483647"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
			</fieldset>
        </asp:Panel>
        <asp:Panel id="pnlYears" runat="server">
            <fieldset>
				<div class="dnnFormItem">
                    <dnn:Label id="labelYear1Cu" runat="server" ControlName="textYear1Cu" />
					<asp:TextBox id="textYear1Cu" runat="server" />
                    <asp:RangeValidator runat="server" resourcekey="YearCu.Invalid"
                        ControlToValidate="textYear1Cu" ValidationGroup="EduVolume"
                        Type="Integer" MinimumValue="0" MaximumValue="1000"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
				</div>
				<div class="dnnFormItem">
                    <dnn:Label id="labelYear2Cu" runat="server" ControlName="textYear2Cu" />
                    <asp:TextBox id="textYear2Cu" runat="server" />
                    <asp:RangeValidator runat="server" resourcekey="YearCu.Invalid"
                        ControlToValidate="textYear2Cu" ValidationGroup="EduVolume"
                        Type="Integer" MinimumValue="0" MaximumValue="1000"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
				<div class="dnnFormItem">
                    <dnn:Label id="labelYear3Cu" runat="server" ControlName="textYear3Cu" />
                    <asp:TextBox id="textYear3Cu" runat="server" />
				    <asp:RangeValidator runat="server" resourcekey="YearCu.Invalid"
                        ControlToValidate="textYear3Cu" ValidationGroup="EduVolume"
                        Type="Integer" MinimumValue="0" MaximumValue="1000"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
				<div class="dnnFormItem">
                    <dnn:Label id="labelYear4Cu" runat="server" ControlName="textYear4Cu" />
                    <asp:TextBox id="textYear4Cu" runat="server" />
                    <asp:RangeValidator runat="server" resourcekey="YearCu.Invalid"
                        ControlToValidate="textYear4Cu" ValidationGroup="EduVolume"
                        Type="Integer" MinimumValue="0" MaximumValue="1000"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
				<div class="dnnFormItem">
                    <dnn:Label id="labelYear5Cu" runat="server" ControlName="textYear5Cu" />
                    <asp:TextBox id="textYear5Cu" runat="server" />
                    <asp:RangeValidator runat="server" resourcekey="YearCu.Invalid"
                        ControlToValidate="textYear5Cu" ValidationGroup="EduVolume"
                        Type="Integer" MinimumValue="0" MaximumValue="1000"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
				<div class="dnnFormItem">
                    <dnn:Label id="labelYear6Cu" runat="server" ControlName="textYear6Cu" />
                    <asp:TextBox id="textYear6Cu" runat="server" />
                    <asp:RangeValidator runat="server" resourcekey="YearCu.Invalid"
                        ControlToValidate="textYear6Cu" ValidationGroup="EduVolume"
                        Type="Integer" MinimumValue="0" MaximumValue="1000"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
			</fieldset>
        </asp:Panel>
		<asp:Panel id="pnlPractices" runat="server">
            <fieldset>
                <div class="dnnFormItem">
                    <dnn:Label id="labelPracticeType1Cu" runat="server" ControlName="textPracticeType1Cu" />
                    <asp:TextBox id="textPracticeType1Cu" runat="server" />
					<asp:RangeValidator runat="server" resourcekey="PracticeCu.Invalid"
                        ControlToValidate="textPracticeType1Cu" ValidationGroup="EduVolume"
                        Type="Integer" MinimumValue="0" MaximumValue="1000"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
				<div class="dnnFormItem">
                    <dnn:Label id="labelPracticeType2Cu" runat="server" ControlName="textPracticeType2Cu" />
                    <asp:TextBox id="textPracticeType2Cu" runat="server" />
                    <asp:RangeValidator runat="server" resourcekey="PracticeCu.Invalid"
                        ControlToValidate="textPracticeType2Cu" ValidationGroup="EduVolume"
                        Type="Integer" MinimumValue="0" MaximumValue="1000"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
				<div class="dnnFormItem">
                    <dnn:Label id="labelPracticeType3Cu" runat="server" ControlName="textPracticeType3Cu" />
                    <asp:TextBox id="textPracticeType3Cu" runat="server" />
                    <asp:RangeValidator runat="server" resourcekey="PracticeCu.Invalid"
                        ControlToValidate="textPracticeType3Cu" ValidationGroup="EduVolume"
                        Type="Integer" MinimumValue="0" MaximumValue="1000"
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
            </fieldset>
        </asp:Panel>
    </div>
    <ul class="dnnActions dnnClear">
		<li><asp:LinkButton id="buttonUpdate" runat="server" CssClass="btn btn-primary" ResourceKey="cmdUpdate" CausesValidation="true" ValidationGroup="EduVolume" /></li>
		<li>&nbsp;</li>
		<li><asp:LinkButton id="buttonDelete" runat="server" CssClass="btn btn-danger" ResourceKey="cmdDelete" /></li>
		<li>&nbsp;</li>
		<li><asp:HyperLink id="linkCancel" runat="server" CssClass="btn btn-outline-secondary" ResourceKey="cmdCancel" /></li>
	</ul>
	<controls:AgplSignature runat="server" ShowRule="false" />
</div>
<input id="hiddenSelectedTab" type="hidden" value="<%= (int) SelectedTab %>" />
<script type="text/javascript">
(function($, Sys) {
    function setupModule() {
	    $("#eduvolume-tabs").dnnTabs({selected: document.getElementById("hiddenSelectedTab").value});
    };
    $(document).ready(function() {
        setupModule();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function() {
            setupModule();
        });
    });
} (jQuery, window.Sys));
</script>
