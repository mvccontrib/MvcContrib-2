using System;
using System.Collections;

namespace MvcContrib.UI.Tags
{
	[Obsolete("The element API has been deprecated. Consider using MvcContrib.FluentHtml or System.Web.Mvc.TagBuilder instead.")]
	public class RadioField : Input
	{
		private const string CHECKED = "checked";

		public RadioField(IDictionary attributes) : base("radio", attributes)
		{
		}

		public RadioField()
			: this(Hash.Empty)
		{
		}

		bool? _checkSet;
		public bool? Checked
		{
			get {  
				if (_checkSet!= null)
					return (NullGet(CHECKED) == CHECKED);
				else 
					return null;}
			set
			{
				_checkSet = value;
				if (value == true)
					NullSet(CHECKED, CHECKED);
				else
					NullSet(CHECKED, null);
			}
		}
	}
}
