<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	JsPrototype
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%= Html.IncludeJs("~/content/js/lib/prototype.js")%>
<%= Html.IncludeJs("~/content/js/lib/scriptaculous/scriptaculous.js")%>

    <h2>JsPrototype</h2>

</asp:Content>
