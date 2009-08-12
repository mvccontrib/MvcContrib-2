<%@ Page Language="C#" %>
<script runat="server">
protected void Page_Load(object sender, EventArgs e) {
	Response.Redirect("~/Home.mvc/Index");
}
</script>
<!-- Please do not delete this file.  It is used to ensure that ASP.NET MVC is activated by IIS when a user makes a "/" request to the server. -->