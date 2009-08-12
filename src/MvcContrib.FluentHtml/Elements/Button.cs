using System.Collections.Generic;
using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.FluentHtml.Elements
{
    /// <summary>
    /// Generate an HTML input element of type 'button.'
    /// </summary>
    public class Button : ButtonBase<Button>
    {
        /// <summary>
        /// Generate an HTML input element of type 'button.'
        /// </summary>
        /// <param name="text">Value of the 'value' and 'name' attributes. Also used to derive the 'id' attribute.</param>
        public Button(string text) : base(text) { }

        /// <summary>
        /// Generate an HTML input element of type 'button.'
        /// </summary>
        /// <param name="text">Value of the 'value' and 'name' attributes. Also used to derive the 'id' attribute.</param>
        /// <param name="behaviors">Behaviors to apply to the element.</param>
        public Button(string text, IEnumerable<IBehaviorMarker> behaviors) : base(text, behaviors) { }
    }
}
