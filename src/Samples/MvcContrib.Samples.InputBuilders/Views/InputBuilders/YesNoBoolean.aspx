<%@ Page Title="" Language="C#" 
Inherits="System.Web.Mvc.ViewPage<MvcContrib.UI.InputBuilder.Views.PropertyViewModel<object>>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Display" runat="server">
<%=(bool) Model.Value ? "Yes" : "No" %>
</asp:Content>
