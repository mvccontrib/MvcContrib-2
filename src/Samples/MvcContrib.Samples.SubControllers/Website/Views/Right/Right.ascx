<%@ Import Namespace="MvcContrib"%>
<%@ Control Language="C#" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewUserControl" %>
<div style="border:dotted 1px black">
<%=ViewData["text"] %>
</div>