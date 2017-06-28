<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewEmployeeDetails.ascx.cs" Inherits="R7.University.Employee.ViewEmployeeDetails" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="AgplSignature" Src="~/DesktopModules/MVC/R7.University/Controls/AgplSignature.ascx" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/css/module.css" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University.Employee/js/module.js" />
<asp:Panel id="panelEmployeeDetails" runat="server" CssClass="dnnForm dnnClear u8y-employee-details">
    <div class="employeeDetailsTable">
    	<asp:Image id="imagePhoto" runat="server" CssClass="img-rounded _photo" />	
    	<div id="employeeTabs_<%= ModuleId %>" class="dnnForm dnnClear _tabs">
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
                    <label><%: LocalizeString ("OccupiedPositions.Text") %></label>
        			<asp:Repeater id="repeaterPositions" runat="server" OnItemDataBound="repeaterPositions_ItemDataBound">
        				<HeaderTemplate><ul></HeaderTemplate>
        				<ItemTemplate>
        					<li>
        						<asp:Label id="labelPosition" runat="server" />
        						<asp:Label id="labelDivision" runat="server" />
        						<asp:HyperLink id="linkDivision" runat="server" />
        					</li>
        				</ItemTemplate>
        				<FooterTemplate></ul></FooterTemplate>
        			</asp:Repeater>
        			<div class="_section">
    					<label class="u8y-label-contacts"><%: LocalizeString ("Contacts.Text") %></label>
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
        		</div>
        		<div id="employeeExperience-<%= ModuleId %>" class="tab-pane fade">	
        			<asp:Label id="labelExperienceYears" runat="server" CssClass="_label" />
        			<div class="_section" style="margin-bottom:10px">
        				<asp:GridView id="gridExperience" runat="server" AutoGenerateColumns="false" CssClass="table table-stripped table-bordered table-hover grid-experience"
                            UseAccessibleHeader="true" OnRowCreated="grid_RowCreated" GridLines="None">
    						<Columns>
                                <asp:BoundField DataField="Years_String" HeaderText="Years" />
                                <asp:BoundField DataField="Title_Link" HeaderText="Title" HtmlEncode="false" />
                                <asp:BoundField DataField="AchievementType_String" HeaderText="AchievementType" />
                                <asp:BoundField DataField="DocumentUrl_Link" HeaderText="DocumentUrl" HtmlEncode="false" />
                            </Columns>
        			    </asp:GridView>
        			</div>
        		</div>
        		<div id="employeeAchievements-<%= ModuleId %>" class="tab-pane fade">
        			<div class="_section" style="margin-bottom:10px">
        				<asp:GridView id="gridAchievements" runat="server" AutoGenerateColumns="false" CssClass="table table-stripped table-bordered table-hover grid-achievements"
        			        UseAccessibleHeader="true" OnRowCreated="grid_RowCreated" GridLines="None">
    						<Columns>
                                <asp:BoundField DataField="Years_String" HeaderText="Years" />
                                <asp:BoundField DataField="Title_Link" HeaderText="Title" HtmlEncode="false" />
                                <asp:BoundField DataField="AchievementType_String" HeaderText="AchievementType" />
                                <asp:BoundField DataField="DocumentUrl_Link" HeaderText="DocumentUrl" HtmlEncode="false" />
                            </Columns>
        			    </asp:GridView>
        			</div>		
        		</div>
        		<div id="employeeDisciplines-<%= ModuleId %>" class="tab-pane fade">
                    <div class="_section">
                        <asp:GridView id="gridDisciplines" runat="server" AutoGenerateColumns="false" CssClass="table table-stripped table-bordered table-hover grid-disciplines" 
                            UseAccessibleHeader="true" OnRowCreated="grid_RowCreated" GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="EduProgramProfile_String" HeaderText="EduProgramProfile" />
                                <asp:BoundField DataField="EduLevel_String" HeaderText="EduLevel" />
                                <asp:BoundField DataField="Disciplines" HeaderText="Disciplines" />
                            </Columns>
                        </asp:GridView>
                    </div>
        			<asp:Literal id="litDisciplines" runat="server" />
        		</div>
        		<div id="employeeAbout-<%= ModuleId %>" class="tab-pane fade">
        			<asp:Literal id="litAbout" runat="server" />
        		</div>
			</div>	
    	</div>
    </div>
    <ul class="dnnActions dnnClear">
        <li><asp:HyperLink id="linkReturn" runat="server" CssClass="btn btn-primary" ResourceKey="cmdReturn" /></li>
        <li><asp:HyperLink id="linkVCard" runat="server" CssClass="btn btn-default" ResourceKey="VCard.Action" Visible="false" /></li>
        <li><asp:HyperLink id="linkBarcode" runat="server" resourcekey="Barcode.Action" 
            CssClass="btn btn-default" onclick="showEmployeeBarcodeDialog(this)" />
        </li>
        <li><asp:HyperLink id="linkEdit" runat="server" CssClass="btn btn-default" ResourceKey="cmdEdit" Visible="false" /></li>
    </ul>
	<controls:AgplSignature id="agplSignature" runat="server" ShowRule="false" />
</asp:Panel>
<div class="dialog-employee-achievement-description" id="dialog-employee-achievement-description-<%= ModuleId %>"></div>
<div class="dialog-employee-barcode" id="dialog-employee-barcode-<%= ModuleId %>">
    <asp:Image id="imageBarcode" runat="server" Style="margin-top:10px" />
    <asp:Label runat="server" resourcekey="BarcodeScan.Text" 
        CssClass="dnnFormMessage" Style="margin-top:10px;margin-bottom:0" />
</div>