<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsEmployeeList.ascx.cs" Inherits="R7.University.EmployeeList.SettingsEmployeeList" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University.EmployeeList/admin.css" Priority="200" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/admin.css" />
<div class="dnnForm dnnClear">
	<h2 class="dnnFormSectionHead"><a href=""><asp:Label runat="server" ResourceKey="sectionBaseSettings.Text" /></a></h2>
	<fieldset>	
		<div class="dnnFormItem">
			<dnn:Label id="labelDivision" runat="server" ControlName="treeDivisions" />
			<dnn:DnnTreeView id="treeDivisions" runat="server"
				DataFieldID="DivisionID"
				DataFieldParentID="ParentDivisionID"
				DataValueField="DivisionID"
				DataTextField="Title"
			/> 
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="labelIncludeSubdivisions" runat="server" ControlName="checkIncludeSubdivisions" />
			<asp:CheckBox id="checkIncludeSubdivisions" runat="server" />
		</div>
        <div class="dnnFormItem">
            <dnn:Label id="labelHideHeadEmployee" runat="server" ControlName="checkHideHeadEmployee" />
            <asp:CheckBox id="checkHideHeadEmployee" runat="server" />
        </div>
		<div class="dnnFormItem">
			<dnn:Label id="labelSortType" runat="server" ControlName="comboSortType" />
			<asp:DropDownList id="comboSortType" runat="server"/>
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="labelPhotoWidth" runat="server" ControlName="textPhotoWidth" />
			<asp:TextBox id="textPhotoWidth" runat="server" />
            <asp:RangeValidator runat="server" resourcekey="PhotoWidth.Invalid"
                ControlToValidate="textPhotoWidth" Type="Integer" MinimumValue="1" MaximumValue="1024"
                Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
            <asp:RequiredFieldValidator runat="server" resourcekey="PhotoWidth.Required"
                ControlToValidate="textPhotoWidth" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
        </div>
	</fieldset>	
</div>