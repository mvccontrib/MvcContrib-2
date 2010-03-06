using System;
using System.Web.Mvc;

namespace MvcContrib.FluentController
{
	public class FluentControllerAction<T>
	{
		public T Model { get; private set; }
		public bool IsValid { get; private set; }
		public int NewErrors { get; private set; }
		protected ActionResult ActionResult { set; get; }
		public Controller Controller { get; private set; }

		public FluentControllerAction(bool isValid)
			: this(isValid, default(T)) {}

		public FluentControllerAction(bool isValid, T model)
			: this(isValid, model, 0, null) {}

		public FluentControllerAction(bool isValid, T model, int newErrors, Controller controller)
		{
			IsValid = isValid;
			Model = model;
			NewErrors = newErrors;
			Controller = controller;
		}

		public FluentControllerAction<T> Valid(Func<T, ActionResult> action)
		{
			if(ActionResult == null)
			{
				if(IsValid)
				{
					ActionResult = action(Model);
				}
			}
			return this;
		}

		public FluentControllerAction<T> Valid(Func<ActionResult> action)
		{
			if(ActionResult == null)
			{
				if(IsValid)
				{
					ActionResult = action();
				}
			}
			return this;
		}

		public FluentControllerAction<T> Invalid(Func<ActionResult> action)
		{
			if(ActionResult == null)
			{
				if(!IsValid)
				{
					ActionResult = action();
				}
			}
			return this;
		}

		public FluentControllerAction<T> InvalidWithNoNewErrors(Func<T, ActionResult> action)
		{
			if(ActionResult == null)
			{
				if(!IsValid && NewErrors == 0)
				{
					ActionResult = action(Model);
				}
			}
			return this;
		}

		public FluentControllerAction<T> InvalidWithNoNewErrors(Func<ActionResult> action)
		{
			if(ActionResult == null)
			{
				if(!IsValid && NewErrors == 0)
				{
					ActionResult = action();
				}
			}
			return this;
		}

		public FluentControllerAction<T> Other(Func<ActionResult> action)
		{
			if(ActionResult == null)
			{
				ActionResult = action();
			}
			return this;
		}

		public FluentControllerAction<T> Other(Func<T, ActionResult> action)
		{
			if(ActionResult == null)
			{
				ActionResult = action(Model);
			}
			return this;
		}

		/// <summary>
		/// Implicit cast so that we can return the results of this directly.
		/// </summary>
		/// <param name="fluentAction"></param>
		/// <returns></returns>
		public static implicit operator ActionResult(FluentControllerAction<T> fluentAction)
		{
			return fluentAction.ActionResult;
		}
	}
}