using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Web.Mvc;

namespace MvcContrib.UI.MenuBuilder
{
	///<summary>
	/// Used internally to create the menu, use MvcContrib.UI.MenuBuilder.Menu to create a menu.
	///</summary>
	public class ActionMenuItem<T> : MenuItem where T : Controller
	{
		public Expression<Action<T>> MenuAction { get; set; }

		public override void Prepare(ControllerContext controllerContext)
		{
			
			if (MenuAction == null)
				throw new InvalidOperationException("MenuAction must be defined prior to using an ActionMenuItem");
			if (string.IsNullOrEmpty(HelpText))
			{
				var attributes = ((MethodCallExpression)(MenuAction.Body)).Method.GetCustomAttributes(
					typeof(MenuHelpText), false);
				if (attributes.Length > 0)
					HelpText = ((MenuHelpText)attributes[0]).HelpText;
			}
			if (string.IsNullOrEmpty(ActionUrl))
			{
				ActionUrl = LinkBuilder.BuildUrlFromExpression(controllerContext.RequestContext, RouteTable.Routes, MenuAction);
			}
			if (string.IsNullOrEmpty(Title) && string.IsNullOrEmpty(Icon))
			{
				var attributes = ((MethodCallExpression)(MenuAction.Body)).Method.GetCustomAttributes(
					typeof(MenuTitle), false);
				if (attributes.Length > 0)
				{
					Title = ((MenuTitle)attributes[0]).Title;
				}
				else
				{
					var expression = MenuAction.Body as MethodCallExpression;
					if (expression != null)
					{
						Title = SplitPascalCase(expression.Method.Name);
					}
				}
			}
			base.Prepare(controllerContext);
		}

		/// <summary>
		/// Replaces pascal casing with spaces. For example "CustomerId" would become "Customer Id".
		/// Strings that already contain spaces are ignored.
		/// </summary>
		/// <param name="input">String to split</param>
		/// <returns>The string after being split</returns>
		protected virtual string SplitPascalCase(string input)
		{
			return Regex.Replace(input, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
		}

		public ActionMenuItem<T> SetMenuAction(Expression<Action<T>> menuAction)
		{
			MenuAction = menuAction;
			return this;
		}
	}
}