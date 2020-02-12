<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="ViewEmployeeDetails.ascx.cs" Inherits="R7.University.Employees.ViewEmployeeDetails" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="AgplSignature" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/AgplSignature.ascx" %>

<%-- tell CDF what we are using lower Bootstrap version than actually used to give skin a preference --%>
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/bootstrap/css/bootstrap.min.css" Name="bootstrap" Version="3.0.0" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/bootstrap/css/bootstrap.theme.min.css" Name="bootstrap.theme" Version="3.0.0" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/bootstrap/js/bootstrap.min.js" Name="bootstrap" Version="3.0.0" ForceProvider="DnnFormBottomProvider" />

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/css/module.css" />

<asp:Panel id="panelEmployeeDetails" runat="server" CssClass="dnnForm dnnClear u8y-employee-details">
    <div class="media">
		<div class="media-left media-top">
    	    <div class="card card-body bg-light">
				<asp:Image id="imagePhoto" runat="server" />
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
				<asp:HyperLink id="linkBarcode" runat="server" resourcekey="Barcode.Action" role="button"
			        CssClass="btn btn-outline-secondary btn-block btn-sm btn-barcode" data-toggle="modal" />
			</div>
		</div>
    	<div id="employeeTabs_<%= ModuleId %>" class="media-body">
            <asp:Literal id="literalFullName" runat="server" />
    		<ul class="nav nav-tabs">
    		    <li class="active"><a href="#employeeCommon-<%= ModuleId %>" data-toggle="tab"><%= LocalizeString("Common.Tab") %></a></li>
    			<li id="tabExperience" runat="server"><a data-toggle="tab" href="#employeeExperience-<%= ModuleId %>"><%= LocalizeString("Experience.Tab") %></a></li>
    			<li id="tabAchievements" runat="server"><a data-toggle="tab" href="#employeeAchievements-<%= ModuleId %>"><%= LocalizeString("Achievements.Tab") %></a></li>
    			<li id="tabDisciplines" runat="server"><a data-toggle="tab" href="#employeeDisciplines-<%= ModuleId %>"><%= LocalizeString("Disciplines.Tab") %></a></li>
    			<li id="tabAbout" runat="server"><a data-toggle="tab" href="#employeeAbout-<%= ModuleId %>"><%= LocalizeString("About.Tab") %></a></li>
    		</ul>
			<div class="tab-content">
        		<div id="employeeCommon-<%= ModuleId %>" class="tab-pane fade in active">
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
				    </asp:Panel>
				</div>
        		<div id="employeeExperience-<%= ModuleId %>" class="tab-pane fade">
        			<asp:Label id="labelExperienceYears" runat="server" CssClass="_label" />
        			<div class="_section" style="margin-bottom:10px">
        				<asp:GridView id="gridExperience" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover grid-experience"
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
        		<div id="employeeAchievements-<%= ModuleId %>" class="tab-pane fade">
        			<asp:Panel id="pnlScienceIndexCounter" runat="server" CssClass="u8y-science-index-counter">
						<!--Science Index counter-->
						<script type="text/javascript"><!--
						document.write('<a href="https://elibrary.ru/author_counter_click.asp?id=<%: Employee.ScienceIndexAuthorId %>"'+
						' target=_blank><img src="https://elibrary.ru/author_counter.aspx?id=<%: Employee.ScienceIndexAuthorId %>&rand='+
						Math.random()+'" title="<%: LocalizeString ("ScienceIndexAuthorProfile.Text") %>" border="0" '+
						'height="31" width="88" border="0"><\/a>')
						//--></script>
						<!--/Science Index counter-->
					</asp:Panel>
					<div class="_section" style="margin-bottom:10px">
        				<asp:GridView id="gridAchievements" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover grid-achievements"
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
        		<div id="employeeDisciplines-<%= ModuleId %>" class="tab-pane fade">
                    <div class="_section">
                        <asp:GridView id="gridDisciplines" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover grid-disciplines"
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
        		<div id="employeeAbout-<%= ModuleId %>" class="tab-pane fade u8y-employee-about">
        			<asp:Literal id="litAbout" runat="server" />
        		</div>
			</div>
    	</div>
    </div>
    <ul class="dnnActions dnnClear" style="margin-bottom:1em">
		<li>
            <asp:HyperLink id="linkEdit" runat="server" role="button" CssClass="btn btn-primary" Visible="false">
                <span class="glyphicon glyphicon glyphicon-pencil" aria-hidden="true"></span>
                <%: LocalizeString ("cmdEdit") %>
            </asp:HyperLink>
        </li>
		<li>
			<asp:HyperLink id="linkReturn" runat="server" role="button" CssClass="btn btn-secondary">
			    <span class="glyphicon glyphicon glyphicon-remove" aria-hidden="true"></span>
				<%: LocalizeString ("Close.Text") %>
			</asp:HyperLink>
		</li>
    </ul>
	<controls:AgplSignature id="agplSignature" runat="server" ShowRule="true" />
</asp:Panel>
<div id="employee-barcode-dialog-<%: ModuleId %>" class="modal fade" role="dialog" aria-labelledby="employee-barcode-dialog-title-<%: ModuleId %>">
    <div class="modal-dialog modal-sm" role="document">
	    <div class="modal-content">
	        <div class="modal-header">
			    <button type="button" class="close" data-dismiss="modal" aria-label='<%: LocalizeString("Close") %>'><span aria-hidden="true">&times;</span></button>
				<h4 id="employee-barcode-dialog-title-<%: ModuleId %>" class="modal-title"><asp:Label id="labelBarcodeEmployeeName" runat="server" /></h4>
			</div>
			<div class="modal-body">
				<p><asp:Label runat="server" resourcekey="BarcodeScan.Text" /></p>
				<asp:Image id="imageBarcode" runat="server" CssClass="center-block" />
			</div>
        </div>
	</div>
</div>
