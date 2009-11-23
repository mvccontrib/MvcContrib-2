using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.Web.Mvc;

namespace MvcContrib.ViewEngines
{
	public class HtmlExtensionDuck : ExtensionDuck
	{
		private static readonly List<Type> _extensionTypes = new List<Type>
		{
			typeof(NVelocityHtmlHelper),
			typeof(ButtonsAndLinkExtensions),                    
			typeof(FormExtensions), 
			typeof(ImageExtensions), 
			typeof(LinkExtensions),
			typeof(ViewExtensions),
			typeof(System.Web.Mvc.Html.LinkExtensions),    
			typeof(System.Web.Mvc.Html.InputExtensions),
			typeof(System.Web.Mvc.Html.FormExtensions),
			typeof(System.Web.Mvc.Html.SelectExtensions),
			typeof(System.Web.Mvc.Html.TextAreaExtensions),
			typeof(System.Web.Mvc.Html.ValidationExtensions),
			typeof(System.Web.Mvc.Html.RenderPartialExtensions),
			typeof(System.Web.Mvc.Html.ChildActionExtensions)
		};

		public HtmlExtensionDuck(ViewContext viewContext, IViewDataContainer container)
			: this(new HtmlHelper(viewContext, container))
		{
		}

		public HtmlExtensionDuck(HtmlHelper htmlHelper)
			: this(htmlHelper, _extensionTypes.ToArray())
		{
		}

		public HtmlExtensionDuck(HtmlHelper htmlHelper, params Type[] extentionTypes)
			: base(htmlHelper, extentionTypes)
		{
		}

		///<summary>
		/// Registers an extension type for evaluation later during duck typing interrogation.
		/// 
		/// Add your own extensions here in Application_Start for use in NVelocity views.
		///</summary>
		///<param name="type"></param>
		public static void AddExtension(Type type)
		{
			if (!_extensionTypes.Contains(type))
				_extensionTypes.Add(type);
		}
	}
}