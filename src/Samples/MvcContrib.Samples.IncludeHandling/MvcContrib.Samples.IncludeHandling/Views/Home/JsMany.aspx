<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	JsMany
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<% Html.IncludeJs("~/content/js/lib/prototype.js"); %>
	<% Html.IncludeJs("~/content/js/lib/scriptaculous/effects.js"); %>
	<% Html.IncludeJs("~/content/js/lib/scriptaculous/controls.js"); %>
	<% Html.IncludeJs("~/content/js/lib/scriptaculous/builder.js"); %>
	<% Html.IncludeJs("~/content/js/lib/scriptaculous/dragdrop.js"); %>
	<% Html.IncludeJs("~/content/js/lib/scriptaculous/slider.js"); %>
	<% Html.IncludeJs("~/content/js/lib/scriptaculous/sound.js"); %>
	<% Html.IncludeJs("~/content/js/lib/scriptaculous/unittest.js"); %>

    <h2>JsMany</h2>

</asp:Content>
