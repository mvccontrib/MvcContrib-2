<%@ Page Title="" Language="C#"  Inherits="System.Web.Mvc.ViewPage<TypeViewModel>" %>
<%@ Import Namespace="MvcContrib.UI.InputBuilder.Views"%>
<%@ Import Namespace="System.Web.Mvc.Html"%>
<!-- SubForm -->
<%
	var name = Model.Name.Substring(0, Model.Name.IndexOf('['));
	var formName = Model.Name.Substring(0, Model.Name.IndexOf('[')) + Model.Index;
 %>
<div class="<%=formName%>" style="border-bottom: 1px dashed #cccccc;">
<input type="hidden" name="<%=name%>.Index" id="" value="<%=Model.Index %>" />
<% Html.InputFields( Model.Properties );%>
<%if (!(Model.AdditionalValues.ContainsKey("hidedeletebutton") && (bool)Model.AdditionalValues["hidedeletebutton"]))
  {%>
<div class="removeLink"><a href="#" class="removeLink<%=name %>">Remove</a></div>
<%} %>
</div>