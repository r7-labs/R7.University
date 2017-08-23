<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditDivisions.ascx.cs" Inherits="R7.University.Controls.EditDivisions" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="DivisionSelector" Src="~/DesktopModules/MVC/R7.University/R7.University/Controls/DivisionSelector.ascx" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/css/admin.css" Priority="200" />
<div class="dnnForm dnnClear u8y-edit-divisions">
    <fieldset>
        <div class="dnnFormItem">
            <asp:GridView id="gridDivisions" runat="server" AutoGenerateColumns="false" CssClass="dnnGrid"
                GridLines="None" Style="width:100%;margin-bottom:30px">
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
                            <span style="white-space:nowrap">
                                <asp:LinkButton id="linkEdit" runat="server" OnCommand="OnEditItemCommand" >
                                    <asp:Image runat="server" ImageUrl="<%# EditIconUrl %>" />
                                </asp:LinkButton>
                                <asp:LinkButton id="linkDelete" runat="server" OnCommand="OnDeleteItemCommand" >
                                    <asp:Image runat="server" ImageUrl="<%# DeleteIconUrl %>" />
                                </asp:LinkButton>
                                <asp:LinkButton id="linkUndelete" runat="server" OnCommand="OnUndeleteItemCommand" >
                                    <asp:Image runat="server" ImageUrl="<%# UndeleteIconUrl %>" />
                                </asp:LinkButton>
								<asp:Label id="labelEditMarker" runat="server" />
                            </span>
                       </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ViewItemID" />
                    <asp:BoundField DataField="DivisionTitle" HeaderText="DivisionTitle" />
                    <asp:BoundField DataField="DivisionRole" HeaderText="DivisionRole" />
                </Columns>
            </asp:GridView>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="labelDivision" runat="server" ControlName="divisionSelector" />
            <controls:DivisionSelector id="divisionSelector" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="labelDivisionRole" runat="server" ControlName="textDivisionRole" />
            <asp:TextBox id="textDivisionRole" runat="server" />
        </div>
	    <div class="dnnFormItem">
            <div class="dnnLabel"></div>
			<ul class="dnnActions">
				<li>
                    <asp:LinkButton id="buttonAddDivision" runat="server" resourcekey="buttonAddDivision" 
                        CssClass="dnnPrimaryAction" CommandArgument="Add" 
                        CausesValidation="true" ValidationGroup="EduProgramDivisions" />
				</li>	
                <li>
				    <asp:LinkButton id="buttonUpdateDivision" runat="server" resourcekey="buttonUpdateDivision" 
                        CssClass="dnnPrimaryAction" CommandArgument="Update" 
                        CausesValidation="true" ValidationGroup="EduProgramDivisions" />
				</li>
                <li>
				    <asp:LinkButton id="buttonCancelEditDivision" runat="server" resourcekey="CancelEdit" 
                        CssClass="dnnSecondaryAction" />
				</li>
				<li>&nbsp;</li>
    			<li>
					<asp:LinkButton id="buttonResetForm" runat="server" resourcekey="ResetForm" 
                        CssClass="dnnSecondaryAction" />
				</li>
			</ul>	
        </div>
        <asp:HiddenField id="hiddenDivisionItemID" runat="server" />
    </fieldset>
</div>
