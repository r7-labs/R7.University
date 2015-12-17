<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewEduProgramProfileDirectory.ascx.cs" 
    Inherits="R7.University.EduProgramProfileDirectory.ViewEduProgramProfileDirectory" %>
<div class="dnnForm dnnClear eduprogramprofile-directory">
    <asp:MultiView id="mviewEduProgramProfileDirectory" runat="server" ActiveViewIndex="0">
        <asp:View id="viewEduProgramProfiles" runat="server">
            <fieldset>
                <asp:GridView id="gridEduProgramProfiles" runat="server" AutoGenerateColumns="false" 
                        CssClass="table table-bordered table-stripped table-hover grid-eduprogramprofiles"
                        GridLines="None" OnRowDataBound="gridEduProgramProfiles_RowDataBound"
                        OnRowCreated="gridEduProgramProfiles_RowCreated"
                        >
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HyperLink id="linkEdit" runat="server">
                                    <asp:Image id="iconEdit" runat="server" />
                                </asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ProfileCode" />
                        <%-- 
                        <asp:BoundField DataField="Title" />
                        <asp:BoundField DataField="EduLevel" />
                        <asp:BoundField DataField="TimeToLearnFullTime" />
                        <asp:BoundField DataField="TimeToLearnPartTime" />
                        <asp:BoundField DataField="TimeToLearnExtramural" />
                        --%>
                        <asp:TemplateField><ItemTemplate>{Title}</ItemTemplate></asp:TemplateField>
                        <asp:TemplateField><ItemTemplate>{EduLevel}</ItemTemplate></asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>{TimeToLearnFull}</HeaderTemplate>
                            <ItemTemplate>{x}</ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>{TimeToLearnPart}</HeaderTemplate>
                            <ItemTemplate>{y}</ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>{TimeToLearnExtramural}</HeaderTemplate>
                            <ItemTemplate>{z}</ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="AccreditedToDate" HeaderText="AccreditedToDate" />
                        <asp:BoundField DataField="CommunityAccreditedToDate" HeaderText="CommunityAccreditedToDate" />
                    </Columns>
                </asp:GridView>
            </fieldset>
        </asp:View>
    </asp:MultiView>
</div>
