using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StructureMap;
using Website.Models;

namespace Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILinkRepository linkRepository;

        public HomeController(ILinkRepository repository)
        {
            this.linkRepository = repository;
        }

        public ActionResult Index()
        {
            IEnumerable<Link> links = linkRepository.GetLinks();
            return View("Index",links);
        }

        public ActionResult About()
        {
            return View("About");
        }
    }
}
