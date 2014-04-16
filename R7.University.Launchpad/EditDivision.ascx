<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditDivision.ascx.cs" Inherits="R7.University.Launchpad.EditDivision" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Audit" Src="~/controls/ModuleAuditControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Url" Src="~/controls/URLControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>

<div class="dnnForm dnnClear">
	<br /><br />
	<fieldset>	
		<div class="dnnFormItem">
			<dnn:Label id="lblTitle" runat="server" ControlName="txtTitle" Suffix=":" />
			<asp:TextBox id="txtTitle" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="lblShortTitle" runat="server" ControlName="txtShortTitle" Suffix=":" />
			<asp:TextBox id="txtShortTitle" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="lblParentDivision" runat="server" ControlName="comboParentDivisions" Suffix=":" />
			<dnn:DnnComboBox id="comboParentDivisions" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="lblDivisionTerm" runat="server" ControlName="tsDivisionTerm" Suffix=":" />
			<dnn:DnnTreeView ID="treeDivisionTerms" runat="server" Style="float:left;display:block;margin-bottom:10px;padding:10px;background-color:#EEE" />
			<br /><br />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="lblHomePage" runat="server" ControlName="urlHomePage" Suffix=":" />
			<dnn:Url id="urlHomePage" runat="server" UrlType="T" 
			        ShowFiles="false" ShowTabs="true"
			        ShowUrls="true" ShowUsers="false"
					ShowLog="false" ShowTrack="false"
					ShowNone="true" ShowNewWindow="false" />      
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="lblWebSite" runat="server" ControlName="txtWebSite" Suffix=":" />
			<asp:TextBox id="txtWebSite" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="lblPhone" runat="server" ControlName="txtPhone" Suffix=":" />
			<asp:TextBox id="txtPhone" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="lblFax" runat="server" ControlName="txtFax" Suffix=":" />
			<asp:TextBox id="txtFax" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="lblEmail" runat="server" ControlName="txtEmail" Suffix=":" />
			<asp:TextBox id="txtEmail" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="lblSecondaryEmail" runat="server" ControlName="txtSecondaryEmail" Suffix=":" />
			<asp:TextBox id="txtSecondaryEmail" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="lblLocation" runat="server" ControlName="txtLocation" Suffix=":" />
			<asp:TextBox id="txtLocation" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="lblWorkingHours" runat="server" ControlName="txtWorkingHours" Suffix=":" />
			<asp:TextBox id="txtWorkingHours" runat="server" />
		</div>
	</fieldset>
	<ul class="dnnActions dnnClear">
		<li><asp:LinkButton id="buttonUpdate" runat="server" CssClass="dnnPrimaryAction" ResourceKey="cmdUpdate" CausesValidation="true" /></li>
		<li><asp:LinkButton id="buttonDelete" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdDelete" /></li>
		<li><asp:HyperLink id="linkCancel" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdCancel" /></li>
	</ul>
	<hr />
	<dnn:Audit id="ctlAudit" runat="server" />	
</div>

