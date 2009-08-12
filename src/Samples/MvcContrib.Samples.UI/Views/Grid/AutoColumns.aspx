<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Person>>" %>
<%@ Import Namespace="MvcContrib.Samples.UI.Models"%>
<%@ Import Namespace="MvcContrib.UI.Grid" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>Simple Grid Example</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Auto-Generated Columns</h2>
	<p>
		When you call <strong>AutoGenerateColumns()</strong> on the grid, the columns will be inferred based on the public properties of the model object.
	</p>


	<%= Html.Grid(Model).AutoGenerateColumns() %>
</asp:Content>
