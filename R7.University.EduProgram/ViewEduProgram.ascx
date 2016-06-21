<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ViewEduProgram.ascx.cs" Inherits="R7.University.EduProgram.ViewEduProgram" %>
<asp:FormView id="formEduProgram" runat="server" OnDataBound="formEduProgram_DataBound" CssClass="u8y-eduprogram">
    <ItemTemplate>
        <div>
            <h3><%# Eval ("Title_String") %></h3>
            <p><asp:Label runat="server" resourcekey="EduLevel.Text" CssClass="u8y-label" /> <%# Eval ("EduLevel_Title") %></p>
            <strong>Standard:</strong>
        </div>
        <h4>Educational Profiles:</h4>
        <asp:ListView id="listEduProgramProfiles" runat="server">
            <LayoutTemplate>
                <ul runat="server" class="eduprogram-profiles">
                    <div runat="server" id="itemPlaceholder"></div>
                </ul>
            </LayoutTemplate>
            <ItemTemplate>
                <li>
                    <h4><%# Eval ("Title_String") %></h4>
                    <p><asp:Label runat="server" resourcekey="EduLevel.Text" CssClass="u8y-label" /> <%# Eval ("EduLevel_Title") %></p>
                    <asp:Panel runat="server" Visible='<%# Eval ("AccreditedToDate_Visible") %>'>
                        <asp:Label runat="server" resourcekey="AccreditedToDate.Text" CssClass="u8y-label" /> <%# Eval ("AccreditedToDate_String") %>
                    </asp:Panel>
                    <asp:Panel runat="server" Visible='<%# Eval ("CommunityAccreditedToDate_Visible") %>'>
                        <asp:Label runat="server" resourcekey="CommunityAccreditedToDate.Text" CssClass="u8y-label" /> <%# Eval ("CommunityAccreditedToDate_String") %>
                    </asp:Panel>
                </li>
            </ItemTemplate>
        </asp:ListView>
    </ItemTemplate>
</asp:FormView>