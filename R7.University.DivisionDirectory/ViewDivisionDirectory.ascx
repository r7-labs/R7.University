<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ViewDivisionDirectory.ascx.cs" Inherits="R7.University.DivisionDirectory.ViewDivisionDirectory" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnJsInclude runat="server" FilePath="dnn.jquery.js" PathNameAlias="SharedScripts" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University.DivisionDirectory/js/tree.js" />

<div class="dnnForm dnnClear divisionDirectory">
    <asp:MultiView id="mviewDivisionDirectory" runat="server">
        <asp:View id="viewSearch" runat="server">
            <asp:Panel runat="server" DefaultButton="linkSearch" CssClass="dnnFormItem dnnClear">
                <div class="wrapperSearchFlags">
                    <asp:CheckBox id="checkIncludeSubdivisions" runat="server" resourcekey="checkIncludeSubdivisions.Text" />
                </div>
                <asp:TextBox id="textSearch" runat="server" MaxLength="50" CssClass="textSearch" />
                <div class="wrapperDivisions">
                    <a id="linkDivisions" class="dnnSecondaryAction linkDivisions" onclick="$('.divisionDirectory #hiddenDivisions').toggle ()"><%= LocalizeString ("AllDivisions.Text") %></a>
                    <div id="hiddenDivisions">
                        <dnn:DnnTreeView id="treeDivisions" runat="server" 
                            OnClientLoad="dd_treeLoad"
                            OnClientNodeClicked="dd_treeNodeClicked" 
                            DataTextField="Title"
                            DataValueField="DivisionID"
                            DataFieldID = "DivisionID"
                            DataFieldParentID="ParentDivisionID"
                        />
                    </div>
                </div>
                <asp:LinkButton id="linkSearch" runat="server" resourcekey="linkSearch.Text" CssClass="dnnPrimaryAction linkSearch" OnClick="linkSearch_Click" />
            </asp:Panel>
            <asp:GridView id="gridDivisions" runat="server" Visible="false" AutoGenerateColumns="false"
                UseAccessibleHeader="true" CssClass="table table-stripped table-bordered table-hover gridDivisions" 
                GridLines="None" OnRowCreated="grid_RowCreated" OnRowDataBound="gridDivisions_RowDataBound">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HyperLink id="linkEdit" runat="server">
                                <asp:Image id="iconEdit" runat="server" />
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Title">
                        <ItemTemplate>
                            <asp:Label id="labelTitle" runat="server" />
                            <asp:HyperLink id="linkTitle" runat="server" target="_blank" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Location">
                        <ItemTemplate>
                            <asp:Literal id="literalLocation" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Phone">
                        <ItemTemplate>
                            <asp:Literal id="literalPhone" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Email">
                        <ItemTemplate>
                            <asp:HyperLink id="linkEmail" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Document">
                        <ItemTemplate>
                            <asp:HyperLink id="linkDocument" runat="server" target="_blank" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ContactPerson">
                        <ItemTemplate>
                            <asp:HyperLink id="linkContactPerson" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:View>
        <asp:View id="viewObrnadzorDivisions" runat="server">
            <div class="table-responsive">
                <asp:GridView id="gridObrnadzorDivisions" runat="server" AutoGenerateColumns="false" 
                        UseAccessibleHeader="true" OnRowCreated="grid_RowCreated" OnRowDataBound="gridObrnadzorDivisions_RowDataBound"
                        CssClass="table table-bordered table-stripped table-hover" GridLines="None" Width="100%">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HyperLink id="linkEdit" runat="server">
                                    <asp:Image id="iconEdit" runat="server" />
                                </asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Order" HeaderText="Order" />
                        <asp:BoundField DataField="TitleLink" HeaderText="TitleObrnadzor" HtmlEncode="false" />
                        <asp:BoundField DataField="LocationString" HeaderText="Location" HtmlEncode="false" />
                        <asp:BoundField DataField="Phone" HeaderText="Phone" />
                        <asp:BoundField DataField="WebSiteLink" HeaderText="WebSiteObrnadzor" HtmlEncode="false" />
                        <asp:BoundField DataField="EmailLink" HeaderText="EmailObrnadzor" HtmlEncode="false" />
                        <asp:BoundField DataField="DocumentLink" HeaderText="Document" HtmlEncode="false" />
                        <asp:TemplateField HeaderText="ContactPerson">
                            <ItemTemplate>
                                <asp:Literal id="literalContactPerson" runat="server" /> 
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </asp:View>
    </asp:MultiView>
</div>