using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace LoginPortableArea.Login.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString LogOffLink(this HtmlHelper helper,
                                               string text)
        {
            return helper.RouteLink(text, "logoff");
        }

        public static MvcHtmlString LoginLink(this HtmlHelper helper,
                                              string text)
        {
            return helper.RouteLink(text, "login");
        }

        public static MvcHtmlString UserWidget(this HtmlHelper helper)
        {
            return helper.Action("UserWidget", "Login", new {area = "login"});
        }

        public static MvcHtmlString RssFeed(this HtmlHelper helper, string feed)
        {
            return helper.Action("Index", "Rss", new {area = "login", rssurl=feed});
        }
    }
}