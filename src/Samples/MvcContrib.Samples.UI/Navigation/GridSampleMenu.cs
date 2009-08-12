using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcContrib.UI.MenuBuilder;

namespace MvcContrib.Samples.UI.Navigation
{
	using UI.Controllers;

	public class GridSampleMenu
	{
		public static MenuItem MainMenu(UrlHelper url)
		{
			Menu.DefaultIconDirectory = url.Content("~/Content/");
			Menu.DefaultDisabledClass = "disabled";
			Menu.DefaultSelectedClass = "selected";
			return Menu.Begin(
						Menu.Items("Grid Samples",
							Menu.Action<GridController>(c => c.Index(), "Simple Grid"),
							Menu.Action<GridController>(c => c.Paged(null), "Paged Grid"),
							Menu.Action<GridController>(c => c.UsingGridModel(), "Using a GridModel"),
							Menu.Action<GridController>(c => c.WithSections(), "Using Grid Sections"),
							Menu.Action<GridController>(c => c.WithActionSections(), "Using Grid Action Sections"),
							Menu.Action<GridController>(c => c.AutoColumns(), "Auto-Generated Columns")
						),

						Menu.Items("FluentHtml Samples", 
							Menu.Action<FluentHtmlController>(c=>c.Index(), "FluentHtml Samples")
						),

						Menu.Items("Secure",
							Menu.Secure<HomeController>(p => p.Index()),
							Menu.Secure<HomeController>(p => p.About()),
							Menu.Secure<HomeController>(p => p.SecurePage1()),
							Menu.Secure<HomeController>(p => p.SecurePage2())
						),
						Menu.Items("Insecure",
							Menu.Action<HomeController>(p => p.Index()),
							Menu.Action<HomeController>(p => p.About(), "About", "blarg.gif"),
							Menu.Action<HomeController>(p => p.SecurePage1()),
							Menu.Action<HomeController>(p => p.SecurePage2())
						),
						Menu.Items("Disabled",
							Menu.Action<HomeController>(p => p.Index(), "Disabled Index").SetDisabled(true).SetShowWhenDisabled(true),
							Menu.Action<HomeController>(p => p.About(), "Disabled About").SetDisabled(true).SetShowWhenDisabled(true)
						),
						Menu.Items("Direct",
							Menu.Link("http://microsoft.com", "Big Blue"),
							Menu.Link("http://google.com", "Google")
						),
						Menu.Items("Administrivia",
						Menu.Secure<AccountController>(p => p.LogOff()),
						Menu.Action<AccountController>(p => p.MagicLogOn()))
					).SetListClass("jd_menu");
		}
	}
}