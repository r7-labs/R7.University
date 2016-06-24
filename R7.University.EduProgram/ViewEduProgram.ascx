<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ViewEduProgram.ascx.cs" Inherits="R7.University.EduProgram.ViewEduProgram" %>
<asp:FormView id="formEduProgram" runat="server" OnDataBound="formEduProgram_DataBound" RenderOuterTable="false">
    <ItemTemplate>
        <div class="u8y-eduprogram">
            <div class="u8y-eduprogram-info">
                <h3 runat="server" class='<%# Eval ("CssClass") %>'>
                    <asp:HyperLink runat="server" Visible='<%# IsEditable %>' NavigateUrl='<%# Eval ("Edit_Url") %>' IconKey="Edit" />
                    <%# Eval ("Title_String") %>
                </h3>
                <p>
                    <label runat="server"><%# LocalizeString ("EduLevel.Text") %></label>
                    <%# Eval ("EduLevel_Title") %>
                </p>
                <p runat="server" Visible='<%# Eval ("Division_Visible") %>'>
                    <label runat="server"><%# LocalizeString ("Faculty.Text") %></label>
                    <%# HttpUtility.HtmlDecode ((string) Eval ("Division_Link")) %>
                </p>
                <div runat="server" Visible='<%# Eval ("EduStandard_Visible") %>' class="u8y-para">
                    <label runat="server"><%# LocalizeString ("EduStandard.Text") %></label>
                    <%# HttpUtility.HtmlDecode ((string) Eval ("EduStandard_Links")) %>
                </div>
            </div>
            <div runat="server" Visible='<%# Eval ("EduProgramProfiles_Visible") %>'>
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
                            <p>
                                <label runat="server"><%# LocalizeString ("EduLevel.Text") %></label>
                                <%# Eval ("EduLevel_Title") %>
                            </p>
                            <p runat="server" Visible='<%# Eval ("Division_Visible") %>'>
                                <label runat="server"><%# LocalizeString ("FacultyDepartment.Text") %></label>
                                <%# HttpUtility.HtmlDecode ((string) Eval ("Division_Link")) %>
                            </p>
                            <p runat="server" Visible='<%# Eval ("AccreditedToDate_Visible") %>'>
                                <label runat="server"><%# LocalizeString ("AccreditedToDate.Text") %></label>
                                <%# Eval ("AccreditedToDate_String") %>
                            </p>
                            <p runat="server" Visible='<%# Eval ("CommunityAccreditedToDate_Visible") %>'>
                                <label runat="server"><%# LocalizeString ("CommunityAccreditedToDate.Text") %></label>
                                <%# Eval ("CommunityAccreditedToDate_String") %>
                            </p>
                            <div runat="server" Visible='<%# Eval ("EduForms_Visible") %>' class="u8y-para">
                                <p><label runat="server"><%# LocalizeString ("EduForms.Text") %></label></p>
                                <%# HttpUtility.HtmlDecode ((string) Eval ("EduForms_String")) %>
                            </div>
                        </li>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </div>
    </ItemTemplate>
</asp:FormView>