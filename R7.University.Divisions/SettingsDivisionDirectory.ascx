<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsDivisionDirectory.ascx.cs" Inherits="R7.University.Divisions.SettingsDivisionDirectory" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University.Divisions/admin.css" Priority="200" />

<div class="dnnForm dnnClear">
	<asp:Panel id="panelGeneralSettings" runat="server">
    <h2 class="dnnFormSectionHead"><a href="#"><%: LocalizeString ("GeneralSettings.Section") %></a></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label id="labelMode" runat="server" ControlName="comboMode" />
            <asp:DropDownList id="comboMode" runat="server" />
        </div>
	</fieldset>
	</asp:Panel>
	<h2 class="dnnFormSectionHead"><a href="#"><%: LocalizeString ("DisplaySettings.Section") %></a></h2>
	<fieldset>
		<div class="dnnFormItem">
            <dnn:Label id="labelShowInformal" runat="server" ControlName="checkShowInformal" />
            <asp:CheckBox id="checkShowInformal" runat="server" />
        </div>
    </fieldset>
</div>
