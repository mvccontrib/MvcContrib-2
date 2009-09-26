<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	CssMany
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% Html.IncludeCss("~/content/css/Stylesheet1.css"); %>
<% Html.IncludeCss("~/content/css/Stylesheet2.css"); %>
<% Html.IncludeCss("~/content/css/Stylesheet3.css"); %>
<% Html.IncludeCss("~/content/css/Stylesheet4.css"); %>
<% Html.IncludeCss("~/content/css/Stylesheet5.css"); %>
<% Html.IncludeCss("~/content/css/Stylesheet6.css"); %>
    <h2>CssMany</h2>

</asp:Content>
