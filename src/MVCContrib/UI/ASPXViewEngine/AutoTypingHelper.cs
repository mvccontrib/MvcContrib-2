using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Web.Mvc;

namespace MvcContrib.UI.ASPXViewEngine
{
	internal static class AutoTypingHelper
	{
		public static ViewDataDictionary PerformLooseTypecast<T>(ViewDataDictionary viewData)
		{
			//There must always be some viewdata.
			if(viewData == null)
			{
				return new ViewDataDictionary();
			}

			if (viewData.Model != null && (typeof(T).IsAssignableFrom(viewData.Model.GetType())))
				// The incoming object is already of the right type
				return viewData;
			else
			{
				// Convert the incoming object to a dictionary, if it isn't one already
				IDictionary<string,Object> suppliedProps = viewData;
				if(viewData.Model != null)
				{
					suppliedProps = viewData.Model.GetType().GetProperties()
									.ToDictionary(pi => pi.Name, pi => pi.GetValue(viewData.Model, null));
				}

				// Construct a T object, taking values from suppliedProps where available
				var result = Activator.CreateInstance<T>();
				foreach (var allowedProp in typeof(T).GetProperties())
					if (suppliedProps.ContainsKey(allowedProp.Name))
						allowedProp.SetValue(result, suppliedProps[allowedProp.Name], null);

				viewData.Model = result;

				return viewData;
			}
		}
	}
}
