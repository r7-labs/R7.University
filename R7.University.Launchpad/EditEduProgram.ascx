<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditEduProgram.ascx.cs" Inherits="R7.University.Launchpad.EditEduProgram" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>

<div class="dnnForm dnnClear">
	<fieldset>
		<div class="dnnFormItem">
            <dnn:Label ID="labelCode" runat="server" ControlName="textCode" />
            <asp:TextBox ID="textCode" runat="server" MaxLength="16" />
        </div>
        <div class="dnnFormItem">
			<dnn:Label ID="labelTitle" runat="server" ControlName="textTitle" />
			<asp:TextBox ID="textTitle" runat="server" MaxLength="250" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="labelEduLevel" runat="server" ControlName="comboEduLevel" />
			<asp:DropDownList id="comboEduLevel" runat="server" 
				DataTextField="DisplayShortTitle"
				DataValueField="EduLevelID"
			/>
		</div>
	</fieldset>
	<ul class="dnnActions dnnClear">
		<li><asp:LinkButton id="buttonUpdate" runat="server" CssClass="dnnPrimaryAction" ResourceKey="cmdUpdate" CausesValidation="true" OnClick="buttonUpdate_Click" /></li>
		<li><asp:LinkButton id="buttonDelete" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdDelete" OnClick="buttonDelete_Click" /></li>
		<li><asp:HyperLink id="linkCancel" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdCancel" /></li>
	</ul>
</div>

