using System;

namespace MvcContrib.UI.MenuBuilder
{
	///<summary>
	/// Apply this tag to an action to set a help text associated with it.
	///</summary>
	public class MenuHelpText : Attribute
	{
		///<summary>
		/// Specify the help text associated with this action
		///</summary>
		///<param name="helpText"></param>
		public MenuHelpText(string helpText)
		{
			HelpText = helpText;
		}

		public string HelpText { get; set; }
	}
}