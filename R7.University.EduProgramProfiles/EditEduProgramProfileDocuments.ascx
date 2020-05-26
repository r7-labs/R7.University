<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditEduProgramProfileDocuments.ascx.cs" Inherits="R7.University.EduProgramProfiles.EditEduProgramProfileDocuments" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Audit" Src="~/controls/ModuleAuditControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="EditDocuments" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/EditDocuments.ascx" %>
<%@ Register TagPrefix="controls" TagName="AgplSignature" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/AgplSignature.ascx" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/assets/css/module.css" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/assets/css/admin.css" Priority="200" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University.EduProgramProfiles/admin.css" />

<div class="dnnForm dnnClear u8y-edit-eduprogramprofile-documents">
    <controls:EditDocuments id="formEditDocuments" runat="server" ForModel="EduProgramProfile" />
    <ul class="dnnActions dnnClear">
		<li><asp:LinkButton id="buttonUpdate" runat="server" CssClass="dnnPrimaryAction" ResourceKey="cmdUpdate" CausesValidation="true" ValidationGroup="EduProgramProfile" /></li>
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
	    $(".dnn-select2").select2();
    };
    $(document).ready(function() {
        setupModule();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function() {
            setupModule();
        });
    });
} (jQuery, window.Sys));
</script>
