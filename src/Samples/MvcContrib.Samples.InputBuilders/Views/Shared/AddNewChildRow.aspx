<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<PropertyViewModel<IEnumerable<TypeViewModel>>>" %>
<%@ Import Namespace="MvcContrib.UI.InputBuilder"%>
<%@ Import Namespace="MvcContrib.UI.InputBuilder.Views"%>
<!-- AddNewChild-->
<button id="clone<%=Model.Name%>"  class="">Add</button>