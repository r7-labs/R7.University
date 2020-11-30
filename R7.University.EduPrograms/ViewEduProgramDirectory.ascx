<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="ViewEduProgramDirectory.ascx.cs" Inherits="R7.University.EduPrograms.ViewEduProgramDirectory" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="AgplSignature" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/AgplSignature.ascx" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/assets/css/module.css" />
<div class="eduprogram-directory">
	<div class="table-responsive">
	    <asp:GridView id="gridEduStandards" runat="server"
			AutoGenerateColumns="false" UseAccessibleHeader="true"
			CssClass="table table-bordered table-striped table-hover grid-edustandards"
	        GridLines="None" OnRowCreated="grid_RowCreated" OnRowDataBound="gridEduStandards_RowDataBound">
	        <Columns>
	            <asp:TemplateField>
	                <ItemTemplate>
	                    <asp:HyperLink id="linkEdit" runat="server">
	                        <asp:Image id="iconEdit" runat="server" />
	                    </asp:HyperLink>
	                </ItemTemplate>
	            </asp:TemplateField>
	            <asp:BoundField DataField="Order" HeaderText="EduProgramOrder.Column" DataFormatString="{0}." />
	            <asp:BoundField DataField="Code" HeaderText="EduProgramCode.Column" />
	            <asp:BoundField DataField="Title_Link" HeaderText="EduProgramTitle.Column" HtmlEncode="false" />
	            <asp:BoundField DataField="EduLevel_String" HeaderText="EduProgramEduLevel.Column" />
	            <asp:BoundField DataField="Generation" HeaderText="EduProgramGeneration.Column" />
	            <asp:BoundField DataField="ProfStandard_Links" HeaderText="EduProgramProfStandard.Column" HtmlEncode="false" />
	            <asp:BoundField DataField="EduStandard_Links" HeaderText="EduProgramEduStandard.Column" HtmlEncode="false" />
	        </Columns>
	    </asp:GridView>
	</div>
	<controls:AgplSignature runat="server" />
</div>
