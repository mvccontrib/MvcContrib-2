<%@ Import Namespace="MvcContrib" %>
<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<div style="border: dotted 1px red;overflow:hidden">
<%=ViewData["text"] %>
	<div style=" float: left;">
		<% ViewData.Get<Action>("left").Invoke(); %>
	</div>
	<div style="float: right;">
		<% ViewData.Get<Action>("right").Invoke(); %>
	</div>
</div>
<% ViewData.Get<Action>("formsubmit").Invoke(); %>
