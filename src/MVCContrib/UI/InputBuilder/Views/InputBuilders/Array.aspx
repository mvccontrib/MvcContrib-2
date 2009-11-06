<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<PropertyViewModel<IEnumerable<TypeViewModel>>>" %>
<%@ Import Namespace="System.Linq"%>
<%@ Import Namespace="System.Collections.Generic"%>
<%@ Import Namespace="MvcContrib.UI.InputBuilder.Views"%>
<asp:Content ID="Content1" ContentPlaceHolderID="Label" runat="server">
<label for="<%=Model.Name%>"><%=Model.Label%></label></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Input" runat="server">
	<div><%
         	Html.InputFields(Model.Value); %></div>
</asp:Content>
