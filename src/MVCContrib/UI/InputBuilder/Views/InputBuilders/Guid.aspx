<%@ Page Title="" Language="C#" MasterPageFile="HiddenField.Master" Inherits="System.Web.Mvc.ViewPage<MvcContrib.UI.InputBuilder.PropertyViewModel<object>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Label" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Input" runat="server"><%=Html.Hidden(Model.Name,Model.Value) %></asp:Content>
