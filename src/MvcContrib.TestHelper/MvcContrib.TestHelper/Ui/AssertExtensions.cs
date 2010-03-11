using System;
using System.Linq.Expressions;
using MvcContrib.UI.InputBuilder.Conventions;
using MvcContrib.UI.InputBuilder.Helpers;

namespace MvcContrib.TestHelper.Ui
{
	public static class AssertExtensions
	{
		public static IBrowserDriver AssertValue<TFormType>(this IBrowserDriver driver,
															Expression<Func<TFormType, object>> expression,
															string expectedValue)
		{
			string id = ReflectionHelper.BuildNameFrom(expression);
			string value = driver.GetValue(id);
			value.ShouldBe(expectedValue);
			return driver;
		}

		public static IBrowserDriver ValidationSummaryContainsMessageFor<TModelType>(this IBrowserDriver driver,
																					 Expression
																						<Func<TModelType, object>>
																						expression)
		{
			string displayname = new DefaultNameConvention().PropertyName(ReflectionHelper.FindPropertyFromExpression(expression));
			var jquerySelector = string.Format(@"$('div.{0} ul li:contains(""{1}"")').text()", "validation-summary-errors", displayname);
			var count = (string)driver.EvaluateScript(jquerySelector);
			count.AssertStringContains(displayname);
			return driver;
		}

		public static IBrowserDriver ValidationSummaryExists(this IBrowserDriver driver)
		{
			var count = (int)driver.EvaluateScript("$('div.validation-summary-errors').length");
			count.ShouldBe(1);
			return driver;
		}
		public static IBrowserDriver VerifyPage(this IBrowserDriver driver, string identifier)
		{
			var value = (string)driver.EvaluateScript("$('input[name=controller-action]').val()");
			value.ShouldBe(identifier);
			return driver;
		}

		public static IBrowserDriver VerifyPage<TController>(this IBrowserDriver driver, Expression<Func<TController, object>> pageExpression) 
		{
			string controllerName = typeof(TController).GetControllerName();
			string actionName = pageExpression.GetActionName();
			string concatenatedIdentifier = controllerName + "." + actionName;
			return VerifyPage(driver, concatenatedIdentifier);
		}

		public static string GetActionName(this LambdaExpression actionExpression)
		{
			return ((MethodCallExpression)actionExpression.Body).Method.Name;
		}

		public static string GetControllerName(this Type controllerType)
		{
			return controllerType.Name.Replace("Controller", string.Empty);
		}

		public static string ShouldBe(this string actualValue, string expectedValue)
		{
			actualValue.ToLower().ShouldEqual(expectedValue.ToLower(), 
				String.Format("Value did not match: expected '{0}' but was '{1}'.", expectedValue.ToLower(), actualValue.ToLower()));
			return actualValue;
		}
	}
}