<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.Web.Mvc.Html"%>
<%@ Import Namespace="LoginPortableArea.Login.Helpers"%>
<%
	if (Request.IsAuthenticated)
	{
%>
        Welcome <b><%=Html.Encode(Page.User.Identity.Name)%></b>!
        [ <%=Html.LogOffLink("Log Off") %> ]
<%
	}
	else
	{
%> 
        [ <%=Html.LoginLink("Log On")%> ]
<%
	}
%>
