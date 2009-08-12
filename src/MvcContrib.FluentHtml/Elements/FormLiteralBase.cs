using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base class to generate a literal element (span) accompanied by a hidden input element having the same 
	/// value.  Use this if you want to display a value and also have that same value be included in the form post.
	/// </summary>
	public abstract class FormLiteralBase<T> : TextInput<T> where T : FormLiteralBase<T>
	{
		private readonly string _name;

		protected FormLiteralBase(string name) : base(HtmlInputType.Hidden, name)
		{
			_name = name;
		}

		protected FormLiteralBase(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(HtmlInputType.Hidden, name, forMember, behaviors)
		{
			_name = name;
		}

		public override string ToString()
		{
			return base.ToString() + new Literal(_name)
			                         	.Id(string.Format("{0}_Literal", builder.Attributes[HtmlAttribute.Name].FormatAsHtmlId()))
			                         	.Value(builder.Attributes[HtmlAttribute.Value]);
		}
	}
}