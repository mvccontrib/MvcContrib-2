<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Person>>" %>
<%@ Import Namespace="MvcContrib.Samples.UI.Models"%>
<%@ Import Namespace="MvcContrib.UI.Grid"%>
<%@ Import Namespace="MvcContrib.UI.Grid.ActionSyntax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>Grid Actions Sections</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Grid with Custom Actions Based Sections Example</h2>
    <p>
    Note that to use actions, you must render the grid using &lt% Html.Grid(...).Render(); %&gt 
    and must import the <strong>MvcContrib.UI.Grid.ActionSyntax</strong> namespace.
    </p>
    
	<% Html.Grid(Model)
		.Columns(column => {
     		column.For(x => x.Id).Named("Person ID");
     		column.For(x => x.Name);
     		column.For(x => x.Gender);
     		column.For(x => x.DateOfBirth).Format("{0:d}").HeaderAction(()=>
     			{
     				%><th style="font-weight: bold; background-color: Yellow">Date of Birth</th><%
     			});
			column.For("View Person").Named("").Action(p =>
				{
                    %><td style="font-weight:bold"><% %>
	                <%= Html.ActionLink("View Person", "Show", new { id = p.Id })%>
	                </td><%
				}


				);
     	}).RowStart((p,row)  =>
     		{
                //For demo reasons only. You really should style the gridrow_alternate class
                if (row.IsAlternate)
                {
                    %><tr style="background-color:#CCDDCC"><%
                                                           
                }
                else
                {
                    %><tr><%
                	
                }
     		}  
         ).Render(); %>

</asp:Content>
