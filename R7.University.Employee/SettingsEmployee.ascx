<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsEmployee.ascx.cs" Inherits="R7.University.Employee.SettingsEmployee" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="act" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University.Employee/admin.css" Priority="200" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/act.css" />

<div class="dnnForm dnnClear">
	<h2 class="dnnFormSectionHead"><a href=""><asp:Label runat="server" ResourceKey="sectionBaseSettings.Text" /></a></h2>
	<fieldset>	
		<div class="dnnFormItem">
			<dnn:Label id="labelEmployee" runat="server" ControlName="comboEmployees" />
            <asp:UpdatePanel id="updatePanelEmployees" runat="server">
                <ContentTemplate>
                    <act:ComboBox id="comboEmployees" runat="server" CssClass="act_combobox"
                        DropDownStyle="DropDownList"
                        AutoCompleteMode="SuggestAppend"
                        CaseSensitive="false"
                        DataValueField="EmployeeID"
                        DataTextField="AbbrName"
                    />
                </ContentTemplate>
            </asp:UpdatePanel>
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="labelPhotoWidth" runat="server" ControlName="textPhotoWidth" />
			<asp:TextBox id="textPhotoWidth" runat="server" Style="width:100px" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="labelShowCurrentUser" runat="server" ControlName="checkShowCurrentUser" />
			<asp:CheckBox id="checkShowCurrentUser" runat="server" Checked="false" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="labelAutoTitle" runat="server" ControlName="checkAutoTitle" />
			<asp:CheckBox id="checkAutoTitle" runat="server" Checked="true" />
		</div>
	</fieldset>	
	<h2 id="panelDataCache" class="dnnFormSectionHead">
		<a href="" class="dnnSectionExpanded"><%= LocalizeString("DataCache") %></a>
	</h2>
	<fieldset>
		<div class="dnnFormItem">
			<asp:Label id="labelDataCacheInfo" runat="server" resourcekey="labelDataCacheInfo" CssClass="dnnFormMessage" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="labelDataCacheTime" runat="server" ControlName="textDataTime" />
			<asp:TextBox id="textDataCacheTime" runat="server" Style="width:100px" />
		</div>
	</fieldset>	
</div>
