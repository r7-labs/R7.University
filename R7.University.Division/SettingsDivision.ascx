<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsDivision.ascx.cs" Inherits="R7.University.Division.SettingsDivision" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University.Division/admin.css" Priority="200" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/admin.css" />
<div class="dnnForm dnnClear">
	<h2 class="dnnFormSectionHead"><a href=""><asp:Label runat="server" ResourceKey="sectionBaseSettings.Text" /></a></h2>
	<fieldset>	
		<div class="dnnFormItem">
			<dnn:Label id="labelDivision" runat="server" ControlName="treeDivisions" Suffix=":" />
			<dnn:DnnTreeView id="treeDivisions" runat="server"
				DataFieldID="DivisionID"
				DataFieldParentID="ParentDivisionID"
				DataValueField="DivisionID"
				DataTextField="DisplayShortTitle"
			/> 
		</div>
	</fieldset>	
</div>

