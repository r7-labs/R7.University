<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsEmployeeList.ascx.cs" Inherits="R7.University.EmployeeList.SettingsEmployeeList" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnnweb" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>

<div class="dnnForm dnnClear">
	<h2 class="dnnFormSectionHead"><a href=""><asp:Label runat="server" ResourceKey="sectionBaseSettings.Text" /></a></h2>
	<fieldset>	
		<div class="dnnFormItem">
			<dnn:Label id="labelDivision" runat="server" ControlName="treeDivisions" Suffix=":" />
			<dnnweb:DnnTreeView id="treeDivisions" runat="server" Style="float:left;display:block;margin-bottom:10px;padding:10px;background-color:#EEE"
				DataFieldID="DivisionID"
				DataFieldParentID="ParentDivisionID"
				DataValueField="DivisionID"
				DataTextField="ShortTitle"
			/> 
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="labelIncludeSubdivisions" runat="server" ControlName="checkIncludeSubdivisions" Suffix=":" />
			<asp:CheckBox id="checkIncludeSubdivisions" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="labelSortType" runat="server" ControlName="comboSortType" Suffix=":" />
			<dnnweb:DnnComboBox id="comboSortType" runat="server"/>
		</div>
	</fieldset>	
</div>