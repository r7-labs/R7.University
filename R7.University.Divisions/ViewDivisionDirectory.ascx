<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ViewDivisionDirectory.ascx.cs" Inherits="R7.University.Divisions.ViewDivisionDirectory" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="AgplSignature" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/AgplSignature.ascx" %>

<dnn:DnnJsInclude runat="server" FilePath="dnn.jquery.js" PathNameAlias="SharedScripts" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/assets/css/module.css" />

<div class="dnnForm dnnClear division-directory">
    <asp:MultiView id="mviewDivisionDirectory" runat="server">
        <asp:View id="viewSearch" runat="server">
            <asp:Panel runat="server" DefaultButton="linkSearch" CssClass="dnnFormItem dnnClear">
                <div class="wrapperSearchFlags"></div>
                <asp:TextBox id="textSearch" runat="server" MaxLength="50" CssClass="textSearch" />
                <asp:LinkButton id="linkSearch" runat="server" resourcekey="linkSearch.Text" CssClass="btn btn-primary linkSearch" OnClick="linkSearch_Click" />
            </asp:Panel>
            <asp:GridView id="gridDivisions" runat="server" Visible="false" AutoGenerateColumns="false"
                UseAccessibleHeader="true" CssClass="table table-striped table-bordered table-hover grid-divisions"
                GridLines="None" OnRowCreated="grid_RowCreated" OnRowDataBound="gridDivisions_RowDataBound">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HyperLink id="linkEdit" runat="server">
                                <asp:Image id="iconEdit" runat="server" />
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Title.Column">
                        <ItemTemplate>
                            <asp:Label id="labelTitle" runat="server" />
                            <asp:HyperLink id="linkTitle" runat="server" target="_blank" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Location.Column">
                        <ItemTemplate>
                            <asp:Literal id="literalLocation" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Phone.Column">
                        <ItemTemplate>
                            <asp:Literal id="literalPhone" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Email.Column">
                        <ItemTemplate>
                            <asp:HyperLink id="linkEmail" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Document.Column">
                        <ItemTemplate>
                            <asp:HyperLink id="linkDocument" runat="server" target="_blank" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="HeadEmployee.Column">
                        <ItemTemplate>
                            <asp:Literal id="literalHeadEmployee" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:View>
        <asp:View id="viewObrnadzorDivisions" runat="server">
            <div class="table-responsive">
                <asp:GridView id="gridObrnadzorDivisions" runat="server" EnableViewState="false" AutoGenerateColumns="false"
                    UseAccessibleHeader="true" OnRowCreated="grid_RowCreated" OnRowDataBound="gridObrnadzorDivisions_RowDataBound"
                        CssClass="table table-bordered table-striped table-hover grid-obrnadzor-divisions" GridLines="None">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HyperLink id="linkEdit" runat="server">
                                    <asp:Image id="iconEdit" runat="server" />
                                </asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Order" HeaderText="Order.Column" />
                        <asp:BoundField DataField="TitleLink" HeaderText="TitleObrnadzor.Column" HtmlEncode="false" />
                        <asp:BoundField DataField="HeadEmployeeHtml" HeaderText="HeadEmployee.Column" HtmlEncode="false" />
					    <asp:BoundField DataField="LocationString" HeaderText="Location.Column" HtmlEncode="false" />
                        <asp:BoundField DataField="WebSiteLink" HeaderText="WebSiteObrnadzor.Column" HtmlEncode="false" />
					    <asp:BoundField DataField="EmailLink" HeaderText="EmailObrnadzor.Column" HtmlEncode="false" />
						<asp:BoundField DataField="Phone" HeaderText="Phone.Column" />
                        <asp:BoundField DataField="DocumentLink" HeaderText="Document.Column" HtmlEncode="false" />
                    </Columns>
                </asp:GridView>
            </div>
        </asp:View>
		<asp:View id="viewObrnadzorGoverningDivisions" runat="server">
            <div class="table-responsive">
                <asp:GridView id="gridObrnadzorGoverningDivisions" runat="server" EnableViewState="false" AutoGenerateColumns="false"
                    UseAccessibleHeader="true" OnRowCreated="grid_RowCreated" OnRowDataBound="gridObrnadzorDivisions_RowDataBound"
                        CssClass="table table-bordered table-striped table-hover grid-obrnadzor-governing-divisions" GridLines="None">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HyperLink id="linkEdit" runat="server">
                                    <asp:Image id="iconEdit" runat="server" />
                                </asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Order" HeaderText="Order.Column" />
                        <asp:BoundField DataField="TitleLink" HeaderText="TitleObrnadzorGoverningDivision.Column" HtmlEncode="false" />
                        <asp:BoundField DataField="HeadEmployeeHtml" HeaderText="HeadEmployee.Column" HtmlEncode="false" />
                        <asp:BoundField DataField="DocumentLink" HeaderText="Document.Column" HtmlEncode="false" />
                    </Columns>
                </asp:GridView>
            </div>
        </asp:View>
    </asp:MultiView>
	<controls:AgplSignature runat="server" />
</div>
