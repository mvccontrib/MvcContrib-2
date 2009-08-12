<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Person>" %>
<%@ Import Namespace="MvcContrib.Samples.UI.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>Show</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Details for: <%= Model.Name %></h2>

	<p><strong>Id:</strong> <%= Model.Id %> </p>
	<p><strong>Gender:</strong> <%= Model.Gender %> </p>
	<p><strong>Date of Birth:</strong> <%= Model.DateOfBirth %> </p>

</asp:Content>
