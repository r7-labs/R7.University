<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsDivision.ascx.cs" Inherits="R7.University.Division.SettingsDivision" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>

<div class="dnnForm dnnClear">
	<h2 class="dnnFormSectionHead"><a href=""><asp:Label runat="server" ResourceKey="sectionBaseSettings.Text" /></a></h2>
	<fieldset>	
		<div class="dnnFormItem">
			<dnn:Label id="labelDivision" runat="server" ControlName="treeDivisions" Suffix=":" />
			<dnn:DnnTreeView id="treeDivisions" runat="server" Style="float:left;display:block;margin-bottom:10px;padding:10px;background-color:#EEE"
				DataFieldID="DivisionID"
				DataFieldParentID="ParentDivisionID"
				DataValueField="DivisionID"
				DataTextField="ShortTitle"
			/> 
		</div>
	</fieldset>	
</div>

