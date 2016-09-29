<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsEmployeeList.ascx.cs" Inherits="R7.University.EmployeeList.SettingsEmployeeList" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="DivisionSelector" Src="~/DesktopModules/R7.University/R7.University/Controls/DivisionSelector.ascx" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University.EmployeeList/admin.css" Priority="200" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/admin.css" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/dnn-ac-combobox.css" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/js/dnn-ac-combobox.js" />

<div class="dnnForm dnnClear">
	<fieldset>	
		<div class="dnnFormItem">
			<dnn:Label id="labelDivision" runat="server" ControlName="divisionDivision" />
            <controls:DivisionSelector id="divisionDivision" runat="server" DefaultMode="List" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="labelIncludeSubdivisions" runat="server" ControlName="checkIncludeSubdivisions" />
			<asp:CheckBox id="checkIncludeSubdivisions" runat="server" />
		</div>
        <div class="dnnFormItem">
            <dnn:Label id="labelHideHeadEmployee" runat="server" ControlName="checkHideHeadEmployee" />
            <asp:CheckBox id="checkHideHeadEmployee" runat="server" />
        </div>
		<div class="dnnFormItem">
			<dnn:Label id="labelSortType" runat="server" ControlName="comboSortType" />
			<asp:DropDownList id="comboSortType" runat="server"/>
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="labelPhotoWidth" runat="server" ControlName="textPhotoWidth" />
			<asp:TextBox id="textPhotoWidth" runat="server" />
            <asp:RangeValidator runat="server" resourcekey="PhotoWidth.Invalid"
                ControlToValidate="textPhotoWidth" Type="Integer" MinimumValue="1" MaximumValue="1024"
                Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
            <asp:RequiredFieldValidator runat="server" resourcekey="PhotoWidth.Required"
                ControlToValidate="textPhotoWidth" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
        </div>
	</fieldset>	
</div>

<script type="text/javascript">
(function($, Sys) {
    function setupModule() {
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