<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsDivision.ascx.cs" Inherits="R7.University.Division.SettingsDivision" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="DivisionSelector" Src="~/DesktopModules/R7.University/R7.University/Controls/DivisionSelector.ascx" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University.Division/admin.css" Priority="200" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/admin.css" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/dnn-ac-combobox.css" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/js/dnn-ac-combobox.js" />

<div class="dnnForm dnnClear">
    <fieldset>	
		<div class="dnnFormItem">
			<dnn:Label id="labelDivision" runat="server" ControlName="divisionSelector" />
			<controls:DivisionSelector id="divisionSelector" runat="server" />
		</div>
		<div class="dnnFormItem">
            <dnn:Label id="labelShowAddress" runat="server" ControlName="checkShowAddress" />
			<asp:CheckBox id="checkShowAddress" runat="server" />
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
