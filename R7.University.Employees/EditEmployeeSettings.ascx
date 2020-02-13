<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditEmployeeSettings.ascx.cs" Inherits="R7.University.Employees.EditEmployeeSettings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="JavaScriptLibraryInclude" Src="~/admin/Skins/JavaScriptLibraryInclude.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:JavaScriptLibraryInclude runat="server" Name="Select2" />
<dnn:DnnCssInclude runat="server" FilePath="~/Resources/Libraries/Select2/04_00_13/css/select2.min.css" />

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University.Employees/admin.css" Priority="200" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/css/admin.css" />

<div class="dnnForm dnnClear">
	<asp:Panel id="panelGeneralSettings" runat="server">
    	<h2 class="dnnFormSectionHead"><a href="#"><%: LocalizeString ("GeneralSettings.Section") %></a></h2>
    	<fieldset>
    		<div class="dnnFormItem">
    			<dnn:Label id="labelEmployee" runat="server" ControlName="comboEmployees" />
                <asp:DropDownList id="comboEmployees" runat="server" CssClass="dnn-select2"
                    DataValueField="EmployeeID"
                    DataTextField="AbbrName"
                />
            </div>
            <div class="dnnFormItem">
                <dnn:Label id="labelShowCurrentUser" runat="server" ControlName="checkShowCurrentUser" />
                <asp:CheckBox id="checkShowCurrentUser" runat="server" Checked="false" />
            </div>
    	</fieldset>
	</asp:Panel>
	<h2 class="dnnFormSectionHead"><a href="#"><%: LocalizeString ("DisplaySettings.Section") %></a></h2>
	<fieldset>
        <div class="dnnFormItem">
            <dnn:Label id="labelAutoTitle" runat="server" ControlName="checkAutoTitle" />
            <asp:CheckBox id="checkAutoTitle" runat="server" Checked="true" />
        </div>
		<div class="dnnFormItem">
			<dnn:Label id="labelPhotoWidth" runat="server" ControlName="textPhotoWidth" />
			<asp:TextBox id="textPhotoWidth" runat="server" />
            <asp:RangeValidator runat="server" resourcekey="PhotoWidth_Invalid.Text"
                ControlToValidate="textPhotoWidth" Type="Integer" MinimumValue="0" MaximumValue="1024"
                Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
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
