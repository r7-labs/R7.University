<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditEmployeeListSettings.ascx.cs" Inherits="R7.University.Employees.EditEmployeeListSettings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="DivisionSelector" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/DivisionSelector.ascx" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University.Employees/admin.css" Priority="200" />
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
    <h2 class="dnnFormSectionHead"><a href="#"><%: LocalizeString ("DisplaySettings.Section") %></a></h2>
	<fieldset>
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
