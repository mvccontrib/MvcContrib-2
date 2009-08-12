using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using Commons.Collections;
using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;

namespace MvcContrib.ViewEngines
{
	public class NVelocityViewEngine : IViewEngine
	{
		private static readonly IDictionary DEFAULT_PROPERTIES = new Hashtable();
		private readonly VelocityEngine _engine;
		private readonly string _masterFolder;

		static NVelocityViewEngine()
		{
			string targetViewFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "views");
			DEFAULT_PROPERTIES.Add(RuntimeConstants.RESOURCE_LOADER, "file");
			DEFAULT_PROPERTIES.Add(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, targetViewFolder);
			DEFAULT_PROPERTIES.Add("master.folder", "masters");

			AddHtmlExtensionsFromConfig();
		}

		public NVelocityViewEngine() : this(DEFAULT_PROPERTIES)
		{
		}

		public NVelocityViewEngine(IDictionary properties)
		{
			if( properties == null ) properties = DEFAULT_PROPERTIES;

			var props = new ExtendedProperties();
			foreach(string key in properties.Keys)
			{
				props.AddProperty(key, properties[key]);
			}

			_masterFolder = props.GetString("master.folder", string.Empty);

			_engine = new VelocityEngine();
			_engine.Init(props);
		}

		private static void AddHtmlExtensionsFromConfig()
		{
			var section = ConfigurationManager.GetSection("nvelocity");
			if (section == null)
				return;

			var config = (NVelocityConfiguration)section;
            
			foreach(var t in  config.HtmlExtensionTypes)
			{
				HtmlExtensionDuck.AddExtension(t);
			}
		}

		private Template ResolveMasterTemplate(string masterName)
		{
			Template masterTemplate = null;

			if(!string.IsNullOrEmpty(masterName))
			{
				string targetMaster = Path.Combine(_masterFolder, masterName);

				if(!Path.HasExtension(targetMaster))
				{
					targetMaster += ".vm";
				}

				if(!_engine.TemplateExists(targetMaster))
				{
					throw new InvalidOperationException("Could not find view for master template named " + masterName +
					                                    ". I searched for '" + targetMaster + "' file. Maybe the file doesn't exist?");
				}

				masterTemplate = _engine.GetTemplate(targetMaster);
			}
			return masterTemplate;
		}

		private Template ResolveViewTemplate(string controllerFolder, string viewName)
		{
			string targetView = Path.Combine(controllerFolder, viewName);

			if(!Path.HasExtension(targetView))
			{
				targetView += ".vm";
			}

			if(!_engine.TemplateExists(targetView))
			{
				throw new InvalidOperationException("Could not find view " + viewName +
				                                    ". I searched for '" + targetView + "' file. Maybe the file doesn't exist?");
			}

			return _engine.GetTemplate(targetView);
		}

		public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
		{
			//TODO: Preview 5: Does this method need any custom logic?
			return FindView(controllerContext, partialViewName, null, useCache);
		}

		public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
		{
			var controllerName = (string)controllerContext.RouteData.Values["controller"];
			string controllerFolder = controllerName;

			Template viewTemplate = ResolveViewTemplate(controllerFolder, viewName);
			Template masterTemplate = ResolveMasterTemplate(masterName);

			var view = new NVelocityView(viewTemplate, masterTemplate);

			return new ViewEngineResult(view, this);
		}

		public void ReleaseView(ControllerContext controllerContext, IView view)
		{
		}
	}
}