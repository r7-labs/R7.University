<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditScience.ascx.cs" Inherits="R7.University.EduPrograms.EditScience" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="AgplSignature" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/AgplSignature.ascx" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University.EduPrograms/admin.css" Priority="200" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/assets/css/module.css" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/assets/css/admin.css" />

<div class="dnnForm dnnClear u8y-edit-science">
    <div id="tabs" runat="server">
        <ul class="dnnAdminTabNav dnnClear">
            <li><a href="#<%= tabDirections.ClientID %>"><%= LocalizeString ("Directions.Tab") %></a></li>
			<li><a href="#<%= tabResults.ClientID %>"><%= LocalizeString ("Results_Tab.Text") %></a></li>
	        <li><a href="#<%= tabBase.ClientID %>"><%= LocalizeString ("Base.Tab") %></a></li>
        </ul>
    	<div id="tabDirections" runat="server">
        	<fieldset>
        		<div class="dnnFormItem">
                    <dnn:Label id="labelDirections" runat="server" ControlName="textDirections" />
        			<dnn:TextEditor id="textDirections" runat="server" ChooseMode="false" />
        		</div>
        	</fieldset>
    	</div>
	    <div id="tabResults" runat="server">
            <fieldset>
                <div class="dnnFormItem">
                    <dnn:Label id="lblResults" runat="server" ControlName="txtResults" />
                    <dnn:TextEditor id="txtResults" runat="server" ChooseMode="false" />
                </div>
            </fieldset>
        </div>
    	<div id="tabBase" runat="server">
            <fieldset>
                <div class="dnnFormItem">
                    <dnn:Label id="labelBase" runat="server" ControlName="textBase" />
                    <dnn:TextEditor id="textBase" runat="server" ChooseMode="false" />
                </div>
            </fieldset>
        </div>
    </div>
	<ul class="dnnActions dnnClear">
		<li><asp:LinkButton id="buttonUpdate" runat="server" CssClass="btn btn-primary" ResourceKey="cmdUpdate" CausesValidation="true" /></li>
		<li>&nbsp;</li>
		<li><asp:LinkButton id="buttonDelete" runat="server" CssClass="btn btn-danger" ResourceKey="cmdDelete" /></li>
		<li>&nbsp;</li>
		<li><asp:HyperLink id="linkCancel" runat="server" CssClass="btn btn-outline-secondary" ResourceKey="cmdCancel" /></li>
	</ul>
	<controls:AgplSignature runat="server" ShowRule="false" />
</div>
<script type="text/javascript">
(function($, Sys) {
    function setupModule() {
	    $("#<%= tabs.ClientID %>").dnnTabs();
	};
    $(document).ready(function() {
        setupModule();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function() {
            setupModule();
        });
    });
} (jQuery, window.Sys));
</script>
