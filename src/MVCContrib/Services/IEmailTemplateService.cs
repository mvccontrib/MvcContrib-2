using System.Net.Mail;
using System.Web.Mvc;

namespace MvcContrib.Services
{
    public interface IEmailTemplateService
    {
        MailMessage RenderMessage(ControllerContext controllerContext, string viewName);
    }
}
