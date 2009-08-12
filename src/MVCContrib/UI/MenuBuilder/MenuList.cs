using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using MvcContrib.UI.Html;

namespace MvcContrib.UI.MenuBuilder
{
	///<summary>
	/// Used internally to create the menu, use MvcContrib.UI.MenuBuilder.Menu to create a menu.
	///</summary>
	public class MenuList : MenuItem, ICollection<MenuItem>
	{
		private readonly List<MenuItem> items;

		public string ListClass { get; set; }
		
		public MenuList()
		{
			items = new List<MenuItem>();
			ListClass = "";
		}

		public void Add(MenuItem item)
		{
			items.Add(item);
		}

		public void Clear()
		{
			items.Clear();
		}

		public bool Contains(MenuItem item)
		{
			return items.Contains(item);
		}

		public void CopyTo(MenuItem[] array, int arrayIndex)
		{
			items.CopyTo(array, arrayIndex);
		}

		public bool Remove(MenuItem item)
		{
			return items.Remove(item);
		}

		public int Count
		{
			get
			{
				return items.Count;
			}
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public MenuItem this[int i]
		{
			get { return items[i]; }
		}

		public IEnumerator<MenuItem> GetEnumerator()
		{
			return items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		protected virtual string RenderItems()
		{
			if (items.Count <= 0) 
				return string.Empty;
			CleanTagBuilder ul = new CleanTagBuilder("ul");
			ul.AddCssClass(ListClass);
            foreach (var menuBase in items)
				ul.InnerHtml += menuBase.RenderHtml();
			return ul.ToString(TagRenderMode.Normal);
		}
        
		public override string RenderHtml()
		{
			if (!Prepared)
				throw new InvalidOperationException("Must call Prepare before RenderHtml(TextWriter) or call RenderHtml(RequestContext, TextWriter)");
			if (HideItem)
				return string.Empty;
			if (!IsRootList && HasSingleRenderableItem())
			{
				return items[0].RenderHtml(); //if there is only one item, don't render this menu instead skip to the item
			}
			if (IsRootList)
				return RenderItems();
			CleanTagBuilder li = new CleanTagBuilder("li");
			li.AddCssClass(ItemClass);
			li.InnerHtml = RenderLink() + RenderItems();
			return li.ToString(TagRenderMode.Normal);
		}

		protected bool HasSingleRenderableItem()
		{
			int c = 0;
			for(int i = 0; i < items.Count && c < 2; i++)
			{
				var item = items[i];
				if(item.HideItem == false)
					c++;
			}
			return c == 1;
		}

		public MenuList SetListClass(string listClass)
		{
			ListClass = listClass;
			return this;
		}

		protected virtual bool IsRootList
		{
			get
			{
				return Title == null && Icon == null;
			}
		}

		public override void Prepare(ControllerContext controllerContext)
		{
			var iCopy = new List<MenuItem>(items);
			foreach (var item in iCopy)
				item.Prepare(controllerContext);
			if(items.Count <= 0)
				Disabled = true;
			Prepared = true;
		}
	}
}