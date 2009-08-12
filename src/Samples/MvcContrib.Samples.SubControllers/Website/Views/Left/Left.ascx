<%@ Import Namespace="MvcContrib"%>
<%@ Control Language="C#" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewUserControl" %>
<div style="border:dotted 1px purple">
<%=ViewData["text"] %>
<% ViewData.Get<Action>("right").Invoke(); %>
</div>