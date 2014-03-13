<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditEmployee.ascx.cs" Inherits="R7.University.Launchpad.EditEmployee" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Audit" Src="~/controls/ModuleAuditControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Url" Src="~/controls/URLControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Picker" Src="~/controls/filepickeruploader.ascx" %> 
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="dnnweb" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>

<script type="text/javascript">
	$(function() { $( "#employeeTabs" ).dnnTabs(); });
</script>

<div class="dnnForm dnnClear">
	<div id="employeeTabs" class="dnnForm dnnClear">
	<ul class="dnnAdminTabNav dnnClear">
	    <li><a href="#employeeCommon">Common</a></li>
	    <li><a href="#employeeAbout">About</a></li>
	    <li><a href="#employeePositions">Positions</a></li>
	    <li><a href="#employeeAchivements">Achivements</a></li>
	</ul>
	<br /><br />
	<div id="employeeCommon">
		<fieldset>
			<div class="dnnFormItem">
				<dnn:Label id="labelPhoto" runat="server" ControlName="pickerPhoto" Suffix=":" />
				<dnn:Picker id="pickerPhoto" runat="server" Required="true" />
				<%-- <dnn:Url id="urlPhoto" runat="server" UrlType="F" 
				        ShowFiles="true" ShowTabs="false"
				        ShowUrls="true" ShowUsers="false"
						ShowLog="false" ShowTrack="false"
						ShowNone="true" ShowNewWindow="false" /> --%>
			</div>
			<%-- <div class="dnnFormItem">
				<dnn:Label id="labelUser" runat="server" ControlName="urlUser" Suffix=":" />
				<dnn:Url id="urlUser" runat="server" UrlType="F" 
				        ShowFiles="false" ShowTabs="false"
				        ShowUrls="false" ShowUsers="true"
						ShowLog="false" ShowTrack="false"
						ShowNone="true" ShowNewWindow="false" />      
			</div> --%>
			<div class="dnnFormItem">
				<dnn:Label id="labelUser" runat="server" ControlName="comboUsers" Suffix=":" />
				<%-- <asp:DropDownList id="listUsers" runat="server" /> --%>
				<dnnweb:DnnComboBox id="comboUsers" runat="server" />
			</div>
			<div class="dnnFormItem">
				<dnn:Label id="labelUserLookup" runat="server" ControlName="textUserLookup" Suffix=":" /> 
				<asp:TextBox id="textUserLookup" runat="server" Style="width:200px" />
				<asp:LinkButton id="buttonUserLookup" runat="server" resourcekey="buttonUserLookup" 
					CssClass="dnnSecondaryAction" OnClick="buttonUserLookup_Click" />
				<asp:CheckBox id="checkIncludeDeletedUsers" runat="server" resourcekey="checkIncludeDeletedUsers" />
			</div>

			<%--
			<div class="dnnFormItem">
				<div class="dnnLabel"></div>

				<asp:Label id="labelUserNames" runat="server" Visible="false" CssClass="dnnFormMessage dnnFormInfo" />
			</div> --%>

			<div class="dnnFormItem">
				<dnn:Label id="labelLastName" runat="server" ControlName="textLastName" Suffix=":" />
				<asp:TextBox id="textLastName" runat="server" />
			</div>
			<div class="dnnFormItem">
				<dnn:Label id="labelFirstName" runat="server" ControlName="textFirstName" Suffix=":" />
				<asp:TextBox id="textFirstName" runat="server" />
			</div>
			<div class="dnnFormItem">
				<dnn:Label id="labelOtherName" runat="server" ControlName="textOtherName" Suffix=":" />
				<asp:TextBox id="textOtherName" runat="server" />
			</div>
			<div class="dnnFormItem">
				<dnn:Label id="labelAcademicDegree" runat="server" ControlName="textAcademicDegree" Suffix=":" />
				<asp:TextBox id="textAcademicDegree" runat="server" />
			</div>
			<div class="dnnFormItem">
				<dnn:Label id="labelAcademicTitle" runat="server" ControlName="textAcademicTitle" Suffix=":" />
				<asp:TextBox id="textAcademicTitle" runat="server" />
			</div>
			<div class="dnnFormItem">
				<dnn:Label id="labelNamePrefix" runat="server" ControlName="textNamePrefix" Suffix=":" />
				<asp:TextBox id="textNamePrefix" runat="server" />
			</div>
			<div class="dnnFormItem">
				<dnn:Label id="labelPhone" runat="server" ControlName="textPhone" Suffix=":" />
				<asp:TextBox id="textPhone" runat="server" />
			</div>
			<div class="dnnFormItem">
				<dnn:Label id="labelCellPhone" runat="server" ControlName="textCellPhone" Suffix=":" />
				<asp:TextBox id="textCellPhone" runat="server" />
			</div>
			<div class="dnnFormItem">
				<dnn:Label id="labelFax" runat="server" ControlName="textFax" Suffix=":" />
				<asp:TextBox id="textFax" runat="server" />
			</div>
			<div class="dnnFormItem">
				<dnn:Label id="labelEmail" runat="server" ControlName="textEmail" Suffix=":" />
				<asp:TextBox id="textEmail" runat="server" />
			</div>
			<div class="dnnFormItem">
				<dnn:Label id="labelSecondaryEmail" runat="server" ControlName="textSecondaryEmail" Suffix=":" />
				<asp:TextBox id="textSecondaryEmail" runat="server" />
			</div>
			<div class="dnnFormItem">
				<dnn:Label id="labelWebSite" runat="server" ControlName="textWebSite" Suffix=":" />
				<asp:TextBox id="textWebSite" runat="server" />
			</div>
			<div class="dnnFormItem">
				<dnn:Label id="labelMessenger" runat="server" ControlName="textMessenger" Suffix=":" />
				<asp:TextBox id="textMessenger" runat="server" />
			</div>
			<div class="dnnFormItem">
				<dnn:Label id="labelWorkingPlace" runat="server" ControlName="textWorkingPlace" Suffix=":" />
				<asp:TextBox id="textWorkingPlace" runat="server" />
			</div>
			<div class="dnnFormItem">
				<dnn:Label id="labelWorkingHours" runat="server" ControlName="textWorkingHours" Suffix=":" />
				<asp:TextBox id="textWorkingHours" runat="server" />
			</div>
			<div class="dnnFormItem">
				<dnn:Label id="labelExperienceYears" runat="server" ControlName="textExperienceYears" Suffix=":" />
				<asp:TextBox id="textExperienceYears" runat="server" />
			</div>
			<div class="dnnFormItem">
				<dnn:Label id="labelExperienceYearsBySpec" runat="server" ControlName="textExperienceYearsBySpec" Suffix=":" />
				<asp:TextBox id="textExperienceYearsBySpec" runat="server" />
			</div>
			<div class="dnnFormItem">
				<dnn:Label id="labelIsPublished" runat="server" ControlName="checkIsPublished" Suffix=":" />
				<asp:CheckBox id="checkIsPublished" runat="server" />
			</div>
			<%-- <div class="dnnFormItem">
				<dnn:Label id="labelIsDeleted" runat="server" ControlName="checkIsDeleted" Suffix=":" />
				<asp:CheckBox id="checkIsDeleted" runat="server" />
			</div> --%>
		</fieldset>
	</div>

	<div id="employeeAbout">
		<fieldset>
			<div class="dnnFormItem">
				<%-- <dnn:Label id="labelBiography" runat="server" ControlName="textBiography" Suffix=":" /> --%>
				<div style="margin-right:20px">
					<dnn:TextEditor id="textBiography" runat="server" Width="100%" Height="300px" />
				</div>
			</div>
		</fieldset>
	</div>

	<div id="employeePositions">
		<fieldset>
			<div class="dnnFormItem">
				<dnn:Label id="labelDivisions" runat="server" ControlName="treeDivisions" Suffix=":" />
				<dnnweb:DnnTreeView id="treeDivisions" runat="server" Style="float:left;display:block;margin-bottom:10px;padding:10px;background-color:#EEE"
					DataTextField="ShortTitle"
					DataValueField="DivisionID"
					DataFieldParentID="ParentDivisionID"
				/>
			</div>

			<div class="dnnFormItem">
				<dnn:Label id="labelPositions" runat="server" ControlName="comboPositions" Suffix=":" />
				<dnnweb:DnnComboBox id="comboPositions" runat="server" />
			</div>

			<div class="dnnFormItem">
				<dnn:Label id="labelIsPrime" runat="server" ControlName="checkIsPrime" Suffix=":" />
				<asp:CheckBox id="checkIsPrime" runat="server" />
			</div>

			<div class="dnnFormItem">
				<div class="dnnLabel"></div>
				<asp:LinkButton id="buttonAddOccupiedPosition" runat="server" resourcekey="buttonAddOccupiedPosition" 
					CssClass="dnnSecondaryAction" OnClick="buttonAddOccupiedPosition_Click" />
			</div>

			<%-- 
			<div class="dnnFormItem">
				<div class="dnnLabel"></div>
				<dnnweb:DnnListBox id="listOccupiedPositions" runat="server" ></dnnweb:DnnListBox>
				<asp:DataList id="listOccupiedPositions" runat="server" RepeatLayout="Flow" OnItemDataBound="listOccupiedPositions_ItemDataBound">
					<ItemTemplate>
						<asp:Label id="labelDivisionTitle" runat="server" />
						<asp:Label id="labelPositionTitle" runat="server" />
						<asp:HyperLink id="linkDeleteOccupiedPosition" runat="server">
							<asp:Image runat="server" ImageUrl="/images/action_delete.gif" />
						</asp:HyperLink>
	 				</ItemTemplate>
				</asp:DataList>
			</div> --%>

			<div class="dnnFormItem">
				<div class="dnnLabel"></div>
				<asp:GridView id="gridOccupiedPositions" runat="server" AutoGenerateColumns="true" 
					GridLines="None" OnRowDataBound="gridOccupiedPositions_RowDataBound">
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
				                	<asp:LinkButton id="linkDeleteOccupiedPosition" runat="server" OnCommand="linkDeleteOccupiedPosition_Command" >
				                		<asp:Image runat="server" ImageUrl="~/images/delete.gif" />
				                	</asp:LinkButton>
				               </ItemTemplate>
				        	</asp:TemplateField>
						</Columns>
			        </asp:GridView>
			</div>
		</fieldset>
	</div>

	<div id="employeeAchivements">
		<fieldset>
		</fieldset>
	</div>

	<%--
	<h2 class="dnnFormSectionHead"><a href="" ><asp:Label runat="server" ResourceKey="sectionTest.Text" /></a></h2>
	<fieldset>
		<div class="dnnFormItem">
			<div class="dnnLabel"></div>
			<asp:TextBox id="textTest" runat="server" />
		</div>
	</fieldset>
	--%>

	<ul class="dnnActions dnnClear">
		<li><asp:LinkButton id="buttonUpdate" runat="server" CssClass="dnnPrimaryAction" ResourceKey="cmdUpdate" CausesValidation="true" /></li>
		<li><asp:LinkButton id="buttonDelete" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdDelete" /></li>
		<li><asp:HyperLink id="linkCancel" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdCancel" /></li>
	</ul>
	<hr />
	<dnn:Audit id="ctlAudit" runat="server" />
</div>

