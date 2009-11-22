using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Mvc;
using System.Web;
using System.Web.Routing;

namespace MvcContrib.Attributes
{
	[Obsolete("Consider using System.Web.Mvc.DefaultModelBinder instead.")]
	[Serializable]
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Interface | AttributeTargets.Enum | AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public abstract class AbstractParameterBinderAttribute : CustomModelBinderAttribute, IModelBinder
	{
		public AbstractParameterBinderAttribute()
			: this(null)
		{
		}

		public AbstractParameterBinderAttribute(string prefix)
			: this(prefix, RequestStore.All)
		{
		}

		public AbstractParameterBinderAttribute(string prefix, RequestStore requestStore)
		{
			_prefix = prefix;
			_requestStore = requestStore;
		}

		private readonly string _prefix;
		public string Prefix
		{
			get { return _prefix; }
		}

		private readonly RequestStore _requestStore;
		public RequestStore RequestStore
		{
			get { return _requestStore; }
		}

		public override IModelBinder GetBinder()
		{
			return this;
		}

		public virtual NameValueCollection GetStore(ControllerContext context)
		{
			NameValueCollection store = null;

            switch (RequestStore)
            {
                   
                case RequestStore.QueryString:
                    store = context.RequestContext.HttpContext.Request.QueryString;
                    break;
                case RequestStore.Form:
                    store = context.RequestContext.HttpContext.Request.Form;
                    break;
                case RequestStore.Cookies:
                    store = CreateStoreFromCookies(context.RequestContext.HttpContext.Request.Cookies);
                    break;
                case RequestStore.ServerVariables:
                    store = context.RequestContext.HttpContext.Request.ServerVariables;
                    break;
                case RequestStore.Params:
                    store = context.RequestContext.HttpContext.Request.Params;
                    break;
                case RequestStore.TempData:
                    store = CreateStoreFromDictionary(context.Controller.TempData);
                    break;
                case RequestStore.RouteData:
                    store = CreateStoreFromDictionary(context.RouteData.Values);
                    break;
                case RequestStore.All:
                    store = CreateStoreFromAll(context.RequestContext.HttpContext.Request.Params, context.Controller.TempData, context.RouteData);
                    break;
            }

            return store;
		}

		public virtual NameValueCollection CreateStoreFromCookies(HttpCookieCollection cookies)
		{
			var cookieCount = cookies.Count;
			var store = new NameValueCollection(cookieCount);
			for (var i = 0; i < cookieCount; i++)
			{
				var cookie = cookies.Get(i);
				store.Add(cookie.Name, cookie.Value);
			}
			return store;
		}

		public virtual NameValueCollection CreateStoreFromDictionary(IDictionary<string, object> dict)
		{
			if (dict == null) return new NameValueCollection();

			var valueCount = dict.Count;
			var store = new NameValueCollection(valueCount);
			foreach (var kvp in dict)
			{
				var value = string.Empty;

				object oValue = kvp.Value;
				if (oValue != null) value = oValue.ToString();

				store.Add(kvp.Key, value);
			}
			return store;
		}

		public virtual NameValueCollection CreateStoreFromAll(NameValueCollection parms, TempDataDictionary tempData, RouteData routeData)
		{
			var store = new NameValueCollection(parms);
			var tempDataStore = CreateStoreFromDictionary(tempData);
			var routeDataStore = CreateStoreFromDictionary(routeData.Values);
			store.Add(tempDataStore);
			store.Add(routeDataStore);
			return store;
		}

	    public abstract object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext);
	  
	}

	public enum RequestStore
	{
		///<summary>
		/// Sets paramter values from Request.QueryString
		///</summary>
		QueryString = 1,
		///<summary>
		/// Sets parameter values from Request.Form
		///</summary>
		Form = 2,

		Cookies = 3,

		ServerVariables = 4,
		///<summary>
		/// Sets parameter values from all of the above locations in the above order
		/// If a parameter value is found in more than one location, they will be returned in comma separated form in the above order
		/// e.g. QueryString = 4, Form = 6 "4,6"
		///</summary>
		Params = 5,
		///<summary>
		/// Sets parameter values from Controller.TempData only
		///</summary>
		TempData = 6,

		RouteData = 7,
		///<summary>
		/// Sets parameter values from all fo the above locations in the above order
		/// If a parameter value is found in more than one location, they will be returned in comma separated form in the above order
		/// e.g. QueryString = 4, Form = 6, TempData = 2, RouteData = 1 "4,6,3,2"
		///</summary>
		All = 8
	}
}