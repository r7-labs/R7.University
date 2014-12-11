<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ViewDivisionDirectory.ascx.cs" Inherits="R7.University.DivisionDirectory.ViewDivisionDirectory" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnJsInclude runat="server" FilePath="dnn.jquery.js" PathNameAlias="SharedScripts" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University.DivisionDirectory/js/tree.js" />

<div class="dnnForm dnnClear divisionDirectory">
<fieldset>
    <asp:Panel runat="server" DefaultButton="linkSearch" CssClass="dnnFormItem dnnClear">
        <div class="wrapperSearchFlags">
            <asp:CheckBox id="checkIncludeSubdivisions" runat="server" resourcekey="checkIncludeSubdivisions.Text" />
        </div>
        <asp:TextBox id="textSearch" runat="server" MaxLength="50" CssClass="textSearch" />
        <div class="wrapperDivisions">
            <a id="linkDivisions" class="dnnSecondaryAction linkDivisions" onclick="$('#hiddenDivisions').toggle ()"><%= LocalizeString ("AllDivisions.Text") %></a>
            <div id="hiddenDivisions">
                <dnn:DnnTreeView id="treeDivisions" runat="server" 
                    OnClientLoad="ed_treeLoad"
                    OnClientNodeClicked="ed_treeNodeClicked" 
                    DataTextField="Title"
                    DataValueField="DivisionID"
                    DataFieldID = "DivisionID"
                    DataFieldParentID="ParentDivisionID"
                />
            </div>
        </div>
        <asp:LinkButton id="linkSearch" runat="server" resourcekey="linkSearch.Text" CssClass="dnnPrimaryAction linkSearch" OnClick="linkSearch_Click" />
    </asp:Panel>

    <asp:GridView id="gridDivisions" runat="server" Visible="false" AutoGenerateColumns="false" CssClass="dnnGrid gridDivisions"
        GridLines="None" OnRowDataBound="gridDivisions_RowDataBound">
        <HeaderStyle CssClass="dnnGridHeader" />
        <RowStyle CssClass="dnnGridItem" />
        <AlternatingRowStyle CssClass="dnnGridAltItem" />
        <SelectedRowStyle CssClass="dnnFormError" />
        <EditRowStyle CssClass="dnnFormInput" />
        <FooterStyle CssClass="dnnGridFooter" />
        <PagerStyle CssClass="dnnGridPager" />
        <Columns>
            <asp:TemplateField>
                <HeaderTemplate>
                    <%# LocalizeString ("Title.Header") %>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label id="labelTitle" runat="server" />
                    <asp:HyperLink id="linkTitle" runat="server" target="_blank" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <%# LocalizeString ("HeadEmployee.Header") %>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:HyperLink id="linkHeadEmployee" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                 <HeaderTemplate>
                    <%# LocalizeString ("Phone.Header") %>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Literal id="literalPhone" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                 <HeaderTemplate>
                    <%# LocalizeString ("Email.Header") %>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:HyperLink id="linkEmail" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                 <HeaderTemplate>
                    <%# LocalizeString ("Location.Header") %>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Literal id="literalLocation" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <%# LocalizeString ("Document.Header") %>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:HyperLink id="linkDocument" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
 </fieldset>
 </div>