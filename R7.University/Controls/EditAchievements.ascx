<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditAchievements.ascx.cs" Inherits="R7.University.Controls.EditAchievements" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Url" Src="~/controls/DnnUrlControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/css/admin.css" Priority="200" />
<asp:Panel id="panelEditAchievements" runat="server" CssClass="dnnForm dnnClear u8y-edit-achievements">
    <fieldset>
        <div class="dnnFormItem">
            <asp:GridView id="gridItems" runat="server" AutoGenerateColumns="false" CssClass="dnnGrid u8y-gaf-grid"
                GridLines="None" OnRowDataBound="gridAchievements_RowDataBound">
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
                    <asp:BoundField DataField="Years_String" HeaderText="Years" />
                    <asp:BoundField DataField="Title_String" HeaderText="Title" />
                    <asp:BoundField DataField="AchievementType_String" HeaderText="AchievementType" />
                    <asp:CheckBoxField DataField="IsTitle" HeaderText="IsTitle" />
                    <asp:BoundField DataField="DocumentUrl_Link" HeaderText="DocumentUrl" HtmlEncode="false" />
                    <asp:BoundField DataField="Description" Visible="false" />
                </Columns>
            </asp:GridView>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="labelAchievements" runat="server" ControlName="comboAchievements" />
            <asp:DropDownList id="comboAchievement" runat="server" CssClass="dnn-ac-combobox"
                DataTextField="Text"
				DataValueField="Value"
                AutoPostBack="true"
                OnSelectedIndexChanged="comboAchievement_SelectedIndexChanged" />
        </div>
        <asp:Panel id="panelAchievementTypes" runat="server" class="dnnFormItem">
            <dnn:Label id="labelAchievementTypes" runat="server" ControlName="comboAchievementTypes" />
            <asp:DropDownList id="comboAchievementTypes" runat="server" 
                DataTextField="Text"
                DataValueField="Value" />
        </asp:Panel>
        <asp:Panel id="panelAchievementTitle" runat="server" class="dnnFormItem dnnFormRequired">
            <dnn:Label id="labelAchievementTitle" runat="server" ControlName="textAchievementTitle" />
            <asp:TextBox id="textAchievementTitle" runat="server" TextMode="MultiLine" Rows="3" />
            <asp:RequiredFieldValidator runat="server" resourcekey="AchievementTitle.Required" 
                ControlToValidate="textAchievementTitle" ValidationGroup="Achievements"
                Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
            <asp:RegularExpressionValidator runat="server"
                CssClass="dnnFormMessage dnnFormError" resourcekey="AchievementTitle.MaxLength"
                ControlToValidate="textAchievementTitle" Display="Dynamic"
                ValidationExpression="[\s\S]{0,250}" ValidationGroup="Achievements" />
        </asp:Panel>
        <div class="dnnFormItem">
            <dnn:Label id="labelYears" runat="server" ControlName="textYearBegin" />
            <div class="dnn-form-control-group">
                <asp:TextBox id="textYearBegin" runat="server" CssClass="dnn-form-control-half-width" />
                &ndash;
                <asp:TextBox id="textYearEnd" runat="server" CssClass="dnn-form-control-half-width" />
            </div>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="labelIsTitle" runat="server" ControlName="checkIsTitle" />
            <asp:CheckBox id="checkIsTitle" runat="server" CssClass="dnn-form-control" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="labelDocumentURL" runat="server" ControlName="urlDocumentURL" />
            <dnn:Url id="urlDocumentURL" runat="server" UrlType="N" 
                IncludeActiveTab="true"
                ShowFiles="true" ShowTabs="true"
                ShowUrls="true" ShowUsers="true"
                ShowLog="false" ShowTrack="false"
                ShowNone="true" ShowNewWindow="false" />      
        </div>
        <h2 class="dnnFormSectionHead dnnClear"><a href="#"><%: LocalizeString ("sectionAdvancedAchievementProperties.Text") %></a></h2>
        <fieldset>
            <asp:Panel id="panelAchievementShortTitle" runat="server" class="dnnFormItem">
                <dnn:Label id="labelAchievementShortTitle" runat="server" ControlName="textAchievementShortTitle" />
                <asp:TextBox id="textAchievementShortTitle" runat="server" MaxLength="64" />
            </asp:Panel>
            <div class="dnnFormItem">
                <dnn:Label id="labelAchievementTitleSuffix" runat="server" ControlName="textAchievementTitleSuffix" />
                <asp:TextBox id="textAchievementTitleSuffix" runat="server" MaxLength="100" />
            </div>
			<div class="dnnFormItem">
                <dnn:Label id="labelAchievementDescription" runat="server" ControlName="textAchievementDescription" />
                <asp:TextBox id="textAchievementDescription" runat="server" TextMode="MultiLine" Rows="3" />
            </div>
        </fieldset>
        <div class="dnnFormItem">
            <div class="dnnLabel"></div>
            <ul class="dnnActions">
                <li>
                    <asp:LinkButton id="buttonAddItem" runat="server" resourcekey="buttonAddAchievement" 
                        CssClass="dnnPrimaryAction" CommandArgument="Add"
                        CausesValidation="true" ValidationGroup="Achievements" />
                </li>   
                <li>
                    <asp:LinkButton id="buttonUpdateItem" runat="server" resourcekey="buttonUpdateAchievement" 
                        CssClass="dnnPrimaryAction" CommandArgument="Update"
                        CausesValidation="true" ValidationGroup="Achievements" />
                </li>   
                <li>
                    <asp:LinkButton id="buttonCancelEditItem" runat="server" resourcekey="CancelEdit" 
                        CssClass="dnnSecondaryAction" />
                </li>
                <li>&nbsp;</li>
                <li>
                    <asp:LinkButton id="buttonResetForm" runat="server" resourcekey="ResetForm" 
                    CssClass="dnnSecondaryAction" />
                </li>   
            </ul>   
        </div>
        <asp:HiddenField id="hiddenViewItemID" runat="server" />
	</fieldset>
</asp:Panel>
<script type="text/javascript">
(function($, Sys) {
    function setupEditAchievements() {
        $("[id $= 'panelEditAchievements']").dnnPanels({defaultState: "closed"});
    };
    $(document).ready(function() {
        setupEditAchievements();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function() {
            setupEditAchievements();
        });
    });
} (jQuery, window.Sys));
</script>
