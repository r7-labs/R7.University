<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditDocumentType.ascx.cs" Inherits="R7.University.Launchpad.EditDocumentType" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="controls" TagName="AgplFooter" Src="~/DesktopModules/R7.University/R7.University/Controls/AgplFooter.ascx" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/module.css" />

<div class="dnnForm dnnClear">
	<fieldset>	
		<div class="dnnFormItem">
			<dnn:Label id="labelType" runat="server" ControlName="textType" />
			<asp:TextBox id="textType" runat="server" MaxLength="64" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="textType" Display="Dynamic"
                CssClass="dnnFormMessage dnnFormError" resourcekey="Type.Required" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="labelDescription" runat="server" ControlName="textDescription" />
			<asp:TextBox id="textDescription" runat="server" TextMode="MultiLine" Rows="3" />
            <asp:RegularExpressionValidator runat="server"
                CssClass="dnnFormMessage dnnFormError" resourcekey="Description.MaxLength"
                ControlToValidate="textDescription" Display="Dynamic"
                ValidationExpression="[\s\S]{0,255}">
            </asp:RegularExpressionValidator>
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
	<controls:AgplFooter id="agplFooter" runat="server" />
</div>
