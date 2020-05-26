<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditLaunchpadSettings.ascx.cs" Inherits="R7.University.Launchpad.EditLaunchpadSettings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/assets/css/admin.css" Priority="200" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University.Launchpad/admin.css" Priority="200" />

<div class="dnnForm dnnClear university-launchpad-settings">
	<asp:Panel id="panelGeneralSettings" runat="server">
	    <h2 class="dnnFormSectionHead"><a href="#"><asp:Label runat="server" ResourceKey="GeneralSettings.Section" /></a></h2>
    	<fieldset>
    		<div class="dnnFormItem">
    			<dnn:Label id="labelTables" runat="server" ControlName="listTables" />
    			<asp:CheckBoxList id="listTables" runat="server" CssClass="dnn-form-control" />
    		</div>
        </fieldset>
    </asp:Panel>
	<h2 class="dnnFormSectionHead"><a href="#"><asp:Label runat="server" ResourceKey="DisplaySettings.Section" /></a></h2>
	<fieldset>
		<div class="dnnFormItem">
			<dnn:Label id="labelPageSize" runat="server" ControlName="comboPageSize" />
			<asp:DropDownList id="comboPageSize" runat="server" />
		</div>
	</fieldset>
</div>

