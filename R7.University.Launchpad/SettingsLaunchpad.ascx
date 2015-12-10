<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsLaunchpad.ascx.cs" Inherits="R7.University.Launchpad.SettingsLaunchpad" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University.Launchpad/admin.css" Priority="200" />
<div class="dnnForm dnnClear university-launchpad-settings">
	<h2 class="dnnFormSectionHead"><a href=""><asp:Label runat="server" ResourceKey="sectionBaseSettings.Text" /></a></h2>
	<fieldset>	
		<div class="dnnFormItem">
			<dnn:Label id="labelTables" runat="server" ControlName="listTables" Suffix=":" />
			<dnn:DnnListBox id="listTables" runat="server" CheckBoxes="true" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="labelPageSize" runat="server" ControlName="comboPageSize" Suffix=":" />
			<dnn:DnnComboBox id="comboPageSize" runat="server" />
		</div>
	</fieldset>	
</div>

