<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="MvcContrib.Samples.UI.Views.SampleFluentHtmlViewPage<Person>" %>
<%@ Import Namespace="MvcContrib.Samples.UI.Models"%>
<%@ Import Namespace="MvcContrib.FluentHtml" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>Fluent HTML</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div class="fluentHtmlContainer">

		<p>These were the values that were submitted:</p>
	
		<%=this.Literal(x => x.Name).Label("Name:") %><br/><br/>
		<%=this.Literal(x => x.Gender).Label("Gender:") %><br/><br/>
		<label>Roles:</label>
		<div>
			<% foreach (var role in Model.Roles) { %>
				<%= role %><br />
			<% } %>
		</div><br /><br/>
		<%=this.Literal(x => x.DateOfBirth).Label("Date Of Birth:").Format("MMMM d, yyyy") %><br/><br/>
		<% this.RenderPartial("ViewParent", x => x.Mother, new ViewDataDictionary { { "label", "Mother's Name:" } }); %><br /><br />
		<% this.RenderPartial("ViewParent", x => x.Father, new ViewDataDictionary { { "label", "Father's Name:" } }); %><br/>
	
	</div>
	
</asp:Content>