<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsDivisionDirectory.ascx.cs" Inherits="R7.University.DivisionDirectory.SettingsDivisionDirectory" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University.DivisionDirectory/admin.css" Priority="200" />

<div class="dnnForm dnnClear">
    <fieldset>  
        <div class="dnnFormItem">
            <dnn:Label id="labelMode" runat="server" ControlName="comboMode" />
            <asp:DropDownList id="comboMode" runat="server" />
        </div>
    </fieldset>
</div>
