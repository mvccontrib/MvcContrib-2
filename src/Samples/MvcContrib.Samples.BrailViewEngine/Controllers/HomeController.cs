using System;
using System.Web.Mvc;

namespace MvcContrib.Samples
{
	public class HomeController : Controller
	{
		
		public ActionResult Index()
		{
			return View("Index", "Site");
		}

		
		public ActionResult Contact()
		{
			CompanyInfo companyInfo = new CompanyInfo();
			companyInfo.CompanyName = "Your company name here";
			companyInfo.AddressLine1 = "Company address Line 1";
			companyInfo.AddressLine2 = "Company address Line 2";
			companyInfo.City = "City";
			companyInfo.State = "State";
			companyInfo.Zip = "00000";
			companyInfo.Email = "email@yourcompany.com";
			
			return View("Contact", "Site", companyInfo);
		}

		
		public ActionResult About()
		{
			ViewData["now"] = DateTime.Now;

			return View("About", "Site");
		}
	}
}
