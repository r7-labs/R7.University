<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsEduProgramDirectory.ascx.cs" Inherits="R7.University.EduProgramDirectory.SettingsEduProgramDirectory" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University.EduProgramDirectory/admin.css" Priority="200" />
<div class="dnnForm dnnClear">
	<h2 class="dnnFormSectionHead"><a href=""><asp:Label runat="server" ResourceKey="sectionBaseSettings.Text" /></a></h2>
    <fieldset>
		<div class="dnnFormItem">
			<dnn:Label id="labelDataCacheTime" runat="server" ControlName="textDataTime" Suffix=":" />
			<asp:TextBox id="textDataCacheTime" runat="server" Style="width:100px" />
		</div>
	</fieldset>	
</div>