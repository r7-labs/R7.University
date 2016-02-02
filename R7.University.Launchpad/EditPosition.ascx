<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditPosition.ascx.cs" Inherits="R7.University.Launchpad.EditPosition" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>

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
			<asp:TextBox id="txtWeight" runat="server" />
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
</div>

