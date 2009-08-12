<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<List<string>>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content ContentPlaceHolderId="childContent" runat="server">
	You are tracking packages:<br />
	<% int i = 0; %>
	<% foreach( string trackingNumber in Model ) { %>
		<%= i++ %> <%= trackingNumber %><br />
	<% } %>
</asp:Content>