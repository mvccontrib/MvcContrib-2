<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Website.Views.Home.Index" %>
<%@ Import Namespace="MvcContrib"%>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= Html.Encode(ViewData["Message"]) %></h2>
    <p>
        To learn more about ASP.NET MVC visit <a href="http://asp.net/mvc" title="ASP.NET MVC Website">http://asp.net/mvc</a>.
    </p>
    <p>To see the below with querystring information, click
			<a href="/?number=999&name=Jeffrey%20Palermo&stringtodisplay=nesting and isolated viewdata">here</a></p>
    <div style="border:dotted 1px blue">
		<%=ViewData["text"] %>
		<% ViewData.Get<Action>("firstLevel").Invoke(); %>
		</div>
</asp:Content>
