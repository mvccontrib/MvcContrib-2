<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Jquery
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<% Html.IncludeJs("~/content/js/lib/jq/jquery.js"); %>
	<% Html.IncludeJs("~/content/js/lib/jq/jquery-ui.js"); %>

    <h2>Jquery</h2>

</asp:Content>
