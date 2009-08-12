using System;
using System.IO;
using System.Web.Mvc;
using MvcContrib.UI.Html;

namespace MvcContrib.UI.MenuBuilder
{
	///<summary>
	/// Used internally to create the menu, use MvcContrib.UI.MenuBuilder.Menu to create a menu.
	///</summary>
	public class MenuItem
	{
		public string Title { get; set; }
		public string Icon { get; set; }
		public string HelpText { get; set; }
		public string ActionUrl { get; set; }

		public string AnchorClass { get; set; }
		public string IconClass { get; set; }
		public string ItemClass { get; set; }
		public string IconDirectory { get; set; }

		public bool ShowWhenDisabled { get; set; }

		public string DisabledClass { get; set; }
		public bool Disabled { get; set; }

		public string SelectedClass { get; set; }

		//internal disabled allows for resetting of the disabled property on each render, 
		//when using Secure items
		protected bool? internalDisabled;
		protected bool ItemDisabled
		{
			get { return internalDisabled ?? Disabled; }
		}

		public bool HideItem
		{
			get { return ItemDisabled && ShowWhenDisabled == false; }
		}

		protected bool Prepared { get; set; }

		public MenuItem()
		{
			Prepared = false;
		}

		protected virtual string RenderLink()
		{
			CleanTagBuilder anchor = new CleanTagBuilder("a");
			if (ItemDisabled)
				anchor.AddCssClass(DisabledClass);
			else
				anchor.Attributes["href"] = ActionUrl;
			if(IsItemSelected())
				anchor.AddCssClass(SelectedClass);
			anchor.Attributes["title"] = HelpText;
			anchor.InnerHtml += RenderIcon() + RenderTitle();
			return anchor.ToString(TagRenderMode.Normal);
		}

		protected bool itemSelected;

		public virtual bool IsItemSelected()
		{
			return itemSelected;
		}

		protected virtual string RenderIcon()
		{
			if (!string.IsNullOrEmpty(Icon))
			{
				string iconPath = (IconDirectory ?? "") + Icon;
				CleanTagBuilder icon = new CleanTagBuilder("img");
				icon.Attributes["border"] = "0";
				icon.Attributes["src"] = iconPath;
				icon.Attributes["alt"] = Title;
				icon.AddCssClass(IconClass);
				return icon.ToString(TagRenderMode.SelfClosing);
			}
			return string.Empty;
		}

		protected virtual string RenderTitle()
		{
			if (!string.IsNullOrEmpty(Title))
				return Title;
			return string.Empty;
		}

		/// <summary>
		/// Renders the menu according to the current RequestContext to the specified TextWriter
		/// </summary>
		/// <param name="requestContext">The current RequestContext</param>
		/// <param name="writer">The TextWriter for output</param>
		public virtual void RenderHtml(ControllerContext requestContext, TextWriter writer)
		{
			Prepare(requestContext);
			writer.Write(RenderHtml());
		}

		/// <summary>
		/// Used internally to render the menu. Do not call this directly without first calling Prepare, or call RenderHtml(RequestContext ...)
		/// </summary>
		public virtual string RenderHtml()
		{
			if (!Prepared)
				throw new InvalidOperationException("Must call Prepare before RenderHtml() or call RenderHtml(RequestContext, TextWriter)");
			if (HideItem)
				return string.Empty;
			CleanTagBuilder li = new CleanTagBuilder("li");
			li.AddCssClass(ItemClass);
			li.InnerHtml = RenderLink();
			return li.ToString(TagRenderMode.Normal);
		}

		/// <summary>
		/// Called internally by RenderHtml(RequestContext, TextWriter) to remove empty items from lists and generate urls.
		/// Can also be called externally to prepare the menu for serialization into Json/Xml
		/// </summary>
		/// <param name="controllerContext">The current RequestContext</param>
		/// <returns>if this item should be rendered</returns>
		public virtual void Prepare(ControllerContext controllerContext)
		{
			Prepared = true;
			if(controllerContext.RequestContext.HttpContext.Request.Path == ActionUrl)
				itemSelected = true;
		}

		public MenuItem SetTitle(string title)
		{
			Title = title;
			return this;
		}

		public MenuItem SetIcon(string icon)
		{
			Icon = icon;
			return this;
		}

		public MenuItem SetHelpText(string helpText)
		{
			HelpText = helpText;
			return this;
		}

		public MenuItem SetActionUrl(string actionUrl)
		{
			ActionUrl = actionUrl;
			return this;
		}

		public MenuItem SetAnchorClass(string anchorClass)
		{
			AnchorClass = anchorClass;
			return this;
		}

		public MenuItem SetIconClass(string iconClas)
		{
			IconClass = iconClas;
			return this;
		}

		public MenuItem SetItemClass(string itemClass)
		{
			ItemClass = itemClass;
			return this;
		}

		public MenuItem SetIconDirectory(string iconDirectory)
		{
			IconDirectory = iconDirectory;
			return this;
		}

		public MenuItem SetDisabled(bool disabled)
		{
			Disabled = disabled;
			return this;
		}

		public MenuItem SetSelectedClass(string selectedClass)
		{
			SelectedClass = selectedClass;
			return this;
		}

		/// <summary>
		/// The class to use when displaying an insecure menu item, won't be used if hiding insecure items (default behavior)
		/// </summary>
		/// <param name="itemClass"></param>
		/// <returns>this</returns>
		public MenuItem SetDisabledMenuItemClass(string itemClass)
		{
			DisabledClass = itemClass;
			return this;
		}

		/// <summary>
		/// Should the menu item be hidden or display with limited functionality when disabled
		/// </summary>
		/// <param name="hide">Set to true to hide the item, false to show it in a disabled format</param>
		/// <returns>this</returns>
		public MenuItem SetShowWhenDisabled(bool hide)
		{
			ShowWhenDisabled = hide;
			return this;
		}

		
	}
}