<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
    Inherits="System.Web.Mvc.ViewPage<HandleErrorInfo>" %>
<asp:Content ContentPlaceHolderId="childContent" runat="server">
You fail at math rescue says: 
<%= Model.Exception.GetBaseException().Message %>
</asp:Content>
