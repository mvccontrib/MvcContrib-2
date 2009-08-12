<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<Link>>" %>
<%@ Import namespace="Website.Models"%>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h2>
        Introduction to IocControllerFactory!</h2>
    <p>
        
    </p>
    <p>
    <ul>
    <%foreach (Link link in this.Model)
      {%>
    <li><a target="_blank" href="<%=link.Url%>"><%=link.Title%></a> </li>
    <%}%>
    </ul>
        
    </p>
</asp:Content>
