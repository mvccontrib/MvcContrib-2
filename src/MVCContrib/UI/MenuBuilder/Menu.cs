using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace MvcContrib.UI.MenuBuilder
{
	///<summary>
	/// Use this class to build fluent menus, with Menu.Begin
	/// See the MvcContrib.Samples.UI for details
	///</summary>
	public static class Menu
	{
		///<summary>
		/// The class to use on every anchor tag
		///</summary>
		public static string DefaultAnchorClass { get; set; }

		/// <summary>
		/// The class to use on every icon's image tag
		/// </summary>
		public static string DefaultIconClass { get; set; }

		/// <summary>
		/// The class to use on every LI tag
		/// </summary>
		public static string DefaultItemClass { get; set; }

		/// <summary>
		/// The class to use on every UL tag
		/// </summary>
		public static string DefaultListClass { get; set; }

        /// <summary>
		/// The directory that icon images are located in
		/// </summary>
		public static string DefaultIconDirectory { get; set; }

        /// <summary>
		/// Should items that the user is not authorized to see be hidden or disabled? True for hidden.
		/// </summary>
		public static bool DefaultShowWhenDisabled { get; set; }

		/// <summary>
		/// The default css class to use for disabled items, if they are shown instead of hidden
		/// </summary>
		public static string DefaultDisabledClass { get; set; }

		/// <summary>
		/// The default css class to use for selected items, based on the requested url and action
		/// </summary>
		public static string DefaultSelectedClass { get; set; }
		
		/// <summary>
		/// Use this at the top of a fluent menu, with items in the params
		/// </summary>
		/// <param name="items">All of the menu items in this menu</param>
		/// <returns>The menu that was just created</returns>
		public static MenuList Begin(params MenuItem[] items)
		{
			return Items(null, null, items);
		}

		/// <summary>
		/// Creates a submenu of items
		/// </summary>
		/// <param name="title">The title for this list</param>
		/// <param name="items">The items in this menu</param>
		/// <returns>The list of items</returns>
		public static MenuList Items(string title, params MenuItem[] items)
		{
			return Items(title, null, items);
		}


		/// <summary>
		/// Creates a submenu of items
		/// </summary>
		/// <param name="title">The title for this list</param>
		/// <param name="items">The items in this menu</param>
		/// <param name="icon">The location of the icon to display</param>
		/// <returns>The list of items</returns>
		public static MenuList Items(string title, string icon, params MenuItem[] items)
		{
			var list = new MenuList { Title = title, Icon = icon };
			foreach (var item in items)
			{
				list.Add(item);
			}
			return (MenuList)AddDefaults(list);
		}

		
		/// <summary>
		/// Adds an item in secure fashion, confirming the user has permissions to access this item
		/// The permissions are determined via the AuthorizeAttribute. A title will be build out of the action name.
		/// </summary>
		/// <typeparam name="T">The controller that this action is in</typeparam>
		/// <param name="menuAction">The action that this menu should invoke</param>
		/// <returns>The menu item to be added to the list</returns>
		public static MenuItem Secure<T>(Expression<Action<T>> menuAction) where T : Controller
		{
			return Secure(menuAction, null, null);
		}

		/// <summary>
		/// Adds an item in secure fashion, confirming the user has permissions to access this item
		/// The permissions are determined via the AuthorizeAttribute
		/// </summary>
		/// <typeparam name="T">The controller that this action is in</typeparam>
		/// <param name="menuAction">The action that this menu should invoke</param>
		/// <param name="title">The title for this menu item</param>
		/// <returns>The menu item to be added to the list</returns>
		public static MenuItem Secure<T>(Expression<Action<T>> menuAction, string title) where T : Controller
		{
			return Secure(menuAction, title, null);
		}


		/// <summary>
		/// Adds an item in secure fashion, confirming the user has permissions to access this item
		/// The permissions are determined via the AuthorizeAttribute
		/// </summary>
		/// <typeparam name="T">The controller that this action is in</typeparam>
		/// <param name="menuAction">The action that this menu should invoke</param>
		/// <param name="title">The title for this menu item</param>
		/// <param name="icon">The location of an icon file</param> 
		/// <returns>The menu item to be added to the list</returns>
		public static MenuItem Secure<T>(Expression<Action<T>> menuAction, string title, string icon) where T : Controller
		{
			return AddDefaults(new SecureActionMenuItem<T> { MenuAction = menuAction, Title = title, Icon = icon });
		}

		/// <summary>
		/// Adds an item. A title will be build out of the action name.
		/// </summary>
		/// <typeparam name="T">The controller that this action is in</typeparam>
		/// <param name="menuAction">The action that this menu should invoke</param>
		/// <returns>The menu item to be added to the list</returns>
		public static MenuItem Action<T>(Expression<Action<T>> menuAction) where T : Controller
		{
			return Action(menuAction, null, null);
		}

		/// <summary>
		/// Adds an item. A title will be build out of the action name.
		/// </summary>
		/// <typeparam name="T">The controller that this action is in</typeparam>
		/// <param name="menuAction">The action that this menu should invoke</param>
		/// <param name="title">The title for this menu item</param>
		/// <returns>The menu item to be added to the list</returns>
		public static MenuItem Action<T>(Expression<Action<T>> menuAction, string title) where T : Controller
		{
			return Action(menuAction, title, null);
		}

		/// <summary>
		/// Adds an item. A title will be build out of the action name.
		/// </summary>
		/// <typeparam name="T">The controller that this action is in</typeparam>
		/// <param name="menuAction">The action that this menu should invoke</param>
		/// <param name="title">The title for this menu item</param>
		/// <param name="icon">The location of an icon file</param> 
		/// <returns>The menu item to be added to the list</returns>
		public static MenuItem Action<T>(Expression<Action<T>> menuAction, string title, string icon) where T : Controller
		{
			return AddDefaults(new ActionMenuItem<T> { MenuAction = menuAction, Title = title, Icon = icon });
		}

		/// <summary>
		/// Adds an item that is a direct link instead of being built off of an action. Used links outside your site.
		/// </summary>
		/// <param name="title">The title for this menu item</param>
		/// <param name="url">The location to visit</param>
		/// <returns>The menu item to be added to the list</returns>
		public static MenuItem Link(string url, string title)
		{
			return Link(url, title, null);
		}

		/// <summary>
		/// Adds an item that is a direct link instead of being built off of an action. Used links outside your site.
		/// </summary>
		/// <param name="title">The title for this menu item</param>
		/// <param name="url">The location to visit</param>
		/// <param name="icon">The location of the icon file</param>
		/// <returns>The menu item to be added to the list</returns>
		public static MenuItem Link(string url, string title, string icon)
		{
			return AddDefaults(new MenuItem { Title = title, ActionUrl = url, Icon = icon});
		}

		private static MenuItem AddDefaults(MenuItem item)
		{
			item.AnchorClass = DefaultAnchorClass;
			item.IconClass = DefaultIconClass;
			item.ItemClass = DefaultItemClass;
			item.IconDirectory = DefaultIconDirectory;
			item.DisabledClass = DefaultDisabledClass;
			item.ShowWhenDisabled = DefaultShowWhenDisabled;
			item.SelectedClass = DefaultSelectedClass;
			if (item is MenuList)
				((MenuList)item).ListClass = DefaultListClass;
			return item;
		}
	}

	public static class MenuBuilderHtmlExtensions
	{
		/// <summary>
		/// Render the menu to the current ViewContext
		/// </summary>
		/// <param name="helper"></param>
		/// <param name="menu">The menu to render</param>
		public static void Menu(this HtmlHelper helper, MenuItem menu)
		{
			menu.RenderHtml(helper.ViewContext, helper.ViewContext.HttpContext.Response.Output);
		}
	}
}