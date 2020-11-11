﻿<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditAchievement.ascx.cs" Inherits="R7.University.Launchpad.EditAchievement" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="AgplSignature" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/AgplSignature.ascx" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/assets/css/module.css" />

<div class="dnnForm dnnClear">
	<br /><br />
	<fieldset>
		<div class="dnnFormItem">
			<dnn:Label ID="labelTitle" runat="server" ControlName="textTitle" />
			<asp:TextBox ID="textTitle" runat="server" MaxLength="250" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="labelShortTitle" runat="server" ControlName="textShortTitle" />
			<asp:TextBox ID="textShortTitle" runat="server" MaxLength="64" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="labelAchievementTypes" runat="server" ControlName="comboAchievementType" />
			<asp:DropDownList id="comboAchievementType" runat="server"
				DataTextField="Text"
				DataValueField="Value"
			/>
		</div>
	</fieldset>
	<ul class="dnnActions dnnClear">
		<li><asp:LinkButton id="buttonUpdate" runat="server" CssClass="btn btn-primary" ResourceKey="cmdUpdate" CausesValidation="true" /></li>
		<li>&nbsp;</li>
		<li><asp:LinkButton id="buttonDelete" runat="server" CssClass="btn btn-danger" ResourceKey="cmdDelete" /></li>
		<li>&nbsp;</li>
		<li><asp:HyperLink id="linkCancel" runat="server" CssClass="btn btn-outline-secondary" ResourceKey="cmdCancel" /></li>
	</ul>
	<controls:AgplSignature runat="server" ShowRule="false" />
</div>
