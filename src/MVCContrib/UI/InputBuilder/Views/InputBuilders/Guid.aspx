<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<PropertyViewModel<object>>" %>
<%@ Import Namespace="MvcContrib.UI.InputBuilder.Views"%>

<asp:Content ID="Content1" ContentPlaceHolderID="Label" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Input" runat="server"><%=Html.Hidden(Model.Name,Model.Value) %></asp:Content>
