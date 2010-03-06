using System;

namespace MvcContrib
{
	public static class ObjectExtensions
	{
		/// <summary>
		/// Returns the result of <paramref name="func"/> if <paramref name="obj"/> is not null.
		/// <example>
		/// <code>
		/// Request.Url.ReadValue(x => x.Query)
		/// </code>
		/// </example>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <param name="obj">The obj.</param>
		/// <param name="func">The func.</param>
		/// <returns></returns>
		public static TResult ReadValue<T, TResult>(this T obj, Func<T, TResult> func) where T : class
		{
			return ReadValue(obj, func, default(TResult));
		}

		/// <summary>
		/// Returns the result of <paramref name="func"/> if <paramref name="obj"/> is not null.
		/// Otherwise, <paramref name="defaultValue"/> is returned.
		/// <example>
		/// <code>
		/// Request.Url.ReadValue(x => x.Query, "default")
		/// </code>
		/// </example>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="obj"></param>
		/// <param name="func"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static TResult ReadValue<T, TResult>(this T obj, Func<T, TResult> func, TResult defaultValue) where T : class
		{
			return obj != null ? func(obj) : defaultValue;
		}

		/// <summary>
		/// Executes an action if <paramref name="obj"/> is not null, otherwise does nothing
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <param name="action"></param>
		public static void ExecuteAction<T>(this T obj, Action<T> action) where T : class
		{
			if(obj != null)
			{
				action(obj);
			}
		}


		public static TResult ReadNullableValue<T, TResult>(this T? obj, Func<T, TResult> func)
			where T : struct
		{
			return ReadNullableValue(obj, func, default(TResult));
		}

		public static TResult ReadNullableValue<T, TResult>(this T? obj, Func<T, TResult> func, TResult defaultValue)
			where T : struct
		{
			return obj.HasValue ? func(obj.Value) : defaultValue;
		}
	}
}