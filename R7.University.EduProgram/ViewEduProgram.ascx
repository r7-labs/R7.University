<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ViewEduProgram.ascx.cs" Inherits="R7.University.EduProgram.ViewEduProgram" %>
<asp:FormView id="formEduProgram" runat="server" OnDataBound="formEduProgram_DataBound" RenderOuterTable="false">
    <ItemTemplate>
        <div class="u8y-eduprogram">
            <div class="u8y-eduprogram-info">
                <h3 runat="server" class='<%# Eval ("CssClass") %>'>
                    <asp:HyperLink runat="server" Visible='<%# IsEditable %>' NavigateUrl='<%# Eval ("Edit_Url") %>' IconKey="Edit" />
                    <%# Eval ("Title_String") %>
                </h3>
                <div><asp:Label runat="server" resourcekey="EduLevel.Text" CssClass="u8y-label" /> <%# Eval ("EduLevel_Title") %></div>
                <asp:Panel runat="server" Visible='<%# Eval ("EduStandard_Visible") %>'>
                    <asp:Label runat="server" resourcekey="EduStandard.Text" CssClass="u8y-label" /> 
                        <%# HttpUtility.HtmlDecode ((string) Eval ("EduStandard_Links")) %>
                </asp:Panel>
                <asp:Panel runat="server" Visible='<%# Eval ("Division_Visible") %>'>
                    <asp:Label runat="server" resourcekey="Faculty.Text" CssClass="u8y-label" /> 
                    <%# HttpUtility.HtmlDecode ((string) Eval ("Division_Link")) %>
                </asp:Panel>
            </div>
            <asp:Panel runat="server" Visible='<%# Eval ("EduProgramProfiles_Visible") %>'>
                <h3><%# LocalizeString ("EduProgramProfiles.Text") %></h3>
                <asp:ListView id="listEduProgramProfiles" runat="server">
                    <LayoutTemplate>
                        <ul runat="server" class="eduprogram-profiles">
                            <div runat="server" id="itemPlaceholder"></div>
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li>
                            <h4 runat="server" class='<%# Eval ("CssClass") %>'>
                                <asp:HyperLink runat="server" Visible='<%# IsEditable %>' NavigateUrl='<%# Eval ("Edit_Url") %>' IconKey="Edit" />
                                <%# Eval ("Title_String") %>
                            </h4>
                            <div><asp:Label runat="server" resourcekey="EduLevel.Text" CssClass="u8y-label" /> <%# Eval ("EduLevel_Title") %></div>
                            <asp:Panel runat="server" Visible='<%# Eval ("Division_Visible") %>'>
                                <asp:Label runat="server" resourcekey="FacultyDepartment.Text" CssClass="u8y-label" /> 
                                <%# HttpUtility.HtmlDecode ((string) Eval ("Division_Link")) %>
                            </asp:Panel>
                            <asp:Panel runat="server" Visible='<%# Eval ("AccreditedToDate_Visible") %>'>
                                <asp:Label runat="server" resourcekey="AccreditedToDate.Text" CssClass="u8y-label" /> <%# Eval ("AccreditedToDate_String") %>
                            </asp:Panel>
                            <asp:Panel runat="server" Visible='<%# Eval ("CommunityAccreditedToDate_Visible") %>'>
                                <asp:Label runat="server" resourcekey="CommunityAccreditedToDate.Text" CssClass="u8y-label" /> <%# Eval ("CommunityAccreditedToDate_String") %>
                            </asp:Panel>
                            <asp:Panel runat="server" Visible='<%# Eval ("EduForms_Visible") %>'>
                                <p><strong><%# LocalizeString ("EduForms.Text") %></strong></p>
                                <%# HttpUtility.HtmlDecode ((string) Eval ("EduForms_String")) %>
                            </asp:Panel>
                        </li>
                    </ItemTemplate>
                </asp:ListView>
            </asp:Panel>
        </div>
    </ItemTemplate>
</asp:FormView>