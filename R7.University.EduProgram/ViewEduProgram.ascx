<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ViewEduProgram.ascx.cs" Inherits="R7.University.EduProgram.ViewEduProgram" %>
<asp:FormView id="formEduProgram" runat="server" OnDataBound="formEduProgram_DataBound">
    <ItemTemplate>
        <div>
            <h3><%# Eval ("Title_String") %></h3>
            <strong>Edu. Level:</strong> <%# Eval ("EduLevelID") %><br />
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
                    <strong><%# Eval ("Title_String") %></strong><br />
                    <strong>Edu. Level:</strong> <%# Eval ("EduLevelId") %><br />
                    <asp:Label runat="server" Visible='<%# Eval ("AccreditedToDate_Visible") %>'
                        Text='<%# Eval ("AccreditedToDate") %>' />
                    <strong>CommunityAccreditedTo:</strong> <%# Eval ("CommunityAccreditedToDate") %>
                </li>
            </ItemTemplate>
        </asp:ListView>
    </ItemTemplate>
</asp:FormView>