using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using MvcContrib.IncludeHandling;
using MvcContrib.IncludeHandling.Configuration;

namespace MvcContrib.UnitTests.IncludeHandling
{
	public class QnDServiceLocator : ServiceLocatorImplBase
	{
		private static IDictionary<Type, object> _types;

		public QnDServiceLocator(IDictionary<Type, object> types)
		{
			_types = types;
		}

		protected override object DoGetInstance(Type serviceType, string key)
		{
			return _types[serviceType];
		}

		protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
		{
			throw new NotImplementedException();
		}

		public static QnDServiceLocator Create(IHttpContextProvider http, IIncludeHandlingSettings settings, Controller[] controllers)
		{
			var types = new Dictionary<Type, object>
			{
				{ typeof (IHttpContextProvider), http },
				{ typeof (IKeyGenerator), new KeyGenerator() },
				{ typeof (IIncludeHandlingSettings), settings }
			};
			types.Add(typeof (IIncludeReader), new FileSystemIncludeReader((IHttpContextProvider) types[typeof (IHttpContextProvider)]));

			var keyGen = (IKeyGenerator) types[typeof (IKeyGenerator)];

			types.Add(typeof (IIncludeStorage), new StaticIncludeStorage(keyGen));

			var includeReader = (IIncludeReader) types[typeof (IIncludeReader)];
			var storage = (IIncludeStorage) types[typeof (IIncludeStorage)];
			var combiner = new IncludeCombiner(settings, includeReader, storage, (IHttpContextProvider)types[typeof(IHttpContextProvider)]);
			types.Add(typeof (IIncludeCombiner), combiner);

			types.Add(typeof (IncludeController), new IncludeController(settings, combiner));
			foreach (var controller in controllers)
			{
				types.Add(controller.GetType(), controller);
			}
			return new QnDServiceLocator(types);
		}
	}
}