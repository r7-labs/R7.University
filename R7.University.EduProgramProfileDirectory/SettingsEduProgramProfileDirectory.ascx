<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="SettingsEduProgramProfileDirectory.ascx.cs" 
    Inherits="R7.University.EduProgramProfileDirectory.SettingsEduProgramProfileDirectory" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="DivisionSelector" Src="~/DesktopModules/R7.University/R7.University/Controls/DivisionSelector.ascx" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/admin.css" Priority="200" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University.EduProgramProfileDirectory/admin.css" Priority="200" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/dnn-ac-combobox.css" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/js/dnn-ac-combobox.js" />

<div class="dnnForm dnnClear eduprogramprofile-directory-settings">
	<fieldset>
		<div class="dnnFormItem">
            <dnn:Label id="labelDivision" runat="server" ControlName="divisionSelector" />
            <controls:DivisionSelector id="divisionSelector" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="labelDivisionLevel" runat="server" ControlName="radioDivisionLevel" />
            <asp:RadioButtonList id="radioDivisionLevel" runat="server"
                DataValueField="Value"
                DataTextField="ValueLocalized"
                RepeatDirection="Horizontal"
                CssClass="dnn-form-control"
           />
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="labelMode" runat="server" ControlName="comboMode" />
            <asp:DropDownList id="comboMode" runat="server"
                DataValueField="Value"
                DataTextField="ValueLocalized"
            />
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="labelEduLevels" runat="server" ControlName="listEduLevels" />
            <dnn:DnnListBox id="listEduLevels" runat="server" CheckBoxes="true" />
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