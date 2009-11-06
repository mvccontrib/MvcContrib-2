using System;
using System.Linq.Expressions;
using System.Reflection;
using MvcContrib.UI.InputBuilder.Helpers;
using NUnit.Framework;

namespace MvcContrib.UnitTests.UI.InputBuilder
{
	[TestFixture]
	public class ReflectionHelperTester
	{
		[Test]
		public void The_toSeperateWords_should_split_a_Property_name()
		{
			//arrange
            
			//act
			var result = "ThisPascalCasedString".ToSeparatedWords();

			//assert
			Assert.AreEqual("This Pascal Cased String",result);
		}
		
		[Test]
		public void Should_create_a_propertyInfo_from_an_expression()
		{
			var expected = typeof(Model).GetProperty("StringProp");

			Expression<Func<Model, object>> expression = m => m.StringProp;


			//act
			var result = ReflectionHelper.FindPropertyFromExpression(expression);

			//assert
			Assert.AreEqual(expected,result);
		}


        
	}
}