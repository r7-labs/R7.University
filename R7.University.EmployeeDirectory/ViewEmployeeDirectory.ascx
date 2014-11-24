<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ViewEmployeeDirectory.ascx.cs" Inherits="R7.University.EmployeeDirectory.ViewEmployeeDirectory" %>

<asp:TextBox id="textSearch" runat="server" />
<asp:LinkButton id="linkSearch" runat="server" OnClick="linkSearch_Click" Text="Search" CssClass="dnnPrimaryAction" />

<asp:GridView id="gridEmployees" runat="server" AutoGenerateColumns="false" CssClass="dnnGrid"
GridLines="None" OnRowDataBound="gridEmployees_RowDataBound">
    <HeaderStyle CssClass="dnnGridHeader" horizontalalign="Left" />
    <RowStyle CssClass="dnnGridItem" horizontalalign="Left" />
    <AlternatingRowStyle CssClass="dnnGridAltItem" />
    <SelectedRowStyle CssClass="dnnFormError" />
    <EditRowStyle CssClass="dnnFormInput" />
    <FooterStyle CssClass="dnnGridFooter" />
    <PagerStyle CssClass="dnnGridPager" />
    <Columns>
        <asp:TemplateField>
           <ItemTemplate>
                <asp:HyperLink id="linkFullName" runat="server" />
           </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
           <ItemTemplate>
                <asp:Literal id="literalPosition" runat="server" />
           </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
           <ItemTemplate>
                <asp:Literal id="literalPhone" runat="server" />
           </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
           <ItemTemplate>
                <asp:HyperLink id="linkEmail" runat="server" />
           </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
           <ItemTemplate>
                <asp:Literal id="literalWorkingPlace" runat="server" />
           </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
