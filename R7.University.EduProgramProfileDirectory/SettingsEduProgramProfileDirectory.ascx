<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="SettingsEduProgramProfileDirectory.ascx.cs" 
    Inherits="R7.University.EduProgramProfileDirectory.SettingsEduProgramProfileDirectory" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnCssInclude runat="server" 
    FilePath="~/DesktopModules/R7.University/R7.University.EduProgramProfileDirectory/admin.css" Priority="200" />
<div class="dnnForm dnnClear eduprogramprofile-directory-settings">
	<h2 class="dnnFormSectionHead"><a href=""><asp:Label runat="server" ResourceKey="sectionBaseSettings.Text" /></a></h2>
    <fieldset>
    </fieldset>	
</div>