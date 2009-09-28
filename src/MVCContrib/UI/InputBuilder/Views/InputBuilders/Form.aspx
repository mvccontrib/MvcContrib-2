<%@ Page Title="" Language="C#"  Inherits="System.Web.Mvc.ViewPage<PropertyViewModel[]>" %>
<%@ Import Namespace="System.Web.Mvc.Html"%>
<%@ Import Namespace="MvcContrib.UI.Html"%>
<%@ Import Namespace="MvcContrib.UI.InputBuilder"%>

<%=Html.ValidationSummary()%>
<%
	using(Html.BeginForm())
	{%>
	<%
		foreach(PropertyViewModel model in Model)
		{%>
		<%
			Html.RenderPartial(model.PartialName, model, model.Layout);%>
	<%
		}%>
	<input type="submit" value="Submit" />
<%
	}%>