<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<SampleDisplay>" %>
<%@ Import Namespace="MvcContrib.UI.InputBuilder.Views"%>
<%@ Import Namespace="Web.Models"%>
<%@ Import Namespace="MvcContrib.UI.InputBuilder"%>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Html.Display( ) Sample</h2>
    <p>
        This sample demonstrates how you could use partials and master pages to create your display Strongly Typed Views.
        <%=Html.Display(c => c.Name)%>        
        <%=Html.Label(c => c.TimeStamp)%> <%=Model.TimeStamp %>
        <%=Html.Display(c => c.Html).Label("Label Overiden From the View")%>
        <%=Html.Display(c => c.IsNeeded).Partial("YesNoBoolean")%>
    </p>
    
    <pre>
        This sample demonstrates how you could use partials and master pages to create your display Strongly Typed Views.
        &lt;%=Html.Display(c =&gt; c.Name)%&gt;        
        &lt;%=Html.Label(c =&gt; c.TimeStamp)%&gt; &lt;%=Model.TimeStamp %&gt;
        &lt;%=Html.Display(c =&gt; c.Html).Label("Label Overiden From the View")%&gt;
        &lt;%=Html.Display(c =&gt; c.IsNeeded).Partial("YesNoBoolean")%>
    </pre>
</asp:Content>
