<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsEduProgramDirectory.ascx.cs" Inherits="R7.University.EduProgramDirectory.SettingsEduProgramDirectory" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="DivisionSelector" Src="~/DesktopModules/R7.University/R7.University/Controls/DivisionSelector.ascx" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/admin.css" Priority="200" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University.EduProgramDirectory/admin.css" Priority="200" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/dnn-ac-combobox.css" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/js/dnn-ac-combobox.js" />

<div class="dnnForm dnnClear eduprogram-directory-settings">
	<fieldset>
        <div class="dnnFormItem">
            <dnn:Label id="labelDivision" runat="server" ControlName="divisionSelector" />
            <controls:DivisionSelector id="divisionSelector" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="labelEduLevels" runat="server" ControlName="listEduLevels" />
            <dnn:DnnListBox id="listEduLevels" runat="server" CheckBoxes="true" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="labelColumns" runat="server" ControlName="listColumns" />
            <dnn:DnnListBox id="listColumns" runat="server" CheckBoxes="true" />
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