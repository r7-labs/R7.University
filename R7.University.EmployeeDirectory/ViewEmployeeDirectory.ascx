<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ViewEmployeeDirectory.ascx.cs" Inherits="R7.University.EmployeeDirectory.ViewEmployeeDirectory" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="AgplSignature" Src="~/DesktopModules/R7.University/R7.University/Controls/AgplSignature.ascx" %>

<dnn:DnnJsInclude runat="server" FilePath="dnn.jquery.js" PathNameAlias="SharedScripts" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University.EmployeeDirectory/js/tree.js" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/css/module.css" />

<div class="dnnForm dnnClear employee-directory">
    <asp:MultiView id="mviewEmployeeDirectory" runat="server">
        <asp:View id="viewSearch" runat="server">
            <fieldset>
                <asp:Panel runat="server" DefaultButton="linkSearch" CssClass="dnnFormItem dnnClear">
                    <div class="wrapperSearchFlags">
                        <asp:CheckBox id="checkTeachersOnly" runat="server" resourcekey="checkTeachersOnly.Text" />
                    </div>
                    <asp:TextBox id="textSearch" runat="server" MaxLength="50" CssClass="textSearch" />
                    <div class="wrapperDivisions">
                        <a id="linkDivisions" class="dnnSecondaryAction linkDivisions" onclick="$('.employee-directory #hiddenDivisions').toggle ()"></a>
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
                <asp:GridView id="gridEmployees" runat="server" Visible="false" AutoGenerateColumns="false"
                    UseAccessibleHeader="true" CssClass="table table-bordered table-stripped table-hover grid-employees"
                    GridLines="None" OnRowCreated="grid_RowCreated" OnRowDataBound="gridEmployees_RowDataBound">
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
        <asp:View id="viewTeachers" runat="server">
            <asp:Repeater id="repeaterEduProgramProfiles" runat="server" OnItemDataBound="repeaterEduProgramProfiles_ItemDataBound">
                <ItemTemplate>
                    <asp:Panel id="panelTeachers" runat="server">
                        <h3>
                            <asp:Literal id="literalEduProgramProfileAnchor" runat="server" />
                            <%# Eval ("Title_String") %>
                        </h3>
                        <div class="table-responsive">
                            <asp:GridView id="gridTeachers" runat="server" AutoGenerateColumns="false" 
                                    UseAccessibleHeader="true" OnRowDataBound="gridTeachers_RowDataBound"
                                    OnRowCreated="grid_RowCreated" CssClass="table table-bordered table-stripped table-hover grid-teachers" 
                                    GridLines="None">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HyperLink id="linkEdit" runat="server">
                                                <asp:Image id="iconEdit" runat="server" />
                                            </asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Order" HeaderText="Order" DataFormatString="{0}." />
                                    <asp:BoundField DataField="FullName" HeaderText="FullName" />
                                    <asp:BoundField DataField="Positions_String" HeaderText="Positions" />
                                    <asp:BoundField DataField="Disciplines_String" HeaderText="Disciplines" />
                                    <asp:BoundField DataField="AcademicDegrees_String" HeaderText="AcademicDegrees" />
                                    <asp:BoundField DataField="AcademicTitles_String" HeaderText="AcademicTitles" />
                                    <asp:BoundField DataField="Education_String" HeaderText="Education" HtmlEncode="false" />
                                    <asp:BoundField DataField="Training_String" HeaderText="Training" HtmlEncode="false" />
                                    <asp:BoundField DataField="ExperienceYears" HeaderText="ExperienceYears" />
                                    <asp:BoundField DataField="ExperienceYearsBySpec" HeaderText="ExperienceYearsBySpec" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                </ItemTemplate>
            </asp:Repeater>
        </asp:View>
    </asp:MultiView>
	<controls:AgplSignature runat="server" />
 </div>