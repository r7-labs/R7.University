<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsEmployeeDirectory.ascx.cs" Inherits="R7.University.Employees.SettingsEmployeeDirectory" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University.Employees/admin.css" Priority="200" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/css/admin.css" />

<div class="dnnForm dnnClear employee-directory-settings">
	<asp:Panel id="panelGeneralSettings" runat="server">
        <h2 class="dnnFormSectionHead"><a href="#"><%: LocalizeString ("GeneralSettings.Section") %></a></h2>
        <fieldset>  
            <div class="dnnFormItem">
                <dnn:Label id="labelMode" runat="server" ControlName="comboMode" />
                <asp:DropDownList id="comboMode" runat="server" />
            </div>
            <div class="dnnFormItem">
                <dnn:Label id="labelEduLevels" runat="server" ControlName="listEduLevels" />
                <asp:CheckBoxList id="listEduLevels" runat="server" CssClass="dnn-form-control" />
            </div>
            <div class="dnnFormItem">
                <dnn:Label id="labelShowAllTeachers" runat="server" ControlName="checkShowAllTeachers" />
                <asp:CheckBox id="checkShowAllTeachers" runat="server" />
            </div>
        </fieldset>
	</asp:Panel>	
</div>

