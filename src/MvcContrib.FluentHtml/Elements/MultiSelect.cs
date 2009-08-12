using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Generate a select HTML element with multiselect attribute = 'true.'
	/// </summary>
	public class MultiSelect : SelectBase<MultiSelect>
	{
		/// <summary>
		/// Generate a select HTML element with multiselect attribute = 'true.'
		/// </summary>
		/// <param name="name">Value of the 'name' attribute of the element.  Also used to derive the 'id' attribute.</param>
		public MultiSelect(string name) : base(name)
		{
			builder.MergeAttribute(HtmlAttribute.Multiple, HtmlAttribute.Multiple, true);
		}

		/// <summary>
		/// Generate a select HTML element with multiselect attribute = 'true'
		/// </summary>
		/// <param name="name">Value of the 'name' attribute of the element.  Also used to derive the 'id' attribute.</param>
		/// <param name="forMember">Expression indicating the view model member assocaited with the element</param>
		/// <param name="behaviors">Behaviors to apply to the element</param>
		public MultiSelect(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(name, forMember, behaviors)
		{
			builder.MergeAttribute(HtmlAttribute.Multiple, HtmlAttribute.Multiple, true);
		}

		/// <summary>
		/// Set the selected values.
		/// </summary>
		/// <param name="selectedValues">Values matching the values of options to be selected.</param>
		public virtual MultiSelect Selected(IEnumerable selectedValues)
		{
			_selectedValues = selectedValues;
			return this;
		}
	}

    /// <summary>
    /// Generate a select HTML element with multiselect attribute = 'true.'
    /// </summary>
    public class MultiSelect<TModel> : SelectBase<MultiSelect<TModel>>
    {
        /// <summary>
        /// Generate a select HTML element with multiselect attribute = 'true.'
        /// </summary>
        /// <param name="name">Value of the 'name' attribute of the element.  Also used to derive the 'id' attribute.</param>
        public MultiSelect(string name)
            : base(name)
        {
            builder.MergeAttribute(HtmlAttribute.Multiple, HtmlAttribute.Multiple, true);
        }

        /// <summary>
        /// Generate a select HTML element with multiselect attribute = 'true'
        /// </summary>
        /// <param name="name">Value of the 'name' attribute of the element.  Also used to derive the 'id' attribute.</param>
        /// <param name="forMember">Expression indicating the view model member assocaited with the element</param>
        /// <param name="behaviors">Behaviors to apply to the element</param>
        public MultiSelect(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
            : base(name, forMember, behaviors)
        {
            builder.MergeAttribute(HtmlAttribute.Multiple, HtmlAttribute.Multiple, true);
        }

        /// <summary>
        /// Set the selected values.
        /// </summary>
        /// <param name="selectedValues">Values matching the values of options to be selected.</param>
        public virtual MultiSelect<TModel> Selected(IEnumerable selectedValues)
        {
            _selectedValues = selectedValues;
            return this;
        }
    }
}
