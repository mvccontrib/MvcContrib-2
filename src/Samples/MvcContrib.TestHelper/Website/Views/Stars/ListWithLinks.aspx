<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<List<Star>>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
<%@ Import Namespace="MvcContrib.TestHelper.Sample.Controllers" %>
<h2>Stars</h2>

<ul>
    <% foreach (var star in Model) { %>
        <li>
            <%= star.Name %> approx: <%= star.Distance %> AU <%= this.Html.ActionLink<StarsController>(c => c.ListWithLinks(), "Nearby Stars")%>
        </li>
    <% } %>
</ul>

</asp:Content>