﻿<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditEduForms.ascx.cs" Inherits="R7.University.Controls.EditEduForms" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/assets/css/admin.css" Priority="200" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University.Controls/css/grid-and-form.css" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University.Controls/js/gridAndForm.js" ForceProvider="DnnFormBottomProvider" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University.Controls/js/editEduForms.js" ForceProvider="DnnFormBottomProvider" />
<div class="dnnForm dnnClear u8y-edit-eduforms">
    <fieldset>
        <div class="dnnFormItem">
            <asp:GridView id="gridItems" runat="server" AutoGenerateColumns="false" CssClass="dnnGrid u8y-gaf-grid" GridLines="None">
                <HeaderStyle CssClass="dnnGridHeader" HorizontalAlign="Left" />
                <RowStyle CssClass="dnnGridItem" HorizontalAlign="Left" />
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
                    <asp:BoundField DataField="EduFormTitleLocalized" HeaderText="EduFormTitle.Column" />
                    <asp:BoundField DataField="TimeToLearnYears_String" HeaderText="TimeToLearnYears.Column" />
					<asp:BoundField DataField="TimeToLearnMonths_String" HeaderText="TimeToLearnMonths.Column" />
					<asp:BoundField DataField="TimeToLearnHours_String" HeaderText="TimeToLearnHours.Column" />
                    <asp:CheckBoxField DataField="IsAdmissive" HeaderText="IsAdmissive.Column" />
                </Columns>
            </asp:GridView>
        </div>
		<asp:ValidationSummary runat="server" EnableClientScript="true" ValidationGroup="EduProgramProfileForms" CssClass="dnnFormMessage dnnFormWarning" />
        <div class="dnnFormItem">
            <dnn:Label id="labelEduForm" runat="server" ControlName="radioEduForm" />
            <asp:RadioButtonList id="radioEduForm" runat="server"
                DataTextField="TitleLocalized"
                DataValueField="EduFormID"
                RepeatDirection="Horizontal"
                CssClass="dnn-form-control"
            />
			<asp:CustomValidator runat="server" resourcekey="EduForm.Invalid" ControlToValidate="radioEduForm"
                ValidationGroup="EduProgramProfileForms" EnableClientScript="true" ClientValidationFunction="eduFormUniqueValidator.validate"
                Display="Dynamic" CssClass="dnnFormMessage dnnFormError"  />
        </div>
	    <div class="dnnFormItem">
            <dnn:Label id="labelTimeToLearnYears" runat="server" ControlName="textTimeToLearnYears" />
            <asp:TextBox id="textTimeToLearnYears" runat="server" Value="0" />
			<asp:RequiredFieldValidator runat="server" resourcekey="TimeToLearnYears.Required"
                ControlToValidate="textTimeToLearnYears" ValidationGroup="EduProgramProfileForms"
                Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
			<asp:RangeValidator runat="server" resourcekey="TimeToLearnYears.Invalid"
                ControlToValidate="textTimeToLearnYears" ValidationGroup="EduProgramProfileForms"
                Type="Integer" MinimumValue="0" MaximumValue="11"
                Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
			<asp:CustomValidator runat="server" resourcekey="TimeToLearn.Required"
			    ControlToValidate="textTimeToLearnYears"
                ValidationGroup="EduProgramProfileForms" EnableClientScript="true" ClientValidationFunction="validateTimeToLearn"
                Display="None" CssClass="dnnFormMessage dnnFormError"  />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="labelTimeToLearnMonths" runat="server" ControlName="textTimeToLearnMonths" />
			<asp:TextBox id="textTimeToLearnMonths" runat="server" Value="0" />
			<asp:RequiredFieldValidator runat="server" resourcekey="TimeToLearnMonths.Required"
                ControlToValidate="textTimeToLearnMonths" ValidationGroup="EduProgramProfileForms"
                Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
			<asp:RangeValidator runat="server" resourcekey="TimeToLearnMonths.Invalid"
                ControlToValidate="textTimeToLearnMonths" ValidationGroup="EduProgramProfileForms"
                Type="Integer" MinimumValue="0" MaximumValue="11"
                Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
		</div>
	    <div class="dnnFormItem">
            <dnn:Label id="labelTimeToLearnHours" runat="server" ControlName="textTimeToLearnHours" />
            <asp:TextBox id="textTimeToLearnHours" runat="server" Value="0" />
			<asp:RequiredFieldValidator runat="server" resourcekey="TimeToLearnHours.Required"
                ControlToValidate="textTimeToLearnHours" ValidationGroup="EduProgramProfileForms"
                Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
            <asp:RangeValidator runat="server" resourcekey="TimeToLearnHours.Invalid"
                ControlToValidate="textTimeToLearnHours" ValidationGroup="EduProgramProfileForms"
                Type="Integer" MinimumValue="0" MaximumValue="2147483647"
                Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="labelIsAdmissive" runat="server" ControlName="checkIsAdmissive" />
            <asp:CheckBox id="checkIsAdmissive" runat="server" CssClass="dnn-form-control" />
        </div>
        <div class="dnnFormItem">
            <div class="dnnLabel"></div>
			<ul class="dnnActions">
				<li>
                    <asp:LinkButton id="buttonAddItem" runat="server" resourcekey="buttonAddEduForm"
                        CssClass="btn btn-sm btn-primary" CommandArgument="Add"
                        CausesValidation="true" ValidationGroup="EduProgramProfileForms" />
				</li>
                <li>
				    <asp:LinkButton id="buttonUpdateItem" runat="server" resourcekey="buttonUpdateEduForm"
                        CssClass="btn btn-sm btn-primary" CommandArgument="Update"
                        CausesValidation="true" ValidationGroup="EduProgramProfileForms" />
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
		<asp:HiddenField id="hiddenEduFormID" runat="server" />
    </fieldset>
</div>
