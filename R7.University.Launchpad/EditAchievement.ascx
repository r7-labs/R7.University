<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditAchievement.ascx.cs" Inherits="R7.University.Launchpad.EditAchievement" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>

<div class="dnnForm dnnClear">
	<br /><br />
	<fieldset>	
		<div class="dnnFormItem">
			<dnn:Label ID="labelTitle" runat="server" ControlName="textTitle" Suffix=":" />
			<asp:TextBox ID="textTitle" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="labelShortTitle" runat="server" ControlName="textShortTitle" Suffix=":" />
			<asp:TextBox ID="textShortTitle" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="labelAchievementTypes" runat="server" ControlName="comboAchievementTypes" Suffix=":" />
			<dnn:DnnComboBox id="comboAchievementTypes" runat="server" 
				DataTextField="LocalizedAchivementType"
				DataValueField="AchievementType"
			/>
		</div>
	</fieldset>
	<ul class="dnnActions dnnClear">
		<li><asp:LinkButton id="buttonUpdate" runat="server" CssClass="dnnPrimaryAction" ResourceKey="cmdUpdate" CausesValidation="true" /></li>
		<li><asp:LinkButton id="buttonDelete" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdDelete" /></li>
		<li><asp:HyperLink id="linkCancel" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdCancel" /></li>
	</ul>
</div>

