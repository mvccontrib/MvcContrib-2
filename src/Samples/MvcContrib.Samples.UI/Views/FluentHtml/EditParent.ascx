<%@ Control Language="C#" Inherits="MvcContrib.FluentHtml.ModelViewUserControl<Parent>" %>
<%@ Import Namespace="MvcContrib.FluentHtml"%>
<%@ Import Namespace="MvcContrib.Samples.UI.Models"%>

<div>
	<%=this.TextBox(x => x.Name).Label(ViewData["label"].ToString())%>
</div>