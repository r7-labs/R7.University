<%@ Control Language="C#" AutoEventWireup="false" Inherits="R7.University.Controls.AgplSignature" CodeBehind="AgplSignature.ascx.cs" %>
<div class="u8y-agpl-footer">
    <% if (Model.ShowRule) { %><hr /><% } %>
    <a href="https://github.com/roman-yagodin/R7.University" rel="nofollow" class="text-muted" target="_blank" title='<%= LocalizeString ("SourceLink.Title") %>'>
		<%: Model.Name %> v<%: Model.InformationalVersion ?? Model.Version.ToString (3) %>
	</a>
</div>