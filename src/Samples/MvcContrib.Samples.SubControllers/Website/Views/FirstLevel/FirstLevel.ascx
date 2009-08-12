<%@ Import Namespace="MvcContrib"%>
<%@ Control Language="C#" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewUserControl" %>
<div style="border:dotted 1px green">
<%=ViewData["text"] %>
<% ViewData.Get<Action>("secondLevel").Invoke(); %>
</div>