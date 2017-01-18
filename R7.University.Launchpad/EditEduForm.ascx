<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditEduForm.ascx.cs" Inherits="R7.University.Launchpad.EditEduForm" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="AgplSignature" Src="~/DesktopModules/R7.University/R7.University/Controls/AgplSignature.ascx" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/module.css" />

<div class="dnnForm dnnClear">
	<fieldset>	
		<div class="dnnFormItem">
			<dnn:Label id="labelTitle" runat="server" ControlName="textTitle" />
			<asp:TextBox id="textTitle" runat="server" MaxLength="250" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="labelShortTitle" runat="server" ControlName="textShortTitle" />
			<asp:TextBox id="textShortTitle" runat="server" MaxLength="64" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="labelIsSystem" runat="server" ControlName="checkIsSystem" />
            <asp:CheckBox id="checkIsSystem" runat="server" Enabled="false" />
		</div>
    </fieldset>
	<ul class="dnnActions dnnClear">
		<li><asp:LinkButton id="buttonUpdate" runat="server" CssClass="dnnPrimaryAction" ResourceKey="cmdUpdate" CausesValidation="true" /></li>
		<li><asp:LinkButton id="buttonDelete" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdDelete" /></li>
		<li><asp:HyperLink id="linkCancel" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdCancel" /></li>
	</ul>
    <controls:AgplSignature runat="server" ShowRule="false" />
</div>

