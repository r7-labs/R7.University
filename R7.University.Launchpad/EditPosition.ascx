<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditPosition.ascx.cs" Inherits="R7.University.Launchpad.EditPosition" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>

<div class="dnnForm dnnClear">
	<br /><br />
	<fieldset>	
		<div class="dnnFormItem">
			<dnn:Label ID="lblTitle" runat="server" ControlName="txtTitle" Suffix=":" />
			<asp:TextBox ID="txtTitle" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="lblShortTitle" runat="server" ControlName="txtShortTitle" Suffix=":" />
			<asp:TextBox ID="txtShortTitle" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="lblWeight" runat="server" ControlName="lblWeight" Suffix=":" />
			<asp:TextBox ID="txtWeight" runat="server" />
		</div>
	</fieldset>
	<ul class="dnnActions dnnClear">
		<li><asp:LinkButton id="buttonUpdate" runat="server" CssClass="dnnPrimaryAction" ResourceKey="cmdUpdate" CausesValidation="true" /></li>
		<li><asp:LinkButton id="buttonDelete" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdDelete" /></li>
		<li><asp:HyperLink id="linkCancel" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdCancel" /></li>
	</ul>
</div>

