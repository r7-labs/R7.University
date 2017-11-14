<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditScienceRecordType.ascx.cs" Inherits="R7.University.Launchpad.EditScienceRecordType" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="AgplSignature" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/AgplSignature.ascx" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/css/module.css" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University.Launchpad/js/editDocumentType.js" ForceProvider="DnnFormBottomProvider" />

<div class="dnnForm dnnClear">
	<fieldset>	
		<div class="dnnFormItem dnnFormRequired">
			<dnn:Label id="labelType" runat="server" ControlName="textType" />
			<asp:TextBox id="textType" runat="server" MaxLength="64" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="textType" Display="Dynamic"
                CssClass="dnnFormMessage dnnFormError" resourcekey="Type.Required" />
		</div>
		<div class="dnnFormItem">
            <dnn:Label id="labelTypeOfValues" runat="server" ControlName="comboTypeOfValues" />
            <asp:DropDownList id="comboTypeOfValues" runat="server"
                DataTextField="ValueLocalized"
                DataValueField="Value"
            />
        </div>
		<div class="dnnFormItem dnnFormRequired">
            <dnn:Label id="labelNumOfValues" runat="server" ControlName="textNumOfValues" />
            <asp:TextBox id="textNumOfValues" runat="server" Value="0" />
			<asp:RequiredFieldValidator runat="server" resourcekey="NumOfValues.Required"
                ControlToValidate="textNumOfValues" 
				Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
			<asp:RangeValidator runat="server" resourcekey="NumOfValues.Invalid"
                ControlToValidate="textNumOfValues" 
				Type="Integer" MinimumValue="0" MaximumValue="2"
                Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
		</div>
		<div class="dnnFormItem dnnFormRequired">
            <dnn:Label id="labelSortIndex" runat="server" ControlName="textSortIndex" />
            <asp:TextBox id="textSortIndex" runat="server" Value="0" />
			<asp:RequiredFieldValidator runat="server" resourcekey="SortIndex.Required"
                ControlToValidate="textSortIndex" 
				Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
            <asp:RegularExpressionValidator runat="server" resourcekey="SortIndex.Invalid"
                ControlToValidate="textSortIndex" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" 
                ValidationExpression="^-?\d+$" />
        </div>
		<div class="dnnFormItem">
            <dnn:Label id="labelDescriptionIsRequired" runat="server" ControlName="checkDescriptionIsRequired" />
            <asp:CheckBox id="checkDescriptionIsRequired" runat="server" />
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
