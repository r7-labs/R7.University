<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsLaunchpad.ascx.cs" Inherits="R7.University.Launchpad.SettingsLaunchpad" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnnweb" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>

<div class="dnnForm dnnClear">
	<h2 class="dnnFormSectionHead"><a href=""><asp:Label runat="server" ResourceKey="sectionBaseSettings.Text" /></a></h2>
	<fieldset>	
		<div class="dnnFormItem">
			<dnn:Label id="labelTables" runat="server" ControlName="listTables" Suffix=":" />
			<dnnweb:DnnListBox id="listTables" runat="server" CheckBoxes="true" Style="margin-bottom:10px" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="labelPageSize" runat="server" ControlName="comboPageSize" Suffix=":" />
			<dnnweb:DnnComboBox id="comboPageSize" runat="server" />
		</div>
	</fieldset>	
</div>

