<%@ Control Language="C#" AutoEventWireup="false" EnableViewState="false" CodeBehind="ViewEmployeeDetails.ascx.cs" Inherits="R7.University.Employees.ViewEmployeeDetails" %>
<%@ Register TagPrefix="dnn" TagName="JavaScriptLibraryInclude" Src="~/admin/Skins/JavaScriptLibraryInclude.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="AgplSignature" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/AgplSignature.ascx" %>
<%@ Import Namespace="R7.University.Components" %>

<dnn:JavaScriptLibraryInclude runat="server" Name="React" />
<dnn:JavaScriptLibraryInclude runat="server" Name="ReactDOM" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/assets/css/module.css" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/assets/js/EmployeeExporter.min.js" />

<script>
function u8y_employee_exporter_recaptchaVerifiedCallback_<%: ModuleId %>() {
	if (typeof(window["u8y_employee_exporter_<%: ModuleId %>"]) !== "undefined") {
		window["u8y_employee_exporter_<%: ModuleId %>"].setState({isVerified: true, isRecaptchaError: false});
	}
}
function u8y_employee_exporter_recaptchaExpiredCallback_<%: ModuleId %>() {
	if (typeof(window["u8y_employee_exporter_<%: ModuleId %>"]) !== "undefined") {
		window["u8y_employee_exporter_<%: ModuleId %>"].setState({isVerified: false, isRecaptchaError: false});
	}
}
function u8y_employee_exporter_recaptchaErrorCallback_<%: ModuleId %>() {
	if (typeof(window["u8y_employee_exporter_<%: ModuleId %>"]) !== "undefined") {
		window["u8y_employee_exporter_<%: ModuleId %>"].setState({isVerified: false, isRecaptchaError: true});
	}
}
</script>
<script src="https://www.google.com/recaptcha/api.js?hl=<%: System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName %>" async="" defer=""></script>

<asp:Panel id="panelEmployeeDetails" runat="server" CssClass="dnnForm dnnClear u8y-employee-details">
    <div class="row no-gutters">
		<div class="col-md-3 mb-3 mb-md-0">
    	    <div class="border bg-light rounded p-3">
				<asp:Image id="imagePhoto" runat="server" CssClass="img-fluid d-block mx-auto" />
				<asp:Panel id="panelContacts" runat="server" CssClass="u8y-employee-contacts">
					<div class="_section">
    					<asp:HyperLink id="linkEmail" runat="server" CssClass="email _email" />
        				<asp:HyperLink id="linkSecondaryEmail" runat="server" CssClass="email _email" />
        				<asp:HyperLink id="linkWebSite" runat="server" Target="_blank" CssClass="_website" />
                        <asp:HyperLink id="linkUserProfile" runat="server" resourcekey="VisitProfile.Text" Target="_blank" CssClass="_userprofile more" />
        			</div>
                    <div class="_section">
        				<asp:Label id="labelMessenger" runat="server" CssClass="_label" />
        			</div>
        			<div class="_section">
        				<asp:Label id="labelPhone" runat="server" CssClass="_label" />
        				<asp:Label id="labelFax" runat="server" CssClass="_label" />
        				<asp:Label id="labelCellPhone" runat="server" CssClass="_label" />
        			</div>
                    <div class="_section">
        				<asp:Label id="labelWorkingPlaceAndHours" runat="server" CssClass="_label" />
        			</div>
				</asp:Panel>
				<% if (Employee != null && Employee.ShowBarcode) { %>
				<button type="button" class="btn btn-outline-secondary btn-block btn-sm btn-barcode" data-toggle="modal" data-target="#u8y_employee_barcode_dlg_<%: ModuleId %>">
					<i class="fas fa-qrcode mr-2"></i><%: LocalizeString ("BarcodeButtonLabel") %>
				</button>
				<% } %>
				<button type="button" class="btn btn-outline-secondary btn-block btn-sm" data-toggle="modal" data-target="#u8y_employee_exporter_dlg_<%: ModuleId %>">
					<i class="fas fa-file-export mr-2"></i><%: LocalizeString ("EmployeeExporterButtonLabel") %>
				</button>
			</div>
		</div>
    	<div id="employeeTabs_<%= ModuleId %>" class="col-md-9 pl-md-3">
            <asp:Literal id="literalFullName" runat="server" />
    		<ul class="nav nav-pills u8y-employee-details-tabs" role="tablist">
    		    <li class="nav-item"><a class="nav-link active" role="tab" data-toggle="pill" href="#employeeCommon_<%= ModuleId %>" aria-controls="employeeCommon_<%= ModuleId %>" aria-selected="true"><%= LocalizeString("Common.Tab") %></a></li>
    			<li class="nav-item" id="tabExperience" runat="server"><a class="nav-link" role="tab" data-toggle="pill" href="#employeeExperience_<%= ModuleId %>" aria-controls="employeeExperience_<%= ModuleId %>" aria-selected="false"><%= LocalizeString("Experience.Tab") %></a></li>
    			<li class="nav-item" id="tabAchievements" runat="server"><a class="nav-link" role="tab" data-toggle="pill" href="#employeeAchievements_<%= ModuleId %>" aria-controls="employeeAchievements_<%= ModuleId %>" aria-selected="false"><%= LocalizeString("Achievements.Tab") %></a></li>
    			<li class="nav-item" id="tabDisciplines" runat="server"><a class="nav-link" role="tab" data-toggle="pill" href="#employeeDisciplines_<%= ModuleId %>" aria-controls="employeeDisciplines_<%= ModuleId %>" aria-selected="false"><%= LocalizeString("Disciplines.Tab") %></a></li>
    			<li class="nav-item" id="tabAbout" runat="server"><a class="nav-link" role="tab" data-toggle="pill" href="#employeeAbout_<%= ModuleId %>" aria-controls="employeeAbout_<%= ModuleId %>" aria-selected="false"><%= LocalizeString("About.Tab") %></a></li>
    		</ul>
			<div class="tab-content">
        		<div id="employeeCommon_<%= ModuleId %>" class="tab-pane fade show active" role="tabpanel">
                    <p><asp:Label id="labelAcademicDegreeAndTitle" runat="server" /></p>
					<asp:Panel id="panelPositions" runat="server" CssClass="_section">
                        <label><%: LocalizeString ("OccupiedPositions.Text") %></label>
            			<asp:Repeater id="repeaterPositions" runat="server" OnItemDataBound="repeaterPositions_ItemDataBound">
            				<HeaderTemplate><ul></HeaderTemplate>
            				<ItemTemplate>
            					<li>
            						<asp:Label id="labelPosition" runat="server" />
            						<asp:Label id="labelDivision" runat="server" />
            						<asp:HyperLink id="linkDivision" runat="server" Target="_blank" />
            					</li>
            				</ItemTemplate>
            				<FooterTemplate></ul></FooterTemplate>
            			</asp:Repeater>
						<asp:Panel id="pnlScienceIndexCounter" runat="server" CssClass="u8y-science-index-counter d-inline-block border rounded p-2">
							<!--Science Index counter-->
							<script type="text/javascript"><!--
							document.write('<a href="https://elibrary.ru/author_counter_click.asp?id=<%: (Employee != null)? Employee.ScienceIndexAuthorId : 0 %>"'+
							' target=_blank><img src="https://elibrary.ru/author_counter.aspx?id=<%: (Employee != null)? Employee.ScienceIndexAuthorId : 0 %>&rand='+
							Math.random()+'" title="<%: LocalizeString ("ScienceIndexAuthorProfile.Text") %>" border="0" '+
							'height="31" width="88" border="0"><\/a>')
							//--></script>
							<!--/Science Index counter-->
						</asp:Panel>
				    </asp:Panel>
				</div>
        		<div id="employeeExperience_<%= ModuleId %>" class="tab-pane fade" role="tabpanel">
        			<asp:Label id="labelExperienceYears" runat="server" CssClass="_label" />
        			<div class="table-responsive">
        				<asp:GridView id="gridExperience" runat="server" AutoGenerateColumns="false" CssClass="table table-sm table-striped table-bordered table-hover grid-experience"
                            UseAccessibleHeader="true" OnRowCreated="grid_RowCreated" GridLines="None">
    						<Columns>
                                <asp:BoundField DataField="Years_String" HeaderText="Years.Column" />
                                <asp:BoundField DataField="Title_Link" HeaderText="Title.Column" HtmlEncode="false" />
                                <asp:BoundField DataField="AchievementType_String" HeaderText="AchievementType.Column" />
                                <asp:BoundField DataField="DocumentUrl_Link" HeaderText="DocumentUrl.Column" HtmlEncode="false" />
                            </Columns>
        			    </asp:GridView>
        			</div>
        		</div>
        		<div id="employeeAchievements_<%= ModuleId %>" class="tab-pane fade" role="tabpanel">
        			<div class="table-responsive">
        				<asp:GridView id="gridAchievements" runat="server" AutoGenerateColumns="false" CssClass="table table-sm table-striped table-bordered table-hover grid-achievements"
        			        UseAccessibleHeader="true" OnRowCreated="grid_RowCreated" GridLines="None">
    						<Columns>
                                <asp:BoundField DataField="Years_String" HeaderText="Years.Column" />
                                <asp:BoundField DataField="Title_Link" HeaderText="Title.Column" HtmlEncode="false" />
                                <asp:BoundField DataField="AchievementType_String" HeaderText="AchievementType.Column" />
                                <asp:BoundField DataField="DocumentUrl_Link" HeaderText="DocumentUrl.Column" HtmlEncode="false" />
                            </Columns>
        			    </asp:GridView>
        			</div>
        		</div>
        		<div id="employeeDisciplines_<%= ModuleId %>" class="tab-pane fade" role="tabpanel">
                    <div class="table-responsive">
                        <asp:GridView id="gridDisciplines" runat="server" AutoGenerateColumns="false" CssClass="table table-sm table-striped table-bordered table-hover grid-disciplines"
                            UseAccessibleHeader="true" OnRowCreated="grid_RowCreated" GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="EduProgramProfile_String" HeaderText="EduProgramProfile.Column" />
                                <asp:BoundField DataField="EduLevel_String" HeaderText="EduLevel.Column" />
                                <asp:BoundField DataField="Disciplines" HeaderText="Disciplines.Column" />
                            </Columns>
                        </asp:GridView>
                    </div>
        			<asp:Literal id="litDisciplines" runat="server" />
        		</div>
        		<div id="employeeAbout_<%= ModuleId %>" class="tab-pane fade u8y-employee-about" role="tabpanel">
        			<asp:Literal id="litAbout" runat="server" />
        		</div>
			</div>
    	</div>
    </div>
    <ul class="dnnActions dnnClear" style="margin-bottom:1em">
		<li>
            <asp:HyperLink id="linkEdit" runat="server" role="button" CssClass="btn btn-primary" Visible="false">
                <i class="fas fa-pencil-alt" aria-hidden="true"></i>
                <%: LocalizeString ("cmdEdit") %>
            </asp:HyperLink>
        </li>
		<li>
			<asp:HyperLink id="linkReturn" runat="server" role="button" CssClass="btn btn-secondary">
			    <i class="fas fa-times" aria-hidden="true"></i>
				<%: LocalizeString ("Close.Text") %>
			</asp:HyperLink>
		</li>
    </ul>
	<controls:AgplSignature id="agplSignature" runat="server" ShowRule="true" />
</asp:Panel>
<div id="u8y_employee_barcode_dlg_<%: ModuleId %>" class="modal fade" role="dialog" tabindex="-1" aria-labelledby="u8y_employee_barcode_dlg_title_<%: ModuleId %>">
    <div class="modal-dialog modal-sm" role="document">
	    <div class="modal-content">
	        <div class="modal-header">
				<h5 id="u8y_employee_barcode_dlg_title_<%: ModuleId %>" class="modal-title"><asp:Label id="labelBarcodeEmployeeName" runat="server" /></h5>
			    <button type="button" class="close" data-dismiss="modal" aria-label='<%: LocalizeString("Close") %>'><span aria-hidden="true">&times;</span></button>
			</div>
			<div class="modal-body">
				<p><%: LocalizeString ("BarcodeInfo.Text") %></p>
				<asp:Image id="imageBarcode" runat="server" CssClass="img-thumbnail d-block mx-auto" />
			</div>
        </div>
	</div>
</div>
<div id="u8y_employee_exporter_dlg_<%: ModuleId %>" class="modal fade" role="dialog" tabindex="-1" aria-labelledby="u8y_employee_exporter_dlg_title_<%: ModuleId %>">
    <div class="modal-dialog modal-lg" role="document">
	    <div class="modal-content">
	        <div class="modal-header">
				<h5 id="u8y_employee_exporter_dlg_title_<%: ModuleId %>" class="modal-title"><%: LocalizeString("EmployeeExporterDialogTitle") %></h5>
			    <button type="button" class="close" data-dismiss="modal" aria-label='<%: LocalizeString("Close") %>'><span aria-hidden="true">&times;</span></button>
			</div>
			<div class="modal-body">
				<div class="react-root"
					 data-module-id="<%: ModuleId %>"
					 data-employee-id="<%: (Employee != null)? Employee.EmployeeID : 0 %>"
					 data-is-admin='<%: (UserInfo.IsSuperUser || UserInfo.IsInRole ("Administrators")).ToString().ToLowerInvariant() %>'
					 data-resources="<%: EmployeeExporterResources %>">
				</div>
				<div class="g-recaptcha"
					 data-sitekey="<%: UniversityConfig.Instance.Recaptcha.SiteKey %>"
					 data-callback="u8y_employee_exporter_recaptchaVerifiedCallback_<%: ModuleId %>"
					 data-expired-callback="u8y_employee_exporter_recaptchaExpiredCallback_<%: ModuleId %>"
					 data-error-callback="u8y_employee_exporter_recaptchaErrorCallback_<%: ModuleId %>"></div>
			</div>
        </div>
	</div>
</div>
<script>
(function($, window, document) {
	$(document).ready(function() {
		$("#u8y_employee_exporter_dlg_<%: ModuleId %>").on("shown.bs.modal", function (e) {
			var root = $(e.target).find(".react-root");
			var moduleId = root.data("module-id");
			var props = {
				moduleId: moduleId,
				employeeId: root.data("employee-id"),
				isAdmin: root.data("is-admin"),
				resources: root.data("resources")
			};
			window["u8y_employee_exporter_<%: ModuleId %>"] = ReactDOM.render(
		  		React.createElement(EmployeeExporter, props, null), root.get(0)
			);
		});
	});
} (jQuery, window, document));
</script>