<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="MvcContrib.Samples.UI.Views.SampleFluentHtmlViewPage<PersonViewModel>" %>
<%@ Import Namespace="MvcContrib.Samples.UI.Models"%>
<%@ Import Namespace="MvcContrib.FluentHtml" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>Fluent HTML</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div class="fluentHtmlContainer">

		<p>These were the values that were submitted:</p>
	
		<%=this.Literal(x => x.Person.Name).Label("Name:") %><br/><br/>
		<%=this.Literal(x => x.Person.Gender).Label("Gender:") %><br/><br/>
		<label>Roles:</label>
		<div>
			<% foreach (var role in Model.Person.Roles) { %>
				<%= role %><br />
			<% } %>
		</div><br /><br/>
		<%=this.Literal(x => x.Person.DateOfBirth).Label("Date Of Birth:").Format("MMMM d, yyyy") %><br/><br/>
		<% this.RenderPartial("ViewParent", x => x.Person.Mother, new ViewDataDictionary { { "label", "Mother's Name:" } }); %><br /><br />
		<% this.RenderPartial("ViewParent", x => x.Person.Father, new ViewDataDictionary { { "label", "Father's Name:" } }); %><br/><br />
		<%=this.Literal(x => x.EmployerName).Label("Employer:") %><br/><br />
		<%=this.Literal(x => x.Person.FavoriteColor).Label("Favorite Color:") %><br/>
	
	</div>
	
</asp:Content>