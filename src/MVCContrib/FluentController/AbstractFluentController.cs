using System;
using System.Linq;
using System.Web.Mvc;

namespace MvcContrib.FluentController
{
	/// <summary>
	/// <para>A fluent controller to standardise the design of controllers and ease the burden of testing.</para>
	/// 
	/// <para>If you need Dependency Injection (and you might want to for repositories) do so in your own constructor.</para>
	/// 
	/// <code>
	///     public class UserController : AbstractFluentController
	///    {
	///        public UserController()
	///        {
	///            DIContainer.ResolveDependencies(this);
	///        }
	///    ...
	/// </code>
	/// 
	/// </summary>
	/// <returns></returns>
	public abstract class AbstractFluentController : Controller
	{
		public virtual FluentControllerAction<bool> CheckValidCall()
		{
			return CheckValidCall(() => true);
		}

		/// <summary>
		/// Wrap any controller calls in this method.  This variant will use a
		/// model of type 'bool' and always return true.
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public virtual FluentControllerAction<bool> CheckValidCall(Action action)
		{
			return CheckValidCall(action, false);
		}

		public virtual FluentControllerAction<bool> CheckValidCall(Action action, bool executeActionWhenInvalid)
		{
			return CheckValidCall(() =>
			{
				action();
				return true;
			}, executeActionWhenInvalid);
		}

		public virtual FluentControllerAction<T> CheckValidCall<T>(Func<T> action)
		{
			return CheckValidCall(action, false);
		}

		/// <summary>
		/// Wrap controller calls with this method.  This will check for ModelState.IsValid
		/// before performing <paramref name="action"/> and check for ModelState.IsValid afterwards.
		/// <typeparam name="T">The type of the model returned from this method.  This will be passed into the Valid method.</typeparam>
		/// <param name="action">The action to perform.</param>
		/// <param name="executeActionWhenInvalid">If true, will execute the action even if ModelState.IsValid is false initially</param>
		/// </summary>
		/// <returns>A FluentControllerAction that can be used to setup the valid and invalid actions.</returns>
		public virtual FluentControllerAction<T> CheckValidCall<T>(Func<T> action, bool executeActionWhenInvalid)
		{
			T model = default(T);
			int numErrors = ModelState.Values.ReadValue(v => v.Select(x => x.Errors.Count).Sum());
			if(action != null)
			{
				if(executeActionWhenInvalid || ModelState.IsValid)
				{
					var obj = ExecuteCheckValidCall(() => action());
					if(obj != null)
					{
						model = (T)obj;
					}
				}
			}
			int newErrors = ModelState.Values.Select(x => x.Errors.Count).Sum() - numErrors;
			return new FluentControllerAction<T>(ModelState.IsValid, model, newErrors, this);
		}

		/// <summary>
		/// This is public for testing purposes only and shouldn't be used
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		protected virtual object ExecuteCheckValidCall(Func<object> action)
		{
			return action();
		}
	}
}