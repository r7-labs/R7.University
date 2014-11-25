<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ViewEmployeeDirectory.ascx.cs" Inherits="R7.University.EmployeeDirectory.ViewEmployeeDirectory" %>

<div class="ViewEmployeeDirectory">

    <div class="dnnFormItem">
        <asp:TextBox id="textSearch" runat="server" CssClass="textSearch" />
        <asp:LinkButton id="linkSearch" runat="server" OnClick="linkSearch_Click" Text="Search" CssClass="dnnPrimaryAction" />
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
 </div>