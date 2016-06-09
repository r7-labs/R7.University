<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditEduLevel.ascx.cs" Inherits="R7.University.Launchpad.EditEduLevel" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>

<div class="dnnForm dnnClear">
	<fieldset>	
        <div class="dnnFormItem">
            <dnn:Label id="labelParentEduLevel" runat="server" ControlName="comboParentEduLevel" />
            <asp:DropDownList id="comboParentEduLevel" runat="server"
                DataValueField="EduLevelID"
                DataTextField="Title" />
        </div>
		<div class="dnnFormItem">
			<dnn:Label ID="labelTitle" runat="server" ControlName="textTitle" />
			<asp:TextBox ID="textTitle" runat="server" MaxLength="250" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="labelShortTitle" runat="server" ControlName="textShortTitle" />
			<asp:TextBox ID="textShortTitle" runat="server" MaxLength="64" />
		</div>
        <div class="dnnFormItem">
            <dnn:Label id="labelSortIndex" runat="server" ControlName="textSortIndex" />
            <asp:TextBox id="textSortIndex" runat="server" />
            <asp:RegularExpressionValidator runat="server" resourcekey="SortIndex.Invalid"
                ControlToValidate="textSortIndex" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" 
                ValidationExpression="^-?\d+$" />
        </div>
	</fieldset>
	<ul class="dnnActions dnnClear">
		<li><asp:LinkButton id="buttonUpdate" runat="server" CssClass="dnnPrimaryAction" ResourceKey="cmdUpdate" CausesValidation="true" /></li>
		<li><asp:LinkButton id="buttonDelete" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdDelete" /></li>
		<li><asp:HyperLink id="linkCancel" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdCancel" /></li>
	</ul>
</div>

