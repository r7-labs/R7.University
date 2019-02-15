<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditDisciplines.ascx.cs" Inherits="R7.University.Controls.EditDisciplines" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/css/admin.css" Priority="200" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University.Controls/css/grid-and-form.css" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University.Controls/js/gridAndForm.js" ForceProvider="DnnFormBottomProvider" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University.Controls/js/editDisciplines.js" ForceProvider="DnnFormBottomProvider" />
<div class="dnnForm dnnClear u8y-edit-divisions">
    <fieldset>
        <div class="dnnFormItem">
            <asp:GridView id="gridItems" runat="server" AutoGenerateColumns="false" CssClass="dnnGrid u8y-gaf-grid" GridLines="None">
                <HeaderStyle CssClass="dnnGridHeader" HorizontalAlign="Left" />
                <RowStyle CssClass="dnnGridItem" HorizontalAlign="Left" />
                <AlternatingRowStyle CssClass="dnnGridAltItem" />
                <SelectedRowStyle CssClass="dnnFormError" />
                <EditRowStyle CssClass="dnnFormInput" />
                <FooterStyle CssClass="dnnGridFooter" />
                <PagerStyle CssClass="dnnGridPager" />
                <Columns>
                    <asp:TemplateField>
                       <ItemTemplate>
                            <span class="u8y-gaf-actions">
                                <asp:LinkButton id="linkEdit" runat="server" OnCommand="OnEditItemCommand" ToolTip='<%# LocalizeString ("EditItem.Title") %>'>
                                    <asp:Image runat="server" ImageUrl="<%# EditIconUrl %>" />
                                </asp:LinkButton>
                                <asp:LinkButton id="linkDelete" runat="server" OnCommand="OnDeleteItemCommand" ToolTip='<%# LocalizeString ("DeleteItem.Title") %>'>
                                    <asp:Image runat="server" ImageUrl="<%# DeleteIconUrl %>" />
                                </asp:LinkButton>
                                <asp:LinkButton id="linkUndelete" runat="server" OnCommand="OnUndeleteItemCommand" ToolTip='<%# LocalizeString ("UndeleteItem.Title") %>'>
                                    <asp:Image runat="server" ImageUrl="<%# UndeleteIconUrl %>" />
                                </asp:LinkButton>
                                <asp:Label id="labelEditMarker" runat="server" />
                            </span>
                       </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ViewItemID" />
                    <asp:BoundField DataField="EduProgramProfileString" HeaderText="EduProgramProfile.Column" />
                    <asp:BoundField DataField="EduLevelString" HeaderText="EduLevel.Column" />
                    <asp:BoundField DataField="Disciplines" HeaderText="Disciplines.Column" />
                </Columns>
            </asp:GridView>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="labelEduLevel" runat="server" ControlName="comboEduLevel" />
            <asp:DropDownList id="comboEduLevel" runat="server"
                AutoPostBack="true"
                OnSelectedIndexChanged="comboEduLevel_SelectedIndexChanged"
                DataValueField="EduLevelID"
                DataTextField="Title" />
        </div>
        <div class="dnnFormItem dnnFormRequired">
            <dnn:Label id="labelEduProgramProfile" runat="server" ControlName="comboEduProgramProfile" />
            <asp:DropDownList id="comboEduProgramProfile" runat="server" CssClass="dnn-ac-combobox"
                DataValueField="EduProgramProfileID"
                DataTextField="Title_String" />
			<asp:RequiredFieldValidator runat="server" ControlToValidate="comboEduProgramProfile" Display="Dynamic"
                CssClass="dnnFormMessage dnnFormError" ValidationGroup="Disciplines" resourcekey="EduProgramProfile.Required" />
            <asp:CustomValidator runat="server" resourcekey="EduProgramProfile.Warning"
				ControlToValidate="comboEduProgramProfile" ValidationGroup="Disciplines" 
                Display="Dynamic" CssClass="dnnFormMessage dnnFormError"
			    EnableClientScript="true" ClientValidationFunction="eduProgramProfileUniqueValidator.validate" />
        </div>
        <div class="dnnFormItem dnnFormRequired">
            <dnn:Label id="labelDisciplines" runat="server" ControlName="textDisciplines" />
            <asp:TextBox id="textDisciplines" runat="server" TextMode="MultiLine" Rows="7" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="textDisciplines" Display="Dynamic"
                CssClass="dnnFormMessage dnnFormError" ValidationGroup="Disciplines" resourcekey="Disciplines.Required" />
        </div>
        <div class="dnnFormItem">
            <div class="dnnLabel"></div>
			<ul class="dnnActions">
				<li>
                    <asp:LinkButton id="buttonAddItem" runat="server" resourcekey="buttonAddDiscipline" 
                        CssClass="dnnPrimaryAction" CommandArgument="Add" 
                        CausesValidation="true" ValidationGroup="Disciplines" />
				</li>	
                <li>
				    <asp:LinkButton id="buttonUpdateItem" runat="server" resourcekey="buttonUpdateDiscipline" 
                        CssClass="dnnPrimaryAction" CommandArgument="Update" 
                        CausesValidation="true" ValidationGroup="Disciplines" />
				</li>
				<li>&nbsp;</li>
                <li>
				    <asp:LinkButton id="buttonCancelEditItem" runat="server" resourcekey="CancelEdit" 
                        CssClass="dnnSecondaryAction" />
				</li>
				<li>&nbsp;</li>
    			<li>
					<asp:LinkButton id="buttonResetForm" runat="server" resourcekey="ResetForm" 
                        CssClass="dnnSecondaryAction" />
				</li>
			</ul>	
        </div>
		<asp:HiddenField id="hiddenViewItemID" runat="server" />
		<asp:HiddenField id="hiddenEduProgramProfileID" runat="server" />
    </fieldset>
</div>
