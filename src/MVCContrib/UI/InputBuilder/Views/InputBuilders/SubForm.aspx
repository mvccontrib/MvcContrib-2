<%@ Page Title="" Language="C#"  Inherits="System.Web.Mvc.ViewPage<TypeViewModel>" %>
<%@ Import Namespace="MvcContrib.UI.InputBuilder.Views"%>
<%@ Import Namespace="System.Web.Mvc.Html"%>
<div><% Html.InputFields( Model.Properties );%></div>
