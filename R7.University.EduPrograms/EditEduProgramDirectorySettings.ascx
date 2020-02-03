<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditEduProgramDirectorySettings.ascx.cs" Inherits="R7.University.EduPrograms.EditEduProgramDirectorySettings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="JavaScriptLibraryInclude" Src="~/admin/Skins/JavaScriptLibraryInclude.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="DivisionSelector" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/DivisionSelector.ascx" %>

<dnn:JavaScriptLibraryInclude runat="server" Name="Select2" />
<dnn:DnnCssInclude runat="server" FilePath="~/Resources/Libraries/Select2/04_00_13/css/select2.min.css" />

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/css/admin.css" Priority="200" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University.EduPrograms/admin.css" Priority="200" />

<div class="dnnForm dnnClear eduprogram-directory-settings">
	<asp:Panel id="panelGeneralSettings" runat="server">
        <h2 class="dnnFormSectionHead"><a href="#"><%: LocalizeString ("GeneralSettings.Section") %></a></h2>
    	<fieldset>
            <div class="dnnFormItem">
                <dnn:Label id="labelDivision" runat="server" ControlName="divisionSelector" />
                <controls:DivisionSelector id="divisionSelector" runat="server" />
            </div>
            <div class="dnnFormItem">
                <dnn:Label id="labelEduLevels" runat="server" ControlName="listEduLevels" />
                <asp:CheckBoxList id="listEduLevels" runat="server" CssClass="dnn-form-control" />
            </div>
    	</fieldset>
	</asp:Panel>
	<h2 class="dnnFormSectionHead"><a href="#"><%: LocalizeString ("DisplaySettings.Section") %></a></h2>
	<fieldset>
        <div class="dnnFormItem">
            <dnn:Label id="labelColumns" runat="server" ControlName="listColumns" />
            <asp:CheckBoxList id="listColumns" runat="server" CssClass="dnn-form-control" />
        </div>
	</fieldset>
</div>

<script type="text/javascript">
(function($, Sys) {
    function setupModule() {
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
