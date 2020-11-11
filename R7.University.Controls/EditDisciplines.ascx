<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditDisciplines.ascx.cs" Inherits="R7.University.Controls.EditDisciplines" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/assets/css/admin.css" Priority="200" />
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
                    <asp:BoundField DataField="EduProfileString" HeaderText="EduProgramProfile.Column" />
                    <asp:BoundField DataField="EduLevelString" HeaderText="EduLevel.Column" />
                    <asp:BoundField DataField="Disciplines" HeaderText="Disciplines.Column" />
                </Columns>
            </asp:GridView>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="lblEduProgram" runat="server" ControlName="ddlEduProgram" />
            <asp:DropDownList id="ddlEduProgram" runat="server" CssClass="dnn-select2"
                AutoPostBack="true"
                OnSelectedIndexChanged="ddlEduProgram_SelectedIndexChanged"
                DataValueField="EduProgramID"
                DataTextField="Title_String" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="labelEduProgramProfile" runat="server" CssClass="dnnFormRequired" ControlName="ddlEduProfile" />
            <asp:DropDownList id="ddlEduProfile" runat="server" CssClass="dnn-select2"
                DataValueField="EduProgramProfileID"
                DataTextField="Title_String" />
            <asp:CustomValidator runat="server" resourcekey="EduProgramProfile_Warning.Text"
				ControlToValidate="ddlEduProfile" ValidationGroup="Disciplines"
                Display="Dynamic" CssClass="dnnFormMessage dnnFormError"
			    EnableClientScript="true" ClientValidationFunction="eduProfileValidator.validate2" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="labelDisciplines" runat="server" CssClass="dnnFormRequired" ControlName="textDisciplines" />
            <asp:TextBox id="textDisciplines" runat="server" TextMode="MultiLine" Rows="7" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="textDisciplines" Display="Dynamic"
                CssClass="dnnFormMessage dnnFormError" ValidationGroup="Disciplines" resourcekey="Disciplines.Required" />
        </div>
        <div class="dnnFormItem">
            <div class="dnnLabel"></div>
			<ul class="dnnActions">
				<li>
                    <asp:LinkButton id="buttonAddItem" runat="server" resourcekey="buttonAddDiscipline"
                        CssClass="btn btn-sm btn-primary" CommandArgument="Add"
                        CausesValidation="true" ValidationGroup="Disciplines" />
				</li>
                <li>
				    <asp:LinkButton id="buttonUpdateItem" runat="server" resourcekey="buttonUpdateDiscipline"
                        CssClass="btn btn-sm btn-primary" CommandArgument="Update"
                        CausesValidation="true" ValidationGroup="Disciplines" />
				</li>
				<li>&nbsp;</li>
                <li>
				    <asp:LinkButton id="buttonCancelEditItem" runat="server" resourcekey="CancelEdit"
                        CssClass="btn btn-sm btn-outline-secondary" />
				</li>
				<li>&nbsp;</li>
    			<li>
					<asp:LinkButton id="buttonResetForm" runat="server" resourcekey="ResetForm"
                        CssClass="btn btn-sm btn-outline-secondary" />
				</li>
			</ul>
        </div>
		<asp:HiddenField id="hiddenViewItemID" runat="server" />
		<asp:HiddenField id="hiddenEduProgramProfileID" runat="server" />
    </fieldset>
</div>
