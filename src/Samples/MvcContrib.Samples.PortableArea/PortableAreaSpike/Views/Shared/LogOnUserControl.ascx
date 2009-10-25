<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
	if (Request.IsAuthenticated)
	{
%>
        Welcome <b><%=Html.Encode(Page.User.Identity.Name)%></b>!
        [ <%=Html.ActionLink("Log Off", "Logoff", new {area = "login"}, null)%> ]
<%
	}
	else
	{
%> 
        [ <%=Html.ActionLink("Log On", "Index", "Login", new {area = "login"}, null)%> ]
<%
	}
%>
