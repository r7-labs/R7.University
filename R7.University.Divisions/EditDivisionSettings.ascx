<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditDivisionSettings.ascx.cs" Inherits="R7.University.Divisions.EditDivisionSettings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="JavaScriptLibraryInclude" Src="~/admin/Skins/JavaScriptLibraryInclude.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="DivisionSelector" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/DivisionSelector.ascx" %>

<dnn:JavaScriptLibraryInclude runat="server" Name="Select2" />
<dnn:DnnCssInclude runat="server" FilePath="~/Resources/Libraries/Select2/04_00_13/css/select2.min.css" />

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University.Divisions/admin.css" Priority="200" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/css/admin.css" />

<div class="dnnForm dnnClear">
	<asp:Panel id="panelGeneralSettings" runat="server">
		<h2 class="dnnFormSectionHead"><a href="#"><%: LocalizeString ("GeneralSettings.Section") %></a></h2>
        <fieldset>
    		<div class="dnnFormItem">
    			<dnn:Label id="labelDivision" runat="server" ControlName="divisionSelector" />
    			<controls:DivisionSelector id="divisionSelector" runat="server" />
    		</div>
    	</fieldset>
    </asp:Panel>
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
