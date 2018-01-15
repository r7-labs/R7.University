<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="ViewEduProgramProfileDirectory.ascx.cs" Inherits="R7.University.EduProgramProfiles.ViewEduProgramProfileDirectory" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="AgplSignature" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/AgplSignature.ascx" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/css/module.css" />

<div class="dnnForm dnnClear eduprogramprofile-directory">
    <asp:MultiView id="mviewEduProgramProfileDirectory" runat="server">
        <asp:View runat="server">
            <asp:Label runat="server" resourcekey="NotConfigured.Text" CssClass="dnnFormMessage dnnFormInfo" />
        </asp:View>
        <asp:View runat="server">
            <div class="table-responsive">
                <asp:GridView id="gridEduProgramProfileObrnadzorEduForms" runat="server" AutoGenerateColumns="false" 
                        UseAccessibleHeader="true"
                        CssClass="table table-bordered table-striped table-hover grid-eduprogramprofile-obrnadzor-eduforms"
                        GridLines="None" OnRowDataBound="gridEduProgramProfileObrnadzorEduForms_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HyperLink id="linkEdit" runat="server">
                                    <asp:Image id="iconEdit" runat="server" />
                                </asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Order" HeaderText="Order.Column" DataFormatString="{0}." />
                        <asp:BoundField DataField="Code" HtmlEncode="false" HeaderText="Code.Column" />
                        <asp:BoundField DataField="Title" HtmlEncode="false" HeaderText="Title.Column" />
                        <asp:BoundField DataField="EduLevelString" HtmlEncode="false" HeaderText="EduLevel.Column" />
						<asp:BoundField DataField="EduForms_String" HtmlEncode="false" HeaderText="EduForms.Column" />
                        <asp:BoundField DataField="AccreditedToDateString" HtmlEncode="false" HeaderText="AccreditedToDate.Column" />
                        <asp:BoundField DataField="CommunityAccreditedToDate" DataFormatString="{0:d}" HeaderText="CommunityAccreditedToDate.Column" />
						<asp:BoundField DataField="Languages_String" HtmlEncode="false" HeaderText="Languages.Column" />
                    </Columns>
                </asp:GridView>
            </div>
        </asp:View>
        <asp:View runat="server">
            <div class="table-responsive">
                <asp:GridView id="gridEduProgramProfileObrnadzorDocuments" runat="server" AutoGenerateColumns="false" 
                        UseAccessibleHeader="true"
                        CssClass="table table-bordered table-striped table-hover grid-eduprogramprofile-obrnadzor-documents"
                        GridLines="None" OnRowDataBound="gridEduProgramProfileObrnadzorDocuments_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HyperLink id="linkEdit" runat="server">
                                    <asp:Image id="iconEdit" runat="server" />
                                </asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Order" HeaderText="Order.Column" DataFormatString="{0}." />
                        <asp:BoundField DataField="Code" HtmlEncode="false" HeaderText="Code.Column" />
                        <asp:BoundField DataField="EduProgram_Links" HtmlEncode="false" HeaderText="Title.Column" />
                        <asp:BoundField DataField="EduLevel_String" HtmlEncode="false" HeaderText="EduLevel.Column" />
						<asp:BoundField DataField="EduForms_String" HtmlEncode="false" HeaderText="EduForms.Column" />
                        <asp:BoundField DataField="EduPlan_Links" HtmlEncode="false" HeaderText="EduPlan.Column" />
                        <asp:BoundField DataField="WorkProgramAnnotation_Links" HtmlEncode="false" HeaderText="WorkProgramAnnotation.Column" />
                        <asp:BoundField DataField="EduSchedule_Links" HtmlEncode="false" HeaderText="EduSchedule.Column" />
                        <asp:BoundField DataField="EduMaterial_Links" HtmlEncode="false" HeaderText="EduMaterial.Column" />
                        <asp:BoundField DataField="WorkProgramOfPractice_Links" HtmlEncode="false" HeaderText="WorkProgramOfPractice.Column" />
                        <asp:BoundField DataField="Contingent_Links" HtmlEncode="false" HeaderText="Contingent.Column" />
                        <asp:BoundField DataField="ContingentMovement_Links" HtmlEncode="false" HeaderText="ContingentMovement.Column" />
                    </Columns>
                </asp:GridView>
            </div>
        </asp:View>
    </asp:MultiView>
	<controls:AgplSignature runat="server" />
	<div id="u8y-epp-docs-dlg-<%: ModuleId %>" class="modal fade" role="dialog" aria-labelledby="u8y-epp-docs-dlg-title-<%: ModuleId %>">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label='<%: LocalizeString("Close") %>'><span aria-hidden="true">&times;</span></button>
                    <h4 id="u8y-epp-docs-dlg-title-<%: ModuleId %>" class="modal-title"></h4>
                </div>
                <div class="modal-body">
                </div>
            </div> 
        </div>
	</div>	
</div>
<script type="text/javascript">
(function($, document) {
    function getTh (td) {
	    // would require column index/header index mapping
        return td.closest("table").children("thead").children("tr").children("th:nth-child(" + (td.index() + 1) + ")");
    }
    $(document).ready(function() {
    	$("#u8y-epp-docs-dlg-<%: ModuleId %>").on("show.bs.modal", function (event) {
    		var link = $(event.relatedTarget);
            var table = $("#" + link.data("table"));
    		$(this).find(".modal-title").text(link.closest("tr").data("title"));
    		$(this).find(".modal-body").html(table.clone()).find("table")
		        .addClass("table table-striped table-bordered table-hover table-condensed small")
		        .prepend("<caption>" + getTh(link.closest ("td")).text() + "</caption>")
                .show();
        });
    });
} (jQuery, document));
</script>		
