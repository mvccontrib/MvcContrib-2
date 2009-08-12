using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.Samples.UI.Models;

namespace MvcContrib.Samples.UI.Controllers
{
	public class FluentHtmlController : Controller
	{
		public ViewResult Index()
		{
			var model = GetViewModel(new Person
			{
				Gender = "M", 
				Name = "Jeremy", 
				Roles = new List<int> {1, 2},
				Father = new Parent {Name ="Jim"},
				Mother = new Parent { Name = "Joan" }
			});
			return View(model);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ViewResult Index(Person person)
		{
			return ModelState.IsValid 
				? View("ViewPerson", person)
				: View(GetViewModel(person));
		}

		private FluentHtmlViewData GetViewModel(Person person)
		{
			return new FluentHtmlViewData 
			{
				Person = person,
				Genders = new Dictionary<string, string> { { "M", "Male" }, { "F", "Female" } },
				Roles = new List<Role> { new Role(0, "Administrator"), new Role(1, "Developer"), new Role(2, "User")  },
			};
		}
	}

	public class FluentHtmlViewData
	{
		public Person Person;
		public IDictionary<string, string> Genders;
		public IEnumerable<Role> Roles;
	}

	
}