using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;

namespace MvcContrib.UI.InputBuilder
{
	public class Input<T> where T : class
	{
		private readonly HtmlHelper<T> _htmlHelper;

		public Input(HtmlHelper<T> htmlHelper)
		{
			_htmlHelper = htmlHelper;
		}

		public IInputSpecification<PropertyViewModel> RenderInput(Expression<Func<T, object>> expression)
		{
			PropertyInfo propertyInfo = ReflectionHelper.FindPropertyFromExpression(expression);

			return new InputPropertySpecification
			{
				Model = new InputModelFactory<T>(_htmlHelper, InputBuilder.Conventions).Create(propertyInfo),
				HtmlHelper = _htmlHelper,
			};
		}

		public IInputSpecification<TypeViewModel> RenderForm(string controller, string action)
		{
			return new InputTypeSpecification<T>
			{
				HtmlHelper = _htmlHelper,
				Controller = controller,
				Action = action,
			};
		}

		public IInputSpecification<TypeViewModel> RenderForm()
		{
			return new InputTypeSpecification<T>
			{
				Model = new InputModelFactory<T>(_htmlHelper, InputBuilder.Conventions).Create(),
				HtmlHelper = _htmlHelper,
			};
		}
	}
}