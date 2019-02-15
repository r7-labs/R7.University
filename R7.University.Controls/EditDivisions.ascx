<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditDivisions.ascx.cs" Inherits="R7.University.Controls.EditDivisions" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="DivisionSelector" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/DivisionSelector.ascx" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/css/admin.css" Priority="200" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University.Controls/css/grid-and-form.css" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University.Controls/js/gridAndForm.js" ForceProvider="DnnFormBottomProvider" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University.Controls/js/editDivisions.js" ForceProvider="DnnFormBottomProvider" />
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
                    <asp:BoundField DataField="DivisionTitle" HeaderText="DivisionTitle.Column" />
                    <asp:BoundField DataField="DivisionRole" HeaderText="DivisionRole.Column" />
                </Columns>
            </asp:GridView>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="labelDivision" runat="server" ControlName="divisionSelector" />
            <controls:DivisionSelector id="divisionSelector" runat="server" IsRequired="true" />
            <asp:CustomValidator runat="server" resourcekey="Division.Invalid" CssClass="dnnFormMessage dnnFormError"
                Display="Dynamic" EnableClientScript="true" ClientValidationFunction="divisionUniqueValidator.validate" ValidationGroup="EduProgramDivisions"/>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="labelDivisionRole" runat="server" ControlName="textDivisionRole" />
            <asp:TextBox id="textDivisionRole" runat="server" />
        </div>
	    <div class="dnnFormItem">
            <div class="dnnLabel"></div>
			<ul class="dnnActions">
				<li>
                    <asp:LinkButton id="buttonAddItem" runat="server" resourcekey="buttonAddDivision" 
                        CssClass="dnnPrimaryAction" CommandArgument="Add" 
                        CausesValidation="true" ValidationGroup="EduProgramDivisions" />
				</li>	
                <li>
				    <asp:LinkButton id="buttonUpdateItem" runat="server" resourcekey="buttonUpdateDivision" 
                        CssClass="dnnPrimaryAction" CommandArgument="Update" 
                        CausesValidation="true" ValidationGroup="EduProgramDivisions" />
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
		<asp:HiddenField id="hiddenDivisionID" runat="server" />
    </fieldset>
</div>
