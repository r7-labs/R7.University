<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsEduProgramDirectory.ascx.cs" Inherits="R7.University.EduProgramDirectory.SettingsEduProgramDirectory" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University.EduProgramDirectory/admin.css" Priority="200" />
<div class="dnnForm dnnClear eduprogram-directory-settings">
	<h2 class="dnnFormSectionHead"><a href=""><asp:Label runat="server" ResourceKey="sectionBaseSettings.Text" /></a></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label id="labelEduLevels" runat="server" ControlName="listEduLevels" />
            <dnn:DnnListBox id="listEduLevels" runat="server" CheckBoxes="true" />
        </div>
	</fieldset>	
</div>