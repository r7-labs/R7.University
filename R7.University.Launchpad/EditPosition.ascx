﻿<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditPosition.ascx.cs" Inherits="R7.University.Launchpad.EditPosition" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="controls" TagName="AgplFooter" Src="~/DesktopModules/R7.University/R7.University/Controls/AgplFooter.ascx" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/module.css" />

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
		<li><asp:LinkButton id="buttonUpdate" runat="server" CssClass="dnnPrimaryAction" resourcekey="cmdUpdate" CausesValidation="true" OnClick="buttonUpdate_Click" /></li>
		<li><asp:LinkButton id="buttonDelete" runat="server" CssClass="dnnSecondaryAction" resourcekey="cmdDelete" OnClick="buttonDelete_Click" /></li>
		<li><asp:HyperLink id="linkCancel" runat="server" CssClass="dnnSecondaryAction" resourcekey="cmdCancel" /></li>
	</ul>
	<controls:AgplFooter id="agplFooter" runat="server" />
</div>

