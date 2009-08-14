<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="MvcContrib.Samples.UI.Views.SampleFluentHtmlViewPage<PersonEditModel>" %>
<%@ Import Namespace="MvcContrib.Samples.UI.Models"%>
<%@ Import Namespace="MvcContrib.FluentHtml" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>Fluent HTML</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	
	<% using(Html.BeginForm()) { %>
	
	<div class="fluentHtmlContainer">

		<h2>Fluent Html</h2>
		
		<p>
			The FluentHtml library allows you to define HTML elements using lambda expressions and a fluent interface. 
			The names and IDs of elements are automatically generated based on the lambda expression.
			As with the framework's built in HTML Helpers, databinding is automatic:
		</p>
		
		<%= this.TextBox(x => x.Person.Name).Title("Enter the person's name").Label("Name:") %><br />
		
		<p>
			Note that for the above textbox the 'maxlength' attribute was automatically set to 50 as the 'Person' class 
			has a <strong>RangeAttribute</strong> applied to the <strong>Name</strong> property.
		</p>
		
		<p>
			This is achieved by using a custom <strong>MemberBehavior</strong> which inspects the model object prior to rendering the element.
			See <strong>MaxLengthBehavior.cs</strong> for the code for this example. Note that behaviors have to be added to the underlying <strong>ModelViewPage</strong> instance.
			For this example the current view inherits from <strong>SampleFluentHtmlViewPage</strong> which defines two behaviors.
		</p>
		
		<p>
			HTML Select lists (Dropdowns/Listboxes) can be easily populated from Dictionary instances:
		</p>
		
		<p>
			...or SelectLists:
		</p>
		
		<%= this.Select(x => x.Person.EmployerId).Options(Model.Companies).Label("Employer:").FirstOption("Choose employer") %><br />
				
        <p>
			...or enumerations:
		</p>
		
		<%= this.Select(x => x.Person.FavoriteColor).Options<Color>().Label("Favorite Color:") %><br />
		
		<%= this.Select(x => x.Person.Gender).Options(Model.Genders).Size(5).Label("Gender:")
				.Title("Select the person's gender") %><br />
		
		<p>
			Checkbox/radio lists work in a similar way to dropdowns.  In this case it is populated from an Enumerable&lt;T&gt; where T is a complex type:
		</p>	
		
		<%= this.CheckBoxList(x => x.Person.Roles).Options(Model.Roles, x => x.Id, x => x.Name).Label("Roles:")
			   .ItemFormat("{0}<br />").Class("checkboxList").Title("Check all roles that apply to this person") %><br />
			   
		<p>
			FluentHtml supports server side validation.  Enter an invalid date and see what happens.
		</p>  
			   
		<%= this.TextBox(x => x.Person.DateOfBirth).Format("M/d/yy").Label("DOB:").Title("Enter the person's date of birth") %>
		<%= this.ValidationMessage(x => x.Person.DateOfBirth, "Please enter a valid date") %><br />
		<br />
		
        <p>
			FluentHtml lets you associate multiple instances of partial view with seperate model properties of the same type:
		</p>  
		
		<% this.RenderPartial("EditParent", x => x.Person.Mother, new ViewDataDictionary { { "label", "Mother's Name:" } }); %><br />
		<br />
		
		<% this.RenderPartial("EditParent", x => x.Person.Father, new ViewDataDictionary { { "label", "Father's Name:" } }); %><br />

		<p>
			<%=this.SubmitButton("Submit") %>
		</p>
	
	</div>
	
	<% } %>
</asp:Content>
