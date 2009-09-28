<%@ Page Title="" Language="C#" MasterPageFile="Field.Master"  Inherits="System.Web.Mvc.ViewPage<MvcContrib.UI.InputBuilder.PropertyViewModel<object>>" %>
<%@ Import Namespace="System.Web.Mvc.Html"%>
<asp:Content ID="Content2" ContentPlaceHolderID="Input" runat="server">
<%=Html.Encode(Model.Value) %>
</asp:Content>
<asp:Content ContentPlaceHolderID=Label runat="server">
<%=Html.Encode(Model.Label) %>:</asp:Content>
