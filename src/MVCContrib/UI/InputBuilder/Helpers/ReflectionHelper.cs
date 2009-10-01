using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MvcContrib.UI.InputBuilder.Helpers
{
	public static class ReflectionHelper
	{
		public static string ToSeparatedWords(this string value)
		{
			return Regex.Replace(value, "([A-Z][a-z])", " $1").Trim();
		}

		//public static string BuildIdFrom(Expression expression)
		//{
		//    Expression expressionToCheck = expression;
		//    var tokens = new List<string>();

		//    bool done = false;
		//    bool accessedMember = false;

		//    while (!done)
		//    {
		//        switch (expressionToCheck.NodeType)
		//        {
		//            case ExpressionType.Convert:

		//                accessedMember = false;
		//                expressionToCheck = ((UnaryExpression)expressionToCheck).Operand;

		//                break;
		//            case ExpressionType.ArrayIndex:
		//                var binaryExpression = (BinaryExpression)expressionToCheck;

		//                Expression indexExpression = binaryExpression.Right;
		//                Delegate indexAction = Expression.Lambda(indexExpression).Compile();
		//                var value = (int)indexAction.DynamicInvoke();

		//                if (accessedMember)
		//                {
		//                    tokens.Add("_");
		//                }

		//                tokens.Add(string.Format("_{0}_", value));

		//                accessedMember = false;
		//                expressionToCheck = binaryExpression.Left;

		//                break;
		//            case ExpressionType.Lambda:
		//                var lambdaExpression = (LambdaExpression)expressionToCheck;
		//                accessedMember = false;
		//                expressionToCheck = lambdaExpression.Body;
		//                break;
		//            case ExpressionType.MemberAccess:
		//                var memberExpression = (MemberExpression)expressionToCheck;

		//                if (accessedMember)
		//                {
		//                    tokens.Add("_");
		//                }

		//                tokens.Add(memberExpression.Member.Name);

		//                if (memberExpression.Expression == null)
		//                {
		//                    done = true;
		//                }
		//                else
		//                {
		//                    accessedMember = true;
		//                    expressionToCheck = memberExpression.Expression;
		//                }
		//                break;
		//            default:
		//                done = true;
		//                break;
		//        }
		//    }

		//    tokens.Reverse();

		//    string result = string.Join(string.Empty, tokens.ToArray());

		//    return result;
		//}

		//public static string BuildNameFrom(Expression expression)
		//{
		//    Expression expressionToCheck = expression;
		//    var tokens = new List<string>();

		//    bool done = false;
		//    bool accessedMember = false;

		//    while (!done)
		//    {
		//        switch (expressionToCheck.NodeType)
		//        {
		//            case ExpressionType.Convert:

		//                accessedMember = false;
		//                expressionToCheck = ((UnaryExpression)expressionToCheck).Operand;

		//                break;
		//            case ExpressionType.ArrayIndex:
		//                var binaryExpression = (BinaryExpression)expressionToCheck;

		//                Expression indexExpression = binaryExpression.Right;
		//                Delegate indexAction = Expression.Lambda(indexExpression).Compile();
		//                var value = (int)indexAction.DynamicInvoke();

		//                if (accessedMember)
		//                {
		//                    tokens.Add(".");
		//                }

		//                tokens.Add(string.Format("[{0}]", value));

		//                accessedMember = false;
		//                expressionToCheck = binaryExpression.Left;

		//                break;
		//            case ExpressionType.Lambda:
		//                var lambdaExpression = (LambdaExpression)expressionToCheck;
		//                accessedMember = false;
		//                expressionToCheck = lambdaExpression.Body;
		//                break;
		//            case ExpressionType.MemberAccess:
		//                var memberExpression = (MemberExpression)expressionToCheck;

		//                if (accessedMember)
		//                {
		//                    tokens.Add(".");
		//                }

		//                tokens.Add(memberExpression.Member.Name);

		//                if (memberExpression.Expression == null)
		//                {
		//                    done = true;
		//                }
		//                else
		//                {
		//                    accessedMember = true;
		//                    expressionToCheck = memberExpression.Expression;
		//                }
		//                break;
		//            default:
		//                done = true;
		//                break;
		//        }
		//    }

		//    tokens.Reverse();

		//    string result = string.Join(string.Empty, tokens.ToArray());

		//    return result;
		//}

		public static PropertyInfo FindPropertyFromExpression(LambdaExpression lambdaExpression)
		{
			Expression expressionToCheck = lambdaExpression;

			bool done = false;

			while(!done)
			{
				switch(expressionToCheck.NodeType)
				{
					case ExpressionType.Convert:
						expressionToCheck = ((UnaryExpression)expressionToCheck).Operand;
						break;
					case ExpressionType.Lambda:
						expressionToCheck = lambdaExpression.Body;
						break;
					case ExpressionType.MemberAccess:
						var propertyInfo = ((MemberExpression)expressionToCheck).Member as PropertyInfo;
						return propertyInfo;
					default:
						done = true;
						break;
				}
			}

			return null;
		}
	}
}