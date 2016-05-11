<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditEduProgram.ascx.cs" Inherits="R7.University.Launchpad.EditEduProgram" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Audit" Src="~/controls/ModuleAuditControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="controls" TagName="EditDocuments" Src="../R7.University/Controls/EditDocuments.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/admin.css" Priority="200" />
<script type="text/javascript">
    $(function() { $("#eduProgramTabs").dnnTabs({selected: <%= (int)SelectedTab %>}); });
</script>
<div class="dnnForm dnnClear">
    <div id="eduProgramTabs" class="dnnForm dnnClear">
        <ul class="dnnAdminTabNav dnnClear">
            <li><a href="#eduProgramCommon"><%= LocalizeString ("CommonTab.Text") %></a></li>
            <li><a href="#eduProgramDocuments"><%= LocalizeString ("DocumentsTab.Text") %></a></li>
        </ul>
        <asp:ValidationSummary runat="server" CssClass="dnnFormMessage dnnFormError" />
        <div id="eduProgramCommon">
        	<fieldset>
        		<div class="dnnFormItem">
                    <dnn:Label ID="labelCode" runat="server" ControlName="textCode" />
                    <asp:TextBox ID="textCode" runat="server" MaxLength="64" />
                </div>
                <div class="dnnFormItem">
        			<dnn:Label ID="labelTitle" runat="server" ControlName="textTitle" />
        			<asp:TextBox ID="textTitle" runat="server" MaxLength="250" />
        		</div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelEduLevel" runat="server" ControlName="comboEduLevel" />
                    <asp:DropDownList id="comboEduLevel" runat="server" 
                        DataTextField="DisplayShortTitle"
                        DataValueField="EduLevelID"
                    />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="labelGeneration" runat="server" ControlName="textGeneration" />
                    <asp:TextBox ID="textGeneration" runat="server" MaxLength="16" />
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
        </div>
        <div id="eduProgramDocuments">
            <controls:EditDocuments id="formEditDocuments" runat="server" ForModel="EduProgram" />
        </div>
        <ul class="dnnActions dnnClear">
            <li><asp:LinkButton id="buttonUpdate" runat="server" CssClass="dnnPrimaryAction" ResourceKey="cmdUpdate" CausesValidation="true" OnClick="buttonUpdate_Click" /></li>
            <li><asp:LinkButton id="buttonDelete" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdDelete" OnClick="buttonDelete_Click" /></li>
            <li><asp:HyperLink id="linkCancel" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdCancel" /></li>
        </ul>
        <hr />
        <dnn:Audit id="auditControl" runat="server" />
    </div>
</div>
