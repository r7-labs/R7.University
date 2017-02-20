<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditEduProgram.ascx.cs" Inherits="R7.University.EduProgram.EditEduProgram" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Audit" Src="~/controls/ModuleAuditControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Url" Src="~/controls/DnnUrlControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="EditDocuments" Src="~/DesktopModules/R7.University/R7.University/Controls/EditDocuments.ascx" %>
<%@ Register TagPrefix="controls" TagName="AgplSignature" Src="~/DesktopModules/R7.University/R7.University/Controls/AgplSignature.ascx" %>
<%@ Register TagPrefix="controls" TagName="DivisionSelector" Src="~/DesktopModules/R7.University/R7.University/Controls/DivisionSelector.ascx" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/module.css" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/admin.css" Priority="200" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/dnn-ac-combobox.css" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/js/dnn-ac-combobox.js" />

<div class="dnnForm dnnClear">
    <div id="eduprogram-tabs" class="dnnForm dnnClear">
        <ul class="dnnAdminTabNav dnnClear">
            <li><a href="#eduprogram-common"><%= LocalizeString ("Common.Tab") %></a></li>
			<li><a href="#eduprogram-profiles"><%= LocalizeString ("EduProgramProfiles.Tab") %></a></li>
            <li><a href="#eduprogram-bindings"><%= LocalizeString ("Bindings.Tab") %></a></li>
            <li><a href="#eduprogram-documents"><%= LocalizeString ("Documents.Tab") %></a></li>
        </ul>
        <div id="eduprogram-common">
        	<fieldset>
        		<div class="dnnFormItem dnnFormRequired">
                    <dnn:Label ID="labelCode" runat="server" ControlName="textCode" />
                    <asp:TextBox ID="textCode" runat="server" MaxLength="64" />
                    <asp:RequiredFieldValidator runat="server" resourcekey="Code.Required"
                        ControlToValidate="textCode" ValidationGroup="EduProgram" 
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
                </div>
                <div class="dnnFormItem dnnFormRequired">
        			<dnn:Label ID="labelTitle" runat="server" ControlName="textTitle" />
        			<asp:TextBox ID="textTitle" runat="server" MaxLength="250" />
                    <asp:RequiredFieldValidator runat="server" resourcekey="Title.Required"
                        ControlToValidate="textTitle" ValidationGroup="EduProgram" 
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
        		</div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelEduLevel" runat="server" ControlName="comboEduLevel" />
                    <asp:DropDownList id="comboEduLevel" runat="server" 
                        DataTextField="Title"
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
		<div id="eduprogram-profiles">
            <fieldset>
                <div class="dnnFormItem">
					<asp:GridView id="gridEduProgramProfiles" runat="server" AutoGenerateColumns="false" 
							OnRowDataBound="gridEduProgramProfiles_RowDataBound"
                            GridLines="None" CssClass="dnnGrid" Style="width:100%;margin-bottom:30px">
                        <HeaderStyle CssClass="dnnGridHeader" HorizontalAlign="Left" />
                        <RowStyle CssClass="dnnGridItem" HorizontalAlign="Left" />
                        <AlternatingRowStyle CssClass="dnnGridAltItem" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HyperLink runat="server" IconKey="Edit" NavigateUrl='<%# Eval ("Edit_Url") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="EduProgramProfile_String" HeaderText="EduProgramProfile" />
                            <asp:BoundField DataField="EduLevel_String" HeaderText="EduLevel" />
							<asp:BoundField DataField="StartDate" HeaderText="StartDate" DataFormatString="{0:d}" />
							<asp:BoundField DataField="EndDate" HeaderText="EndDate" DataFormatString="{0:d}" />
                        </Columns>
                    </asp:GridView>
			    </div>
				<div class="dnnFormItem">
					<label class="dnnLabel" />
					<asp:HyperLink id="linkAddEduProgramProfile" runat="server" CssClass="dnnPrimaryAction" resourcekey="AddEduProgramProfile.Text" />
                </div>
				<asp:Panel id="panelAddDefaultProfile" runat="server" CssClass="dnnFormItem">
                    <dnn:Label id="labelAddDefaultProfile" runat="server" ControlName="checkAddDefaultProfile" />
                    <asp:CheckBox id="checkAddDefaultProfile" runat="server" Checked="true" />
                </asp:Panel>
			</fieldset>	
        </div>
        <div id="eduprogram-bindings">
            <fieldset>
                <div class="dnnFormItem">
                    <dnn:Label id="labelDivision" runat="server" ControlName="divisionSelector" />
					<controls:DivisionSelector id="divisionSelector" runat="server" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label id="labelHomePage" runat="server" ControlName="urlHomePage" />
                    <dnn:Url id="urlHomePage" runat="server" UrlType="T" 
                            IncludeActiveTab="true"
                            ShowFiles="false" ShowTabs="true"
                            ShowUrls="false" ShowUsers="false"
                            ShowLog="false" ShowTrack="false"
                            ShowNone="true" ShowNewWindow="false" />      
                </div>
            </fieldset>
        </div>
        <div id="eduprogram-documents">
            <controls:EditDocuments id="formEditDocuments" runat="server" ForModel="EduProgram" />
        </div>
		<ul class="dnnActions dnnClear">
            <li><asp:LinkButton id="buttonUpdate" runat="server" CssClass="dnnPrimaryAction" ResourceKey="cmdUpdate" CausesValidation="true" ValidationGroup="EduProgram" /></li>
            <li><asp:LinkButton id="buttonDelete" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdDelete" /></li>
            <li><asp:HyperLink id="linkCancel" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdCancel" /></li>
        </ul>
		<controls:AgplSignature runat="server" ShowRule="false" />
        <hr />
        <dnn:Audit id="auditControl" runat="server" />
    </div>
</div>
<input id="hiddenSelectedTab" type="hidden" value="<%= (int) SelectedTab %>" />

<script type="text/javascript">
(function($, Sys) {
    function setupModule() {
        var selectedTab = document.getElementById("hiddenSelectedTab").value;
        $("#eduprogram-tabs").dnnTabs(selectedTab);
	    dnnAcCombobox_Init($);
        $(".dnn-ac-combobox").combobox();
    };
    $(document).ready(function() {
        setupModule();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function() {
            setupModule();
        });
    });
} (jQuery, window.Sys));
</script>