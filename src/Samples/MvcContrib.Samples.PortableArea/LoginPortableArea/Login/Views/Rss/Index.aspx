<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage< System.ServiceModel.Syndication.SyndicationFeed>" %>
    
    <ul>
<%foreach(var item in Model.Items) {%>
<li><b><a href="<%=item.Id %>"><%=item.Authors[0].Email %></a></b>-<%=item.Title.Text %></li>
<%} %>
</ul>