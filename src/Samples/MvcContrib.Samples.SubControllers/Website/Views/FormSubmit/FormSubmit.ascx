<%@ Import Namespace="MvcContrib" %>
<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<div style="border:solid 2px yellow">
	<%using (Html.BeginForm())
	 { %>
		<%=Html.TextBox("sometextbox") %>
		<%=Html.SubmitButton() %>
	<%} %>
</div>
