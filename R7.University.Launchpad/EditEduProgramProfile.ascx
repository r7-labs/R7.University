<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditEduProgramProfile.ascx.cs" Inherits="R7.University.Launchpad.EditEduProgramProfile" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Audit" Src="~/controls/ModuleAuditControl.ascx" %>
<%@ Register TagPrefix="act" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/admin.css" Priority="200" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/act.css" />
<div class="dnnForm dnnClear">
	<fieldset>
        <div class="dnnFormItem">
            <dnn:Label id="labelEduProgram" runat="server" ControlName="comboEduProgram" />
            <asp:UpdatePanel id="updatePanelEduProgram" runat="server">
                <ContentTemplate>
                    <act:ComboBox id="comboEduProgram" runat="server" CssClass="act_combobox"
                        DropDownStyle="DropDownList"
                        AutoCompleteMode="SuggestAppend"
                        CaseSensitive="false"
                        DataValueField="EduProgramID"
                        DataTextField="EduProgramString"
                    />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
		<div class="dnnFormItem">
            <dnn:Label ID="labelProfileCode" runat="server" ControlName="textProfileCode" />
            <asp:TextBox ID="textProfileCode" runat="server" MaxLength="64" />
        </div>
        <div class="dnnFormItem">
			<dnn:Label ID="labelProfileTitle" runat="server" ControlName="textProfileTitle" />
			<asp:TextBox ID="textProfileTitle" runat="server" MaxLength="250" />
		</div>
        <div class="dnnFormItem">
            <dnn:Label ID="labelStartDate" runat="server" ControlName="datetimeStartDate" />
            <dnn:DnnDateTimePicker id="datetimeStartDate" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="labelEndDate" runat="server" ControlName="datetimeEndDate" />
            <dnn:DnnDateTimePicker id="datetimeEndDate" runat="server" />
        </div>
	</fieldset>
	<ul class="dnnActions dnnClear">
		<li><asp:LinkButton id="buttonUpdate" runat="server" CssClass="dnnPrimaryAction" ResourceKey="cmdUpdate" CausesValidation="true" OnClick="buttonUpdate_Click" /></li>
		<li><asp:LinkButton id="buttonDelete" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdDelete" OnClick="buttonDelete_Click" /></li>
		<li><asp:HyperLink id="linkCancel" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdCancel" /></li>
	</ul>
    <hr />
    <dnn:Audit id="auditControl" runat="server" />
</div>
