using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using MvcContrib.TestHelper.Sample.Controllers;
using MvcContrib.TestHelper.Sample.Models;
using MvcContrib.TestHelper;

//This class is to demo the use of the framework
//by testing the controller located in MvcTestingFramework.Sample.
//The actual unit tests for the framework are located in MvcTestingFramework.Test

namespace MvcContrib.TestHelper.Sample
{
    [TestFixture]
    public class StarsControllerTest
    {
    	private StarsController _controller;
    	private TestControllerBuilder _builder;

    	[SetUp]
    	public void Setup()
    	{
    		_controller = new StarsController();	
			_builder = new TestControllerBuilder();
    		_builder.InitializeController(_controller);
		}

        [Test]
        public void ListControllerSelectsListView()
        {
            _builder.InitializeController(_controller);
        	_controller.List().AssertViewRendered().ForView("List");
        }

        [Test]
        public void AddFormStarShouldRedirectToList()
        {
        	_controller.AddFormStar().AssertActionRedirect().ToAction("List");
        }

        [Test]
        public void AddFormStarShouldSaveFormToTempData()
        {
            _builder.Form["NewStarName"] = "alpha c";
            _controller.AddFormStar();
			_controller.TempData["NewStarName"].ShouldEqual("alpha c", "");
        }

        [Test]
        public void AddSessionStarShouldSaveFormToSession()
        {
            _builder.Form["NewStarName"] = "alpha c";
            _controller.AddSessionStar();
			_controller.HttpContext.Session["NewStarName"].ShouldEqual("alpha c", "");
        }

        [Test]
        public void NearbyShouldRedirectToListWithLinks() //ok, really it should do something more useful, but you get the point
        {
            _controller.Nearby().AssertActionRedirect().ToAction("ListWithLinks"); 
        }
    }
}
