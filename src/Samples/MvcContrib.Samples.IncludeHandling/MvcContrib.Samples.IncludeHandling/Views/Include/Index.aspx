<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IncludeIndexModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Include Index</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h2>Includes</h2>
	<table>
		<thead>
			<tr>
				<th>Type</th>
				<th>Last Modified At</th>
				<th>Source</th>
			</tr>
		</thead>
		<tbody>
			<% foreach (var include in Model.Includes)
			{%>
			<tr>
				<td><%= include.Type %></td>
				<td><%= include.LastModifiedAt %></td>
				<td><%= include.Source %></td>
			</tr>
			<%} %>
		</tbody>
	</table>
	
	<h2>Include Combinations</h2>
	<table>
		<thead>
			<tr>
				<th>Type</th>
				<th>Last Modified At</th>
				<th>Sources</th>
				<th>Key</th>
			</tr>
		</thead>
		<tbody>
			<% foreach (var kvp in Model.Combinations)
			{ %>
			<tr>
				<td><%= kvp.Value.Type %></td>
				<td><%= kvp.Value.LastModifiedAt %></td>
				<td>
					<ul>
						<% foreach (var source in kvp.Value.Sources)
				 { %>
						<li><%= source %></li>
						<%} %>
					</ul>
				</td>
				<td><%= Html.RouteLink(kvp.Key, "Default", new {controller="include",action=kvp.Value.Type.ToString(), id=kvp.Key}) %></td>
			</tr>
			<%} %>
		</tbody>
	</table>
</asp:Content>
