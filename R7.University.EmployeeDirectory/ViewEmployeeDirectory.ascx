<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ViewEmployeeDirectory.ascx.cs" Inherits="R7.University.EmployeeDirectory.ViewEmployeeDirectory" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnJsInclude runat="server" FilePath="dnn.jquery.js" PathNameAlias="SharedScripts" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University.EmployeeDirectory/js/tree.js" />

<div class="dnnForm dnnClear ViewEmployeeDirectory">
<fieldset>
    <div class="dnnFormItem dnnClear">
        <asp:Label id="labelInfo" runat="server" CssClass="dnnFormMessage dnnFormInfo" resourcekey="labelInfo.Text" />
        <asp:TextBox id="textSearch" runat="server" CssClass="textSearch" />
        <%-- <dnn:DnnComboBox id="comboDivisions" runat="server" DataTextField="Title" DataValueField="DivisionID" CssClass="comboDivisions" /> --%>
        <div id="wrapperDivisions">
            <a id="linkDivisions" class="dnnSecondaryAction linkDivisions" onclick="$('#hiddenDivisions').toggle ()"><%= LocalizeString ("SelectDivision.Text") %></a>
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
    </div>

    <asp:GridView id="gridEmployees" runat="server" AutoGenerateColumns="false" CssClass="dnnGrid gridEmployees"
    GridLines="None" OnRowDataBound="gridEmployees_RowDataBound">
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
                    <%# LocalizeString ("Name.Header") %>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:HyperLink id="linkName" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <%# LocalizeString ("Position.Header") %>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Literal id="literalPosition" runat="server" />
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
                    <%# LocalizeString ("WorkingPlace.Header") %>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Literal id="literalWorkingPlace" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
 </fieldset>
 </div>