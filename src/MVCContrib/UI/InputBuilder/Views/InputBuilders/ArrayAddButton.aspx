<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<PropertyViewModel<IEnumerable<TypeViewModel>>>" %>
<%@ Import Namespace="MvcContrib.UI.InputBuilder.Views"%>
<button id="clone<%=Model.Name%>" type="button">Add</button>