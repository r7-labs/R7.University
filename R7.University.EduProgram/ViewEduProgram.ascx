<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ViewEduProgram.ascx.cs" Inherits="R7.University.EduProgram.ViewEduProgram" %>
<asp:FormView id="formEduProgram" runat="server">
    <ItemTemplate>
        <strong>EduProgramID:</strong> <%# Eval ("EduProgramID") %><br />
        <strong>Code:</strong> <%# Eval ("Code") %><br />
        <strong>Title:</strong> <%# Eval ("Title") %>
    </ItemTemplate>
</asp:FormView>