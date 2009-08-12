using System;

namespace MvcContrib.UI.MenuBuilder
{
	///<summary>
	/// Apply this tag to an action to set the title associated with it
	///</summary>
	public class MenuTitle : Attribute
	{
		///<summary>
		/// Specify the title text associated with this action
		///</summary>
		///<param name="title"></param>
		public MenuTitle(string title)
		{
			Title = title;
		}

		public string Title { get; set; }
	}
}