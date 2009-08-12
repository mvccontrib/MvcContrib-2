using System;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using MvcContrib.TestHelper.Sample.Models;
using System.Collections.Generic;

namespace MvcContrib.TestHelper.Sample.Controllers
{
    public class StarsController : Controller
    {
        
        public ActionResult List()
        {
            List<Star> stars = StarDatabase.GetStars();
            return View("List", stars);
        }

        
        public ActionResult ListWithLinks()
        {
            List<Star> stars = StarDatabase.GetStarsAndLinks();
            return View("ListWithLinks", stars);
        }

        
        public ActionResult AddFormStar()
        {
            string name = Request.Form["NewStarName"];
            this.TempData["NewStarName"] = name;
            return RedirectToAction("List");
        }

        
        public ActionResult AddSessionStar()
        {
            string name = Request.Form["NewStarName"];
            this.HttpContext.Session["NewStarName"] = name;
            return RedirectToAction("List");
        }

        
        public ActionResult Nearby()
        {
            //Placeholder link for demonstration of link checking
            return RedirectToAction("ListWithLinks");
        }
    }
}
