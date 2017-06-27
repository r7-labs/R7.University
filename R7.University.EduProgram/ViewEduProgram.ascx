<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ViewEduProgram.ascx.cs" Inherits="R7.University.EduProgram.ViewEduProgram" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/css/module.css" />

<asp:FormView id="formEduProgram" runat="server" OnDataBound="formEduProgram_DataBound" RenderOuterTable="false">
    <ItemTemplate>
        <div class="u8y-eduprogram">
            <div class="u8y-eduprogram-info">
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
                <asp:ListView id="listEduProgramProfiles" runat="server">
                    <LayoutTemplate>
                        <div runat="server" class="u8y-eduprogram-profiles">
                            <div runat="server" id="itemPlaceholder"></div>
                        </div>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <div>
                            <h3 runat="server" class='<%# Eval ("CssClass") %>'>
                                <asp:HyperLink runat="server" Visible='<%# IsEditable %>' NavigateUrl='<%# Eval ("Edit_Url") %>' IconKey="Edit" />
                                <%# Eval ("Title_String") %>
                            </h3>
                            <p>
                                <label runat="server"><%# LocalizeString ("EduLevel.Text") %></label>
                                <%# Eval ("EduLevel_Title") %>
                            </p>
                            <p runat="server" Visible='<%# Eval ("Division_Visible") %>'>
                                <label runat="server"><%# LocalizeString ("FacultyDepartment.Text") %></label>
                                <%# HttpUtility.HtmlDecode ((string) Eval ("Division_Link")) %>
                            </p>
                            <div runat="server" Visible='<%# (bool) Eval ("AccreditedToDate_Visible") || (bool) Eval ("CommunityAccreditedToDate_Visible") %>' class="u8y-para">
                                <div runat="server" Visible='<%# Eval ("AccreditedToDate_Visible") %>'>
                                    <label runat="server"><%# LocalizeString ("AccreditedToDate.Text") %></label>
                                    <%# Eval ("AccreditedToDate_String") %>
                                </div>
                                <div runat="server" Visible='<%# Eval ("CommunityAccreditedToDate_Visible") %>'>
                                    <label runat="server"><%# LocalizeString ("CommunityAccreditedToDate.Text") %></label>
                                    <%# Eval ("CommunityAccreditedToDate_String") %>
                                </div>
                            </div>
                            <div runat="server" Visible='<%# Eval ("EduForms_Visible") %>' class="u8y-para-end">
                                <label runat="server"><%# LocalizeString ("EduForms.Text") %></label>
                                <%# HttpUtility.HtmlDecode ((string) Eval ("EduForms_String")) %>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </div>
    </ItemTemplate>
</asp:FormView>