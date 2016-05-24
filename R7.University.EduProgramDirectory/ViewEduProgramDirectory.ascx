<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewEduProgramDirectory.ascx.cs" Inherits="R7.University.EduProgramDirectory.ViewEduProgramDirectory" %>
<div class="dnnForm dnnClear eduprogram-directory">
    <asp:MultiView id="mviewEduProgramDirectory" runat="server" ActiveViewIndex="0">
        <asp:View id="viewEduStandards" runat="server">
            <div class="table-responsive">
                <asp:GridView id="gridEduStandards" runat="server" AutoGenerateColumns="false"
                    UseAccessibleHeader="true" CssClass="table table-bordered table-stripped table-hover grid-edustandards"
                    GridLines="None" OnRowCreated="grid_RowCreated" OnRowDataBound="gridEduStandards_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HyperLink id="linkEdit" runat="server">
                                    <asp:Image id="iconEdit" runat="server" />
                                </asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Order" HeaderText="EduProgramOrder" DataFormatString="{0}." />
                        <asp:BoundField DataField="Code" HeaderText="EduProgramCode" />
                        <asp:BoundField DataField="Title" HeaderText="EduProgramTitle" />
                        <asp:BoundField DataField="EduLevel_String" HeaderText="EduProgramEduLevel" />
                        <asp:BoundField DataField="Generation" HeaderText="EduProgramGeneration" />
                        <asp:BoundField DataField="ProfStandard_Links" HeaderText="EduProgramProfStandard" HtmlEncode="false" />
                        <asp:BoundField DataField="EduStandard_Links" HeaderText="EduProgramEduStandard" HtmlEncode="false" />
                    </Columns>
                </asp:GridView>
            </div>
        </asp:View>
    </asp:MultiView>
 </div>
