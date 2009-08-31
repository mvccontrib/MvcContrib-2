using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.Samples.UI.Models;
using MvcContrib.Samples.UI.Services;

namespace MvcContrib.Samples.UI.Controllers
{
	public class FluentHtmlController : Controller
	{
		public ViewResult Index()
		{
			var model = GetEditModel(PersonService.GetPerson());
			return View(model);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ViewResult Index(Person person)
		{
			if (!ModelState.IsValid)
			{
				return View(GetEditModel(person));
			}
			PersonService.SavePerson(person);
			return View("ViewPerson", GetViewModel(person));
		}

		private PersonEditModel GetEditModel(Person person)
		{
			return new PersonEditModel
			{
				Person = person,
				Genders = new Dictionary<string, string> { { "M", "Male" }, { "F", "Female" } },
				Roles = new List<Role> { new Role(0, "Administrator"), new Role(1, "Developer"), new Role(2, "User") },
				Companies = new SelectList(CompanyService.GetCompanies(), "Id", "Name")
			};
		}

		private PersonViewModel GetViewModel(Person person)
		{
			return new PersonViewModel
			{
				Person = person,
				EmployerName = person.EmployerId.HasValue 
					? CompanyService.GetCompany(person.EmployerId.Value).Name
					: null
			};
		}
	}
}