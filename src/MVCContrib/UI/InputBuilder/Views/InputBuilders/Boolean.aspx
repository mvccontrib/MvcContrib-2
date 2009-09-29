<%@ Page Title="" Language="C#" MasterPageFile="Field.Master" Inherits="System.Web.Mvc.ViewPage<PropertyViewModel<object>>" %>
<%@ Import Namespace="MvcContrib.UI.InputBuilder.Views"%>

<asp:Content ID="Content1" ContentPlaceHolderID="Label" runat="server"><label for="<%=Model.Name%>"><%=Model.Label%></label></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Input" runat="server"><%=Html.CheckBox(Model.Name,Model.Value) %></asp:Content>
