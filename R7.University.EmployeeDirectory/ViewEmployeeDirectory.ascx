<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ViewEmployeeDirectory.ascx.cs" Inherits="R7.University.EmployeeDirectory.ViewEmployeeDirectory" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnJsInclude runat="server" FilePath="dnn.jquery.js" PathNameAlias="SharedScripts" />

<script type="text/javascript">
function employeeDirectoryDivisionNodeClicked (sender, eventArgs)
{
    var node = eventArgs.get_node ();
    $("#linkDivisions").text (node.get_text ());
    $("#linkDivisions").attr ("title", node.get_text ());
    $("#hiddenDivisions").hide ();
}

function employeeDirectoryDivisionLoad (sender, eventArgs)
{
    // set #linkDivisions text and tooltip by current treeview selection
    var nodes = sender.get_selectedNodes ();
    if (nodes.length > 0)
    {
         $("#linkDivisions").text (nodes [0].get_text ());
         $("#linkDivisions").attr ("title", nodes [0].get_text ());
    }
}
</script>

<div class="dnnForm dnnClear ViewEmployeeDirectory">
<fieldset>
    <div class="dnnFormItem dnnClear">
        <asp:TextBox id="textSearch" runat="server" CssClass="textSearch" />
        <%-- <dnn:DnnComboBox id="comboDivisions" runat="server" DataTextField="Title" DataValueField="DivisionID" CssClass="comboDivisions" /> --%>
        <a id="linkDivisions" class="dnnSecondaryAction linkDivisions" onclick="$('#hiddenDivisions').toggle ()"><%= LocalizeString ("NotSelected.Text") %></a>
        <asp:LinkButton id="linkSearch" runat="server" OnClick="linkSearch_Click" Text="Search" CssClass="dnnPrimaryAction linkSearch" />
    </div>
    <div id="hiddenDivisions" style="display:none">
        <dnn:DnnTreeView id="treeDivisions" runat="server" 
            OnClientLoad="employeeDirectoryDivisionLoad"
            OnClientNodeClicked="employeeDirectoryDivisionNodeClicked" 
            DataTextField="Title"
            DataValueField="DivisionID"
            DataFieldID = "DivisionID"
            DataFieldParentID="ParentDivisionID"
        />
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