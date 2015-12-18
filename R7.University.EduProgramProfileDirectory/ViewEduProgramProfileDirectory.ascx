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
                        <asp:BoundField DataField="Code" />
                        <asp:BoundField DataField="Title" />
                        <asp:BoundField DataField="EduLevelString" />
                        <asp:BoundField DataField="TimeToLearnFullTimeString" HeaderText="TimeToLearnFullTime" />
                        <asp:BoundField DataField="TimeToLearnPartTimeString" HeaderText="TimeToLearnPartTime" />
                        <asp:BoundField DataField="TimeToLearnExtramuralString" HeaderText="TimeToLearnExtramural" />
                        <asp:BoundField DataField="AccreditedToDate" DataFormatString="{0:d}" HeaderText="AccreditedToDate" />
                        <asp:BoundField DataField="CommunityAccreditedToDate" DataFormatString="{0:d}" HeaderText="CommunityAccreditedToDate" />
                    </Columns>
                </asp:GridView>
            </fieldset>
        </asp:View>
    </asp:MultiView>
</div>
