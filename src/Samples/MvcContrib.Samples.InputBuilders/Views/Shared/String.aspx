<%@ Page Title="" Language="C#" MasterPageFile="~/Views/InputBuilders/Field.Master"  
Inherits="System.Web.Mvc.ViewPage<MvcContrib.UI.InputBuilder.PropertyViewModel<object>>" %>
<%@ Import Namespace="System.Web.Mvc.Html"%>
<asp:Content ID="Content2" ContentPlaceHolderID="Input" runat="server">
    <%=Html.TextBox(Model.Name,Model.Value) %>   From Webproject 
</asp:Content>
