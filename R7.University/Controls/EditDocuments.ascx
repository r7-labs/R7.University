<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditDocuments.ascx.cs" Inherits="R7.University.Controls.EditDocuments" %>
<%@ Register TagPrefix="dnn" TagName="Url" Src="~/controls/DnnUrlControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/R7.University/R7.University/Controls/js/editDocuments.js" ForceProvider="DnnFormBottomProvider" />

<div id="eduProgramDocuments" class="eduProgramDocuments">
    <fieldset>
		<div class="dnnFormItem" style="width:auto;margin-right:1.5em">
            <asp:GridView id="gridDocuments" runat="server" AutoGenerateColumns="false" 
					OnRowDataBound="gridDocuments_RowDataBound"
					CssClass="dnnGrid" GridLines="None" Style="width:100%;margin-bottom:30px">
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
                            <span style="white-space:nowrap">
                                <asp:LinkButton id="linkEdit" runat="server" OnCommand="OnEditItemCommand" >
                                    <asp:Image runat="server" ImageUrl="<%# EditIconUrl %>" />
                                </asp:LinkButton>
                                <asp:LinkButton id="linkDelete" runat="server" OnCommand="OnDeleteItemCommand" >
                                    <asp:Image runat="server" ImageUrl="<%# DeleteIconUrl %>" />
                                </asp:LinkButton>
                            </span>
                       </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ViewItemID" />
                    <asp:BoundField DataField="LocalizedType" HeaderText="DocumentType" />
                    <asp:BoundField DataField="Title" HeaderText="DocumentTitle" />
                    <asp:BoundField DataField="Group" HeaderText="DocumentGroup" />
                    <asp:BoundField DataField="FileName" HeaderText="DocumentFileName" />
                    <asp:BoundField DataField="SortIndex" HeaderText="DocumentSortIndex" />
                    <asp:BoundField DataField="StartDate" HeaderText="DocumentStartDate" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="EndDate" HeaderText="DocumentEndDate" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="FormattedUrl" HeaderText="DocumentUrl" HtmlEncode="false" />
                </Columns>
            </asp:GridView>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="labelDocumentType" runat="server" ControlName="comboDocumentType" />
            <asp:DropDownList id="comboDocumentType" runat="server" 
                DataTextField="LocalizedType"
                DataValueField="DocumentTypeID"
            />
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="labelDocumentUrl" runat="server" ControlName="urlDocumentUrl" />
            <dnn:Url id="urlDocumentUrl" runat="server" UrlType="N" 
                ShowNone="true" ShowFiles="true"
				ShowTabs="true" IncludeActiveTab="true"
				ShowUrls="true" ShowUsers="true"
                ShowLog="false" ShowTrack="false"
                ShowNewWindow="false" 
            />
			<asp:CustomValidator id="valDocumentUrl" runat="server" ValidationGroup="Documents"
                Display="Dynamic" CssClass="dnnFormMessage dnnFormError"
				EnableClientScript="true" ClientValidationFunction="validateDocumentUrl" />
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
            <asp:LinkButton id="buttonAddDocument" runat="server" resourcekey="buttonAddDocument" 
                CssClass="dnnPrimaryAction" CommandArgument="Add"
                CausesValidation="true" ValidationGroup="Documents" />
            <asp:LinkButton id="buttonUpdateDocument" runat="server" resourcekey="buttonUpdateDocument" 
                CssClass="dnnPrimaryAction" Visible="false" CommandArgument="Update"
                CausesValidation="true" ValidationGroup="Documents" />
            <asp:LinkButton id="buttonCancelEditDocument" runat="server" resourcekey="buttonCancelEditDocument" 
                CssClass="dnnSecondaryAction" />
        </div>
        <asp:HiddenField id="hiddenDocumentItemID" runat="server" />
    </fieldset>
</div>
<script type="text/javascript">
jQuery(document).ready(function () {
    $("#eduProgramDocuments").dnnPanels({defaultState: "closed"});
});
</script>
