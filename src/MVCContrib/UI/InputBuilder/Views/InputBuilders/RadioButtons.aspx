<%@ Page Title="" Language="C#" MasterPageFile="Field.Master" 
Inherits="System.Web.Mvc.ViewPage<PropertyViewModel<IEnumerable<SelectListItem>>>" %>
<%@ Import Namespace="MvcContrib.UI.InputBuilder.Views"%>

<asp:Content ID="Content1" ContentPlaceHolderID="Label" runat="server"><label for="<%=Model.Name%>"><%=Model.Label%></label></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Input" runat="server">
<%
    foreach (SelectListItem item in Model.Value)
    {%>
    
    <%=Html.RadioButton(Model.Name,item.Value,item.Selected,new{id=Model.Name+"_"+item.Value})%><label for="<%=Model.Name+"_"+item.Value %>" ><%=item.Text%></label>
<%  } %></asp:Content>        

