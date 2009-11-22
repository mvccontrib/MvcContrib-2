using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib;
using MvcContrib.ActionResults;
using MvcContrib.Attributes;
using MvcContrib.Samples.Models;
using MvcContrib.Filters;

namespace MvcContrib.Samples.Controllers
{
	//Note that the actual ConventionController class has now been removed.
	//The ConventionController's features are available as extensions to the regular Controller class instead.
	public class ShipmentController : Controller 
	{
		public ActionResult Index()
		{
			return View("index");
		}

		public ActionResult New([Bind(Prefix = "shipment")] Shipment newShipment)
		{
			return View("new", newShipment);
		}

		[Rescue("Error")]
		[RegExPreconditionFilter("id", PreconditionFilter.ParamType.RouteData, ".+", typeof(ArgumentException))]
		public ActionResult TrackSingle(string id)
		{
			return View("track", new List<string>() { id });
		}

		public ActionResult Track(string[] trackingNumbers)
		{
			List<string> validTrackingNumbers = new List<string>();
			foreach (string trackingNumber in trackingNumbers)
			{
				if (!string.IsNullOrEmpty(trackingNumber))
				{
					validTrackingNumbers.Add(trackingNumber);
				}
			}

			return View("track", validTrackingNumbers);
		}

		[Rescue("Error")]
		public ActionResult ToTheRescue()
		{
			throw new InvalidOperationException();
		}

		[Rescue("Error")]
		public ActionResult DivideByZero()
		{
			int j = 5;
			int f = 0;
			int y = j / f; //throw new DivideByZeroException(); // displays DivideByZeroException.aspx
			return null;
		}

		public XmlResult XmlAction()
		{
			Dimension[] dims = new Dimension[] {
			                   		new Dimension{Height=2,Length=1,Units=UnitOfMeasure.English},
									new Dimension{Height=6,Length=8,Units=UnitOfMeasure.Metric},
								};
			return new XmlResult(dims);
		}
	}
}
