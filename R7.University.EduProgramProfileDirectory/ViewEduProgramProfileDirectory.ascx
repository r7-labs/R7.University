<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewEduProgramProfileDirectory.ascx.cs" 
    Inherits="R7.University.EduProgramProfileDirectory.ViewEduProgramProfileDirectory" %>
<div class="dnnForm dnnClear eduprogramprofile-directory">
    <asp:MultiView id="mviewEduProgramProfileDirectory" runat="server" ActiveViewIndex="0">
        <asp:View id="viewEduProgramProfiles" runat="server">
            <fieldset>
                <asp:GridView id="gridEduProgramProfiles" runat="server" AutoGenerateColumns="false" 
                        CssClass="table table-bordered table-stripped table-hover grid-eduprogramprofiles"
                        GridLines="None" OnRowDataBound="gridEduProgramProfiles_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HyperLink id="linkEdit" runat="server">
                                    <asp:Image id="iconEdit" runat="server" />
                                </asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ProfileCode" HeaderText="ProfileCode" />
                        <asp:BoundField DataField="ProfileTitle" HeaderText="ProfileTitle" />
                    </Columns>
                </asp:GridView>
            </fieldset>
        </asp:View>
    </asp:MultiView>
</div>
