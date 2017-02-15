<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsEmployee.ascx.cs" Inherits="R7.University.Employee.SettingsEmployee" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University.Employee/admin.css" Priority="200" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/admin.css" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/dnn-ac-combobox.css" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/js/dnn-ac-combobox.js" />

<div class="dnnForm dnnClear">
	<asp:Panel id="panelGeneralSettings" runat="server">
    	<h2 class="dnnFormSectionHead"><a href="#"><%: LocalizeString ("GeneralSettings.Section") %></a></h2>
    	<fieldset>
    		<div class="dnnFormItem">
    			<dnn:Label id="labelEmployee" runat="server" ControlName="comboEmployees" />
                <asp:DropDownList id="comboEmployees" runat="server" CssClass="dnn-ac-combobox"
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