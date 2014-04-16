<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsEmployee.ascx.cs" Inherits="R7.University.Employee.SettingsEmployee" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>

<div class="dnnForm dnnClear">
	<h2 class="dnnFormSectionHead"><a href=""><asp:Label runat="server" ResourceKey="sectionBaseSettings.Text" /></a></h2>
	<fieldset>	
		<div class="dnnFormItem">
			<dnn:Label id="labelEmployee" runat="server" ControlName="comboEmployees" Suffix=":" />
			<dnn:DnnComboBox id="comboEmployees" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="labelPhotoWidth" runat="server" ControlName="textPhotoWidth" Suffix=":" />
			<asp:TextBox id="textPhotoWidth" runat="server" Style="width:100px" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="labelAutoTitle" runat="server" ControlName="checkAutoTitle" Suffix=":" />
			<asp:CheckBox id="checkAutoTitle" runat="server" Checked="true" />
		</div>
	</fieldset>	
</div>
