<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewEduProgramDirectory.ascx.cs" Inherits="R7.University.EduProgramDirectory.ViewEduProgramDirectory" %>
<div class="dnnForm dnnClear eduprogram-directory">
    <asp:MultiView id="mviewEduProgramDirectory" runat="server" ActiveViewIndex="0">
        <asp:View id="viewEduStandards" runat="server">
            <fieldset>
                <asp:GridView id="gridEduStandards" runat="server" AutoGenerateColumns="false" 
                    CssClass="table table-bordered table-stripped table-hover grid-edustandards"
                    GridLines="None" OnRowDataBound="gridEduStandards_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HyperLink id="linkEdit" runat="server">
                                    <asp:Image id="iconEdit" runat="server" />
                                </asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Code" HeaderText="EduProgramCode" />
                        <asp:BoundField DataField="Title" HeaderText="EduProgramTitle" />
                        <asp:BoundField DataField="EduLevelString" HeaderText="EduProgramEduLevel" />
                        <asp:BoundField DataField="EduStandardLink" HeaderText="EduProgramStandard" HtmlEncode="false">
                            <ItemStyle CssClass="edustandard-link"></ItemStyle>
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </fieldset>
        </asp:View>
    </asp:MultiView>
 </div>
