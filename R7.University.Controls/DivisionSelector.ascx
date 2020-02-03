<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="DivisionSelector.ascx.cs" Inherits="R7.University.Controls.DivisionSelector" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>

<div class="dnn-form-control-group">
    <asp:RadioButtonList id="radioSelectionMode" runat="server"
        DataValueField="Value"
        DataTextField="ValueLocalized"
        RepeatDirection="Horizontal"
		RepeatLayout="Flow"
    	OnSelectedIndexChanged="radioSelectionMode_SelectedIndexChanged"
        AutoPostBack="true"
		Style="display:block;margin-bottom:.5em"
    />
    <asp:DropDownList id="comboDivision" runat="server"
        DataValueField="DivisionID"
        DataTextField="Title"
		CssClass="dnn-select2"
    />
    <dnn:DnnTreeView id="treeDivision" runat="server"
    	DataFieldID="DivisionID"
        DataFieldParentID="ParentDivisionID"
        DataTextField="Title"
        DataValueField="DivisionID"
		CssClass="full-width"
    />
</div>