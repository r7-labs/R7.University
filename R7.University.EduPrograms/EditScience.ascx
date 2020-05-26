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
			<li><a href="#<%= tabBase.ClientID %>"><%= LocalizeString ("Base.Tab") %></a></li>
			<li><a href="#<%= tabValues.ClientID %>"><%= LocalizeString ("Values.Tab") %></a></li>
        </ul>
    	<div id="tabDirections" runat="server">
        	<fieldset>
        		<div class="dnnFormItem">
                    <dnn:Label id="labelDirections" runat="server" ControlName="textDirections" />
        			<dnn:TextEditor id="textDirections" runat="server" ChooseMode="false" />
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
    	<div id="tabValues" runat="server">
			<fieldset>
                <div class="dnnFormItem">
                    <dnn:Label id="labelScientists" runat="server" ControlName="textScientists" />
                    <asp:TextBox id="textScientists" runat="server" />
    				<asp:RangeValidator runat="server" ControlToValidate="textScientists"
    					Type="Integer" MinimumValue="0" MaximumValue="2147483647" ValidationGroup="Science" 
    					Display="Dynamic" CssClass="dnnFormMessage dnnFormError" resourcekey="Value.Invalid" />
                </div>
    			<div class="dnnFormItem">
                    <dnn:Label id="labelStudents" runat="server" ControlName="textStudents" />
                    <asp:TextBox id="textStudents" runat="server" />
                    <asp:RangeValidator runat="server" ControlToValidate="textStudents"
                        Type="Integer" MinimumValue="0" MaximumValue="2147483647" ValidationGroup="Science" 
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" resourcekey="Value.Invalid" />
                </div>
    			<div class="dnnFormItem">
                    <dnn:Label id="labelMonographs" runat="server" ControlName="textMonographs" />
                    <asp:TextBox id="textMonographs" runat="server" />
    				<asp:RangeValidator runat="server" ControlToValidate="textMonographs"
                        Type="Integer" MinimumValue="0" MaximumValue="2147483647" ValidationGroup="Science" 
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" resourcekey="Value.Invalid" />
                </div>
				<div class="dnnFormItem">
				    <dnn:Label id="labelArticles" runat="server" ControlName="textArticles" />
				</div>	
    			<div class="dnnFormItem">
                    <div class="dnnLabel text-muted"><%: LocalizeString ("ArticlesHAC.Text") %></div>
					<asp:TextBox id="textArticles" runat="server" />
					<asp:RangeValidator runat="server" ControlToValidate="textArticles"
                        Type="Integer" MinimumValue="0" MaximumValue="2147483647" ValidationGroup="Science" 
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" resourcekey="Value.Invalid" />
    			</div>
				<div class="dnnFormItem">
					<div class="dnnLabel text-muted"><%: LocalizeString ("ArticlesForeign.Text") %></div>
                    <asp:TextBox id="textArticlesForeign" runat="server" />
                    <asp:RangeValidator runat="server" ControlToValidate="textArticlesForeign"
                        Type="Integer" MinimumValue="0" MaximumValue="2147483647" ValidationGroup="Science" 
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" resourcekey="Value.Invalid" />
                </div>
				<div class="dnnFormItem">
				    <dnn:Label id="labelPatents" runat="server" ControlName="textPatents" />
				</div>	
    			<div class="dnnFormItem">
					<div class="dnnLabel text-muted"><%: LocalizeString ("Local.Text") %></div>
                    <asp:TextBox id="textPatents" runat="server" />
					<asp:RangeValidator runat="server" ControlToValidate="textPatents"
                        Type="Integer" MinimumValue="0" MaximumValue="2147483647" ValidationGroup="Science" 
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" resourcekey="Value.Invalid" />
                </div>
				<div class="dnnFormItem">
					<div class="dnnLabel text-muted"><%: LocalizeString ("Foreign.Text") %></div>
					<asp:TextBox id="textPatentsForeign" runat="server" />
					<asp:RangeValidator runat="server" ControlToValidate="textPatentsForeign"
                        Type="Integer" MinimumValue="0" MaximumValue="2147483647" ValidationGroup="Science" 
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" resourcekey="Value.Invalid" />
				</div>
				<div class="dnnFormItem">
				    <dnn:Label id="labelCertificates" runat="server" ControlName="textCertificates" />
				</div>
    			<div class="dnnFormItem">
					<div class="dnnLabel text-muted"><%: LocalizeString ("Local.Text") %></div>
                	<asp:TextBox id="textCertificates" runat="server" />
					<asp:RangeValidator runat="server" ControlToValidate="textCertificates"
                        Type="Integer" MinimumValue="0" MaximumValue="2147483647" ValidationGroup="Science" 
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" resourcekey="Value.Invalid" />
                </div>
				<div class="dnnFormItem">
					<div class="dnnLabel text-muted"><%: LocalizeString ("Foreign.Text") %></div>
				    <asp:TextBox id="textCertificatesForeign" runat="server" />
				    <asp:RangeValidator runat="server" ControlToValidate="textCertificatesForeign"
                        Type="Integer" MinimumValue="0" MaximumValue="2147483647" ValidationGroup="Science" 
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" resourcekey="Value.Invalid" />
				</div>
    			<div class="dnnFormItem">
                    <dnn:Label id="labelFinancingByScientist" runat="server" ControlName="textFinancingByScientist" />
                    <asp:TextBox id="textFinancingByScientist" runat="server" />
                    <asp:RangeValidator runat="server" ControlToValidate="textFinancingByScientist"
                        Type="Double" MinimumValue="0" MaximumValue="2147483647" ValidationGroup="Science" 
                        Display="Dynamic" CssClass="dnnFormMessage dnnFormError" resourcekey="FinancingByScientistValue.Invalid" />
                </div>
            </fieldset>
		</div>	
    </div>
	<ul class="dnnActions dnnClear">
		<li><asp:LinkButton id="buttonUpdate" runat="server" CssClass="dnnPrimaryAction" ResourceKey="cmdUpdate" CausesValidation="true" /></li>
		<li>&nbsp;</li>
		<li><asp:LinkButton id="buttonDelete" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdDelete" /></li>
		<li>&nbsp;</li>
		<li><asp:HyperLink id="linkCancel" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdCancel" /></li>
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