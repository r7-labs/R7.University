﻿<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditDocuments.ascx.cs" Inherits="R7.University.Controls.EditDocuments" %>
<%@ Register TagPrefix="dnn" TagName="Url" Src="~/controls/DnnUrlControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/assets/css/grid-and-form.css" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/assets/css/admin.css" Priority="200" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University.Controls/js/editDocuments.js" ForceProvider="DnnFormBottomProvider" />
<asp:Panel id="panelEditDocuments" runat="server" CssClass="u8y-edit-documents">
    <fieldset>
		<div class="dnnFormItem" style="width:auto;margin-right:1.5em">
            <asp:GridView id="gridItems" runat="server" AutoGenerateColumns="false" CssClass="dnnGrid u8y-gaf-grid" GridLines="None">
                <HeaderStyle CssClass="dnnGridHeader" horizontalalign="Left" />
                <RowStyle CssClass="dnnGridItem" horizontalalign="Left" />
                <AlternatingRowStyle CssClass="dnnGridAltItem" />
                <SelectedRowStyle CssClass="dnnFormError" />
                <EditRowStyle CssClass="dnnFormInput" />
                <FooterStyle CssClass="dnnGridFooter" />
                <PagerStyle CssClass="dnnGridPager" />
                <Columns>
                    <asp:TemplateField>
                       <ItemTemplate>
                            <span class="u8y-gaf-actions">
                                <asp:LinkButton id="linkEdit" runat="server" OnCommand="OnEditItemCommand" ToolTip='<%# LocalizeString ("EditItem.Title") %>'>
                                    <asp:Image runat="server" ImageUrl="<%# EditIconUrl %>" />
                                </asp:LinkButton>
                                <asp:LinkButton id="linkDelete" runat="server" OnCommand="OnDeleteItemCommand" ToolTip='<%# LocalizeString ("DeleteItem.Title") %>'>
                                    <asp:Image runat="server" ImageUrl="<%# DeleteIconUrl %>" />
                                </asp:LinkButton>
                                <asp:LinkButton id="linkUndelete" runat="server" OnCommand="OnUndeleteItemCommand" ToolTip='<%# LocalizeString ("UndeleteItem.Title") %>'>
                                    <asp:Image runat="server" ImageUrl="<%# UndeleteIconUrl %>" />
                                </asp:LinkButton>
                                <asp:Label id="labelEditMarker" runat="server" />
                            </span>
                       </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ViewItemID" />
                    <asp:BoundField DataField="LocalizedType" HeaderText="DocumentType.Column" />
                    <asp:BoundField DataField="Title" HeaderText="DocumentTitle.Column" />
                    <asp:BoundField DataField="Group" HeaderText="DocumentGroup.Column" />
                    <asp:BoundField DataField="FormattedUrl" HeaderText="DocumentUrl.Column" HtmlEncode="false" />
					<asp:BoundField DataField="SortIndex" HeaderText="DocumentSortIndex.Column" />
                    <asp:BoundField DataField="StartEndDates" HeaderText="DocumentStartEndDates_Column.Text" />
                </Columns>
            </asp:GridView>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="labelDocumentType" runat="server" ControlName="comboDocumentType" />
            <asp:DropDownList id="comboDocumentType" runat="server"  CssClass="dnn-select2"
                DataTextField="LocalizedType"
                DataValueField="DocumentTypeID"
            />
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="labelDocumentUrl" runat="server" ControlName="urlDocumentUrl" />
            <dnn:Url id="urlDocumentUrl" runat="server"
				ShowNone="true"
				ShowFiles="true"
				ShowTabs="true"
				IncludeActiveTab="true"
				ShowUrls="false"
				ShowLog="false"
				ShowTrack="false"
                ShowNewWindow="false"
            />
			<asp:CustomValidator id="valDocumentUrl" runat="server" ValidationGroup="Documents"
                Display="Dynamic" CssClass="dnnFormMessage dnnFormError"
				EnableClientScript="true" ClientValidationFunction="validateDocumentUrl" />
			<div class="dnnFormItem">
	            <dnn:Label id="lblDocumentUrl2" runat="server" ControlName="txtDocumentUrl" />
				<asp:TextBox id="txtDocumentUrl" runat="server" />
			</div>
        </div>
		<div class="dnnFormItem">
            <dnn:Label id="labelDocumentTitle" runat="server" ControlName="textDocumentTitle" />
            <asp:TextBox id="textDocumentTitle" runat="server" MaxLength="255" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="labelDocumentGroup" runat="server" ControlName="textDocumentGroup" />
            <asp:TextBox id="textDocumentGroup" runat="server" MaxLength="255" />
        </div>
        <h2 class="dnnFormSectionHead"><a href="#"><%: LocalizeString ("sectionAdvancedProperties.Text") %></a></h2>
        <fieldset>
			<div class="dnnFormItem">
            	<dnn:Label id="labelDocumentSortIndex" runat="server" ControlName="textDocumentSortIndex" />
                <asp:TextBox id="textDocumentSortIndex" runat="server" Value="0" />
                <asp:RegularExpressionValidator runat="server" resourcekey="DocumentSortIndex.Invalid"
                    ControlToValidate="textDocumentSortIndex" ValidationGroup="Documents"
                    Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationExpression="^-?\d+$" />
            </div>
	        <div class="dnnFormItem">
                <dnn:Label ID="labelDocumentStartDate" runat="server" ControlName="datetimeDocumentStartDate" />
                <dnn:DnnDateTimePicker id="datetimeDocumentStartDate" runat="server" />
            </div>
            <div class="dnnFormItem">
                <dnn:Label ID="labelDocumentEndDate" runat="server" ControlName="datetimeDocumentEndDate" />
                <dnn:DnnDateTimePicker id="datetimeDocumentEndDate" runat="server" />
            </div>
        </fieldset>
		<div class="dnnFormItem">
            <div class="dnnLabel"></div>
			<ul class="dnnActions">
                <li>
					<asp:LinkButton id="buttonAddItem" runat="server" resourcekey="buttonAddDocument"
                        CssClass="btn btn-sm btn-success" CommandArgument="Add"
                        CausesValidation="true" ValidationGroup="Documents" />
				</li>
                <li>
					<asp:LinkButton id="buttonUpdateItem" runat="server" resourcekey="buttonUpdateDocument"
                        CssClass="btn btn-sm btn-primary" CommandArgument="Update"
                        CausesValidation="true" ValidationGroup="Documents" />
				</li>
				<li>&nbsp;</li>
                <li>
					<asp:LinkButton id="buttonCancelEditItem" runat="server" resourcekey="CancelEdit"
                        CssClass="btn btn-sm btn-outline-secondary" />
				</li>
				<li>&nbsp;</li>
    			<li>
					<asp:LinkButton id="buttonResetForm" runat="server" resourcekey="ResetForm"
                    CssClass="btn btn-sm btn-outline-secondary" />
				</li>
			</ul>
        </div>
        <asp:HiddenField id="hiddenViewItemID" runat="server" />
    </fieldset>
</asp:Panel>
<script type="text/javascript">
(function($, Sys) {
    function setupEditDocuments() {
        $("[id $= '_panelEditDocuments']").dnnPanels({defaultState: "closed"});
    };
    $(document).ready(function() {
        setupEditDocuments();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function() {
            setupEditDocuments();
        });
    });
} (jQuery, window.Sys));
</script>
