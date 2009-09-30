<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<SampleInput>" %>
<%@ Import Namespace="MvcContrib.UI.InputBuilder.Views"%>
<%@ Import Namespace="Web.Models"%>
<%@ Import Namespace="MvcContrib.UI.InputBuilder"%>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Html.Input( ) Sample</h2>
    <p>
    <%=Html.BeginForm("save","Home") %>
    <%=Html.ValidationSummary() %>
        This sample demonstrates how you could use partials and master pages to create your field inputs for your Strongly Typed View.
        <%=Html.Input(c => c.Name).Partial(Partial.ReadOnly)%>        
        <%=Html.Input(c => c.TimeStamp).Example("An Overridden example").Partial("DatePicker")%>
        <%=Html.Input(c => c.Guid)%>
        <%=Html.Input(c => c.Enum)%>
        <%=Html.Input(c => c.EnumAsRadioButton)%>
        <%=Html.Input(c => c.Html).Label("Label Overiden From the View")%>
        <%=Html.Input(c => c.IsNeeded)%>
        <%=Html.Input(c=>c.IntegerRangeValue) %>
        <%=Html.InputButtons()%>
        <%Html.EndForm();%>
    </p>
    
    <pre>
    &lt;%=Html.ValidationSummary() %&gt;
        This sample demonstrates how you could use partials and master pages to create your field inputs for your Strongly Typed View.
        &lt;%Html.BeginForm("save","home");%&gt;
        &lt;%=Html.Input(c => c.Name).Partial(Partial.ReadOnly)%&gt;
        &lt;%=Html.Input(c => c.TimeStamp)%&gt;
        &lt;%=Html.Input(c => c.TimeStamp).UsingPartial("DatePicker").WithExample("An Overridden example")%&gt;
        &lt;%=Html.Input(c => c.Guid)%&gt;
        &lt;%=Html.Input(c => c.Enum)%&gt;
        &lt;%=Html.Input(c => c.EnumAsRadioButton)%&gt;
        &lt;%=Html.Input(c => c.Html).WithLabel("Label Overiden From the View")%&gt;
        &lt;%=Html.Input(c => c.IsNeeded)%&gt;
        &lt;%=Html.Input(c=>c.IntegerRangeValue) %&gt;
        &lt;%=Html.InputButtons() %&gt;
        &lt;%Html.EndForm();%&gt;
    </pre>
    <%
          var enumerator = this.ViewContext.HttpContext.Cache.GetEnumerator();
          while (enumerator.MoveNext()) {
            Response.Write( enumerator.Key.ToString()+"<br/>");
            Response.Write(enumerator.Value.ToString() + "<br/>");
            }    
     %>
</asp:Content>
