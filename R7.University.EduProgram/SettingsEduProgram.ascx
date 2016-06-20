<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsEduProgram.ascx.cs" Inherits="R7.University.EduProgram.SettingsEduProgram" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University.EduProgram/R7.University.EduProgram/admin.css" Priority="200" />
<div class="dnnForm dnnClear">
 <h2 class="dnnFormSectionHead"><a href=""><asp:Label runat="server" ResourceKey="sectionBasicSettings.Text" /></a></h2>
 <fieldset>  
     <div class="dnnFormItem">
         <dnn:Label ID="lblTemplate" runat="server" ControlName="txtTemplate" />
         <asp:TextBox ID="txtTemplate" runat="server" CssClass="NormalTextBox" Rows="10" Columns="30" TextMode="MultiLine" MaxLength="2000" />
     </div>
 </fieldset> 
</div>

