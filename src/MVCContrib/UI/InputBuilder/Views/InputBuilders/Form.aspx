<%@ Page Title="" Language="C#"  Inherits="System.Web.Mvc.ViewPage<PropertyViewModel[]>" %>
<%@ Import Namespace="MvcContrib.UI.InputBuilder.Views"%>
<%@ Import Namespace="System.Web.Mvc.Html"%>
<%=Html.ValidationSummary()%>
<%
	using(Html.BeginForm())
	{%>
		<%Html.RenderPartials(Model);%>
		<%=Html.SubmitButton() %>
<%	}%>