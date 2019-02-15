<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditYear.ascx.cs" Inherits="R7.University.Launchpad.EditYear" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="AgplSignature" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/AgplSignature.ascx" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/css/module.css" />

<div class="dnnForm dnnClear">
	<br /><br />
	<fieldset>	
		<div class="dnnFormItem dnnFormRequired">
			<dnn:Label id="labelYear" runat="server" ControlName="textYear" />
			<asp:TextBox id="textYear" runat="server" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="textYear" 
			    Display="Dynamic" CssClass="dnnFormMessage dnnFormError" resourcekey="Year.Required" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="labelAdmissionIsOpen" runat="server" ControlName="checkAdmissionIsOpen" />
			<asp:CheckBox id="checkAdmissionIsOpen" runat="server" Enabled="false" />
		</div>
	</fieldset>
	<ul class="dnnActions dnnClear">
		<li><asp:LinkButton id="buttonUpdate" runat="server" CssClass="dnnPrimaryAction" resourcekey="cmdUpdate" CausesValidation="true" /></li>
		<li>&nbsp;</li>
		<li><asp:LinkButton id="buttonDelete" runat="server" CssClass="dnnSecondaryAction" resourcekey="cmdDelete" /></li>
		<li>&nbsp;</li>
		<li><asp:HyperLink id="linkCancel" runat="server" CssClass="dnnSecondaryAction" resourcekey="cmdCancel" /></li>
	</ul>
    <controls:AgplSignature runat="server" ShowRule="false" />
</div>

