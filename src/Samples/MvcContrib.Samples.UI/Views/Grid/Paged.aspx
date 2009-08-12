<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IPagination<Person>>" %>
<%@ Import Namespace="MvcContrib.UI.Pager"%>
<%@ Import Namespace="MvcContrib.UI.Grid"%>
<%@ Import Namespace="MvcContrib.Samples.UI.Models"%>
<%@ Import Namespace="MvcContrib.Pagination"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>Paged Grid</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Paged Grid Example</h2>
	
	<%= Html.Grid(Model).Columns(column => {
     		column.For(x => x.Id).Named("Person ID");
     		column.For(x => x.Name);
     		column.For(x => x.Gender);
     		column.For(x => x.DateOfBirth).Format("{0:d}");
			column.For(x => Html.ActionLink("View Person", "Show", new { id = x.Id })).DoNotEncode();
     	}) %>

	
	<%= Html.Pager(Model) %>

</asp:Content>
