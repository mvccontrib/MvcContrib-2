<%@ Page Language="C#" %>
<script runat="server">
protected void Page_Load(object sender, EventArgs e) {
    //For IIS6 Response.Redirect("~/Home.mvc/Index");
    Response.Redirect("~/Home/Index");
}
</script>