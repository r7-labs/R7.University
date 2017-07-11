<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditEduForms.ascx.cs" Inherits="R7.University.Controls.EditEduForms" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/css/admin.css" Priority="200" />
<div class="dnnForm dnnClear university-edit-eduforms">
    <fieldset>
        <div class="dnnFormItem">
            <asp:GridView id="gridEduForms" runat="server" AutoGenerateColumns="false" CssClass="dnnGrid"
                GridLines="None" Style="margin-bottom:30px;width:775px">
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
                            <span style="white-space:nowrap">
                                <asp:LinkButton id="linkEdit" runat="server" OnCommand="OnEditItemCommand" >
                                    <asp:Image runat="server" ImageUrl="<%# EditIconUrl %>" />
                                </asp:LinkButton>
                                <asp:LinkButton id="linkDelete" runat="server" OnCommand="OnDeleteItemCommand" >
                                    <asp:Image runat="server" ImageUrl="<%# DeleteIconUrl %>" />
                                </asp:LinkButton>
                                <asp:LinkButton id="linkUndelete" runat="server" OnCommand="OnUndeleteItemCommand" >
                                    <asp:Image runat="server" ImageUrl="<%# UndeleteIconUrl %>" />
                                </asp:LinkButton>
								<asp:Label id="labelEditMarker" runat="server" />
                            </span>
                       </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ViewItemID" />
                    <asp:BoundField DataField="EduFormTitleLocalized" HeaderText="EduFormTitle" />
                    <asp:BoundField DataField="TimeToLearnString" HeaderText="TimeToLearn" />
					<asp:BoundField DataField="TimeToLearnHoursString" HeaderText="TimeToLearnHours" />
                    <asp:BoundField DataField="IsAdmissive" HeaderText="IsAdmissive" />
                </Columns>
            </asp:GridView>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="labelEduForm" runat="server" ControlName="radioEduForm" />
            <asp:RadioButtonList id="radioEduForm" runat="server" 
                DataTextField="TitleLocalized"
                DataValueField="EduFormID"
                RepeatDirection="Horizontal"
                CssClass="dnn-form-control"
            /> 
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="labelTimeToLearn" runat="server" ControlName="textTimeToLearnYears" />
            <div class="dnn-form-control-group">
                <asp:TextBox id="textTimeToLearnYears" runat="server" Value="0" 
                    CssClass="dnn-form-control-quarter-width" Style="margin-right:0.5em;" />
                <asp:Label runat="server" resourcekey="Years.Text" />
                <asp:TextBox id="textTimeToLearnMonths" runat="server" Value="0" 
                    CssClass="dnn-form-control-quarter-width" Style="margin-left:1em;margin-right:0.5em;" />
				<asp:Label runat="server" resourcekey="Months.Text" />
			</div>
            <asp:RangeValidator runat="server" resourcekey="TimeToLearnYears.Invalid"
                ControlToValidate="textTimeToLearnYears" ValidationGroup="EduProgramProfileForms" 
                Type="Integer" MinimumValue="0" MaximumValue="7"
                Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
            <asp:RequiredFieldValidator runat="server" resourcekey="TimeToLearnYears.Required"
                ControlToValidate="textTimeToLearnYears" ValidationGroup="EduProgramProfileForms" 
                Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
            <asp:RangeValidator runat="server" resourcekey="TimeToLearnMonths.Invalid"
                ControlToValidate="textTimeToLearnMonths" ValidationGroup="EduProgramProfileForms" 
                Type="Integer" MinimumValue="0" MaximumValue="11"
                Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
            <asp:RequiredFieldValidator runat="server" resourcekey="TimeToLearnMonths.Required"
                ControlToValidate="textTimeToLearnMonths" ValidationGroup="EduProgramProfileForms" 
                Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
        </div>
	    <div class="dnnFormItem">
            <dnn:Label id="labelTimeToLearnHours" runat="server" ControlName="textTimeToLearnHours" />
            <asp:TextBox id="textTimeToLearnHours" runat="server" Value="0" />
            <asp:RangeValidator runat="server" resourcekey="TimeToLearnHours.Invalid"
                ControlToValidate="textTimeToLearnHours" ValidationGroup="EduProgramProfileForms" 
                Type="Integer" MinimumValue="0" MaximumValue="99999"
                Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
            <asp:RequiredFieldValidator runat="server" resourcekey="TimeToLearnHours.Required"
                ControlToValidate="textTimeToLearnHours" ValidationGroup="EduProgramProfileForms" 
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
                    <asp:LinkButton id="buttonAddEduForm" runat="server" resourcekey="buttonAddEduForm" 
                        CssClass="dnnPrimaryAction" CommandArgument="Add" 
                        CausesValidation="true" ValidationGroup="EduProgramProfileForms" />
				</li>	
                <li>
				    <asp:LinkButton id="buttonUpdateEduForm" runat="server" resourcekey="buttonUpdateEduForm" 
                        CssClass="dnnPrimaryAction" CommandArgument="Update" 
                        CausesValidation="true" ValidationGroup="EduProgramProfileForms" />
				</li>
                <li>
				    <asp:LinkButton id="buttonCancelEditEduForm" runat="server" resourcekey="CancelEdit" 
                        CssClass="dnnSecondaryAction" />
				</li>
				<li>&nbsp;</li>
    			<li>
					<asp:LinkButton id="buttonResetForm" runat="server" resourcekey="ResetForm" 
                        CssClass="dnnSecondaryAction" />
				</li>
			</ul>	
        </div>
        <asp:HiddenField id="hiddenEduFormItemID" runat="server" />
    </fieldset>
</div>
