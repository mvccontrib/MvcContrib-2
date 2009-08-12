using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Generate and HTML select element.
	/// </summary>
	public class Select : SelectBase<Select>
	{
		/// <summary>
		/// Generate an HTML select element.
		/// </summary>
		/// <param name="name">Value of the 'name' attribute of the element.  Also used to derive the 'id' attribute.</param>
		public Select(string name) : base(name) { }

		/// <summary>
		/// Generate an HTML select element.
		/// </summary>
		/// <param name="name">Value of the 'name' attribute of the element.  Also used to derive the 'id' attribute.</param>
		/// <param name="forMember">Expression indicating the model member assocaited with the element.</param>
		/// <param name="behaviors">Behaviors to apply to the element.</param>
		public Select(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(name, forMember, behaviors) { }


		/// <summary>
		/// Set the selected option.
		/// </summary>
		/// <param name="selectedValue">A value matching the option to be selected.</param>
		/// <returns></returns>
		public virtual Select Selected(object selectedValue)
		{
			_selectedValues = new List<object> { selectedValue };
			return this;
		}
	}

    /// <summary>
    /// Generate and HTML select element.
    /// </summary>
    public class Select<TModel> : SelectBase<Select<TModel>>
    {
        /// <summary>
        /// Generate an HTML select element.
        /// </summary>
        /// <param name="name">Value of the 'name' attribute of the element.  Also used to derive the 'id' attribute.</param>
        public Select(string name) : base(name) { }

        /// <summary>
        /// Generate an HTML select element.
        /// </summary>
        /// <param name="name">Value of the 'name' attribute of the element.  Also used to derive the 'id' attribute.</param>
        /// <param name="forMember">Expression indicating the model member assocaited with the element.</param>
        /// <param name="behaviors">Behaviors to apply to the element.</param>
        public Select(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
            : base(name, forMember, behaviors) { }

        /// <summary>
        /// Set the selected option.
        /// </summary>
        /// <param name="selectedValue">A value matching the option to be selected.</param>
        /// <returns></returns>
        public virtual Select<TModel> Selected(object selectedValue)
        {
            _selectedValues = new List<object> { selectedValue };
            return this;
        }
    }
}
