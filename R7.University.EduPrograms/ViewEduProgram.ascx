<%@ Control Language="C#" AutoEventWireup="false" EnableViewState="false" CodeBehind="ViewEduProgram.ascx.cs" Inherits="R7.University.EduPrograms.ViewEduProgram" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/assets/css/module.css" />

<asp:FormView id="formEduProgram" runat="server" ItemType="R7.University.EduPrograms.ViewModels.EduProgramViewModel" RenderOuterTable="false">
    <ItemTemplate>
        <div class="u8y-eduprogram">
            <div class="u8y-eduprogram-info">
                <p>
                    <label runat="server"><%# LocalizeString ("EduLevel.Text") %></label>
                    <%# Item.EduLevel_Title %>
                </p>
                <div runat="server" Visible='<%# Item.DivisionsVisible %>'>
                    <label><%# LocalizeString ("Divisions.Text") %></label>
                    <ul class="u8y-eduprogram-divisions">
                    <asp:ListView runat="server" DataSource="<%# Item.DivisionViewModels %>" ItemType="R7.University.EduPrograms.ViewModels.EduProgramDivisionViewModel" >
                        <LayoutTemplate>
                            <div runat="server" id="itemPlaceholder"></div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <li><%# Item.DivisionLink %></li>
                        </ItemTemplate>
                    </asp:ListView>
                    </ul>
                </div>
                <div runat="server" Visible='<%# Item.EduStandard_Visible %>' class="u8y-para">
                    <label runat="server"><%# LocalizeString ("EduStandard.Text") %></label>
                    <%# HttpUtility.HtmlDecode (Item.EduStandard_Links) %>
                </div>
            </div>
            <div runat="server" Visible='<%# Item.EduProfiles_Visible %>'>
                <asp:ListView runat="server" DataSource="<%# Item.EduProgramProfileViewModels %>" ItemType="R7.University.EduPrograms.ViewModels.EduProfileViewModel">
                    <LayoutTemplate>
                        <div runat="server" class="u8y-eduprogram-profiles">
                            <div runat="server" id="itemPlaceholder"></div>
                        </div>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <div class="card card-body bg-light mb-3">
                            <h3 runat="server" class='<%# Item.CssClass %>'>
                                <asp:HyperLink runat="server" Visible='<%# IsEditable %>' NavigateUrl='<%# Item.Edit_Url %>' ImageUrl="<%# R7.University.Components.UniversityIcons.Edit %>" />
                                <%# Item.Title_String %>
                            </h3>
                            <p>
                                <label runat="server"><%# LocalizeString ("EduLevel.Text") %></label>
                                <%# Item.EduLevel_Title %>
                            </p>
							<div runat="server" Visible='<%# Item.DivisionsVisible %>'>
                                <label><%# LocalizeString ("Divisions.Text") %></label>
                                <ul class="u8y-eduprogram-profiles-divisions">
                                <asp:ListView runat="server" DataSource="<%# Item.DivisionViewModels %>" ItemType="R7.University.EduPrograms.ViewModels.EduProgramDivisionViewModel" >
                                    <LayoutTemplate>
                                        <div runat="server" id="itemPlaceholder"></div>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <li><%# Item.DivisionLink %></li>
                                    </ItemTemplate>
                                </asp:ListView>
                                </ul>
                            </div>
							<div runat="server" Visible='<%# Item.AccreditedToDate_Visible || Item.CommunityAccreditedToDate_Visible %>' class="u8y-para">
                                <div runat="server" Visible='<%# Item.AccreditedToDate_Visible %>'>
                                    <label runat="server"><%# LocalizeString ("AccreditedToDate.Text") %></label>
                                    <%# Item.AccreditedToDate_String %>
                                </div>
                                <div runat="server" Visible='<%# Item.CommunityAccreditedToDate_Visible %>'>
                                    <label runat="server"><%# LocalizeString ("CommunityAccreditedToDate.Text") %></label>
                                    <%# Item.CommunityAccreditedToDate_String %>
                                </div>
                            </div>
                            <div runat="server" Visible='<%# Item.EduFormsForAdmission_Visible %>'>
                                <label runat="server">
									<strong>
										<%# LocalizeString ("EduFormsForAdmission.Text") %>
										<%# Item.YearOfAdmission %><%# LocalizeString ("Year.Text") %>
									</strong>
								</label>
                                <%# HttpUtility.HtmlDecode (Item.EduFormsForAdmission_String) %>
                            </div>
							<div runat="server" Visible='<%# Item.ImplementedEduForms_Visible %>' class="u8y-para-end">
                                <label runat="server"><%# LocalizeString ("ImplementedEduForms.Text") %></label>
                                <%# HttpUtility.HtmlDecode (Item.ImplementedEduForms_String) %>
                            </div>
						</div>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </div>
    </ItemTemplate>
</asp:FormView>
