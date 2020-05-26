<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditPosition.ascx.cs" Inherits="R7.University.Launchpad.EditPosition" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="AgplSignature" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/AgplSignature.ascx" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/assets/css/module.css" />

<div class="dnnForm dnnClear">
	<br /><br />
	<fieldset>	
		<div class="dnnFormItem">
			<dnn:Label id="lblTitle" runat="server" ControlName="txtTitle" />
			<asp:TextBox id="txtTitle" runat="server" MaxLength="100" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="lblShortTitle" runat="server" ControlName="txtShortTitle" />
			<asp:TextBox id="txtShortTitle" runat="server" MaxLength="64" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="lblWeight" runat="server" ControlName="lblWeight" />
			<asp:TextBox id="txtWeight" runat="server" Value="0" />
            <asp:RegularExpressionValidator runat="server" resourcekey="Weight.Invalid"
                ControlToValidate="txtWeight" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" 
                ValidationExpression="^-?\d+$" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="labelIsTeacher" runat="server" ControlName="checkIsTeacher" />
			<asp:CheckBox id="checkIsTeacher" runat="server" />
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

