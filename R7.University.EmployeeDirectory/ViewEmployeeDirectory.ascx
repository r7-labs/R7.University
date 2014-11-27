<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ViewEmployeeDirectory.ascx.cs" Inherits="R7.University.EmployeeDirectory.ViewEmployeeDirectory" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnJsInclude runat="server" FilePath="dnn.jquery.js" PathNameAlias="SharedScripts" />

<script type="text/javascript">
function ed_shorten (n, text) {
    return (text.length > n) ? text.substring (0, n-1) + "\u2026" : text;
}

function ed_treeNodeClicked (sender, eventArgs) {
    var nodeText = eventArgs.get_node ().get_text ();
    $("#linkDivisions").text (ed_shorten (25, nodeText));
    $("#linkDivisions").attr ("title", nodeText);
    $("#hiddenDivisions").hide ();
}

function ed_treeLoad (sender, eventArgs) {
    var nodes = sender.get_selectedNodes ();
    if (nodes.length > 0) {
        var nodeText = nodes [0].get_text ();
         $("#linkDivisions").text (ed_shorten (25, nodeText));
         $("#linkDivisions").attr ("title", nodeText);
    }
}
</script>

<div class="dnnForm dnnClear ViewEmployeeDirectory">
<fieldset>
    <div class="dnnFormItem dnnClear">
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
        <asp:LinkButton id="linkSearch" runat="server" OnClick="linkSearch_Click" Text="Search" CssClass="dnnPrimaryAction linkSearch" />
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