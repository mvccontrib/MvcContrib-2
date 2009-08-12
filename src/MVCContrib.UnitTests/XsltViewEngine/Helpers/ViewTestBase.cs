using System;
using System.IO;
using System.Reflection;
using System.Xml;
using MvcContrib.ViewFactories;
using NUnit.Framework;
using Rhino.Mocks;
using System.Web;
using System.Security.Principal;

namespace MvcContrib.UnitTests.XsltViewEngine.Helpers
{
	public abstract class ViewTestBase
	{
		protected MockRepository mockRepository;

		[SetUp]
		public virtual void SetUp()
		{
			HttpMethodToReturn = "GET";
			EcmaScriptVersionToReturn = new Version(1, 1);

			mockRepository = new MockRepository();
			HttpContext = mockRepository.DynamicHttpContextBase();
			SetupResult.For(Request.UserHostAddress).Return("::1");
			SetupResult.For(Request.UserHostName).Return("::1");
			SetupResult.For(Request.RequestType).Do(new Func<string>(() => HttpMethodToReturn)); //.Return("GET");
			SetupResult.For(Request.HttpMethod).Do(new Func<string>(() => HttpMethodToReturn));
			SetupResult.For(Request.PhysicalApplicationPath).Return("http://testing/mycontroller/test");
			SetupResult.For(Request.Url).Return(new Uri("http://testing/mycontroller/test"));
			mockRepository.Replay(Request);
			SetupResult.For(Request.Browser.EcmaScriptVersion).Do(new Func<Version>(() => EcmaScriptVersionToReturn));
			SetupResult.For(Request.Browser.Browser).Return("Firefox 2.0.11");
			SetupResult.For(HttpContext.User.Identity).Return(new MockIdentity {Name = string.Empty});
			mockRepository.ReplayAll();
		}

		protected Version EcmaScriptVersionToReturn { get; set; }

		protected string HttpMethodToReturn { get; set; }


		protected XmlDocument LoadXmlDocument(string path)
		{
			string assemblyPath = "MvcContrib.UnitTests.XsltViewEngine.Data." + path;

			var xmlDoc = new XmlDocument();

			using(Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(assemblyPath))
			{
				xmlDoc.Load(stream);
			}

			return xmlDoc;
		}

		protected HttpRequestBase Request
		{
			get { return HttpContext.Request; }
		}

		protected HttpResponseBase Response
		{
			get { return HttpContext.Response; }
		}

		protected HttpContextBase HttpContext { get; set; }

		protected HttpSessionStateBase Session
		{
			get { return HttpContext.Session; }
		}

		protected HttpServerUtilityBase Server
		{
			get { return HttpContext.Server; }
		}

		public class MockIdentity : IIdentity
		{
			#region IIdentity Members

			public string AuthenticationType { get; set; }

			public bool IsAuthenticated { get; set; }

			public string Name { get; set; }

			#endregion
		}
	}
}
