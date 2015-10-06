<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ViewEmployeeDirectory.ascx.cs" Inherits="R7.University.EmployeeDirectory.ViewEmployeeDirectory" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnJsInclude runat="server" FilePath="dnn.jquery.js" PathNameAlias="SharedScripts" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University.EmployeeDirectory/js/tree.js" />

<div class="dnnForm dnnClear employeeDirectory">
    <asp:MultiView id="mviewEmployeeDirectory" runat="server">
        <asp:View id="viewSearch" runat="server">
            <fieldset>
                <asp:Panel runat="server" DefaultButton="linkSearch" CssClass="dnnFormItem dnnClear">
                    <div class="wrapperSearchFlags">
                        <asp:CheckBox id="checkTeachersOnly" runat="server" resourcekey="checkTeachersOnly.Text" />
                        <asp:CheckBox id="checkIncludeSubdivisions" runat="server" resourcekey="checkIncludeSubdivisions.Text" />
                    </div>
                    <asp:TextBox id="textSearch" runat="server" MaxLength="50" CssClass="textSearch" />
                    <div class="wrapperDivisions">
                        <a id="linkDivisions" class="dnnSecondaryAction linkDivisions" onclick="$('.employeeDirectory #hiddenDivisions').toggle ()"><%= LocalizeString ("AllDivisions.Text") %></a>
                        <div id="hiddenDivisions">
                            <dnn:DnnTreeView id="treeDivisions" runat="server" 
                                OnClientLoad="ed_treeLoad"
                                OnClientNodeClicked="ed_treeNodeClicked" 
                                DataTextField="Title"
                                DataValueField="DivisionID"
                                DataFieldID = "DivisionID"
                                DataFieldParentID="ParentDivisionID"
                            />
                        </div>
                    </div>
                    <asp:LinkButton id="linkSearch" runat="server" resourcekey="linkSearch.Text" CssClass="dnnPrimaryAction linkSearch" OnClick="linkSearch_Click" />
                </asp:Panel>
                <asp:GridView id="gridEmployees" runat="server" Visible="false" AutoGenerateColumns="false" CssClass="dnnGrid gridEmployees"
                    GridLines="None" OnRowDataBound="gridEmployees_RowDataBound">
                    <HeaderStyle CssClass="dnnGridHeader" />
                    <RowStyle CssClass="dnnGridItem" />
                    <AlternatingRowStyle CssClass="dnnGridAltItem" />
                    <SelectedRowStyle CssClass="dnnFormError" />
                    <EditRowStyle CssClass="dnnFormInput" />
                    <FooterStyle CssClass="dnnGridFooter" />
                    <PagerStyle CssClass="dnnGridPager" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HyperLink id="linkEdit" runat="server">
                                    <asp:Image id="iconEdit" runat="server" />
                                </asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <%# LocalizeString ("Name.Header") %>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HyperLink id="linkName" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <%# LocalizeString ("Position.Header") %>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal id="literalPosition" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                             <HeaderTemplate>
                                <%# LocalizeString ("Phone.Header") %>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal id="literalPhone" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                             <HeaderTemplate>
                                <%# LocalizeString ("Email.Header") %>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HyperLink id="linkEmail" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                             <HeaderTemplate>
                                <%# LocalizeString ("WorkingPlace.Header") %>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal id="literalWorkingPlace" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </fieldset>
        </asp:View>
        <asp:View id="viewTeachersByEduProgram" runat="server">
            <asp:Repeater id="repeaterEduPrograms" runat="server" OnItemDataBound="repeaterEduPrograms_ItemDataBound">
                <ItemTemplate>
                    <h3>
                        <asp:Literal id="literalEduProgramAnchor" runat="server" />
                        <asp:Label id="labelEduProgram" runat="server" Text='<%# Eval ("EduProfileString") %>' />
                    </h3>
                    <asp:GridView id="gridTeachersByEduProgram" runat="server" AutoGenerateColumns="false" 
                            OnRowDataBound="gridTeachersByEduProgram_RowDataBound"
                            CssClass="table table-bordered table-stripped table-hover small" GridLines="None" Width="100%">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HyperLink id="linkEdit" runat="server">
                                        <asp:Image id="iconEdit" runat="server" />
                                    </asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Order">
                                <ItemTemplate>
                                    <asp:Literal id="literalOrder" runat="server" /> 
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FullName" HeaderText="FullName" />
                            <asp:TemplateField HeaderText="Positions">
                                <ItemTemplate>
                                    <asp:Literal id="literalPositions" runat="server" /> 
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Disciplines">
                                <ItemTemplate>
                                    <asp:Literal id="literalDisciplines" runat="server" /> 
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="AcademicDegrees">
                                <ItemTemplate>
                                    <asp:Literal id="literalAcademicDegrees" runat="server" /> 
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="AcademicTitles">
                                <ItemTemplate>
                                    <asp:Literal id="literalAcademicTitles" runat="server" /> 
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Education">
                                <ItemTemplate>
                                    <asp:Literal id="literalEducation" runat="server" /> 
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Training">
                                <ItemTemplate>
                                    <asp:Literal id="literalTraining" runat="server" /> 
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ExperienceYears" HeaderText="ExperienceYears" />
                            <asp:BoundField DataField="ExperienceYearsBySpec" HeaderText="ExperienceYearsBySpec" />
                        </Columns>
                    </asp:GridView>
                </ItemTemplate>
            </asp:Repeater>
        </asp:View>
    </asp:MultiView>
 </div>