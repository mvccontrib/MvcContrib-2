<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="System.Web.Mvc" %>
<asp:Content ContentPlaceHolderId="childContent" runat="server">
Whoops an error occured! 
<p>Error Message:</p>
<p><%= ((HandleErrorInfo)Model).Exception.GetBaseException().Message %></p>
</asp:Content>