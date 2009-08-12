using System;
using System.Web.Mvc;
using Castle.Windsor;

namespace MvcContrib.Castle
{
	/// <summary>
	/// An IModelBinder implementation that supports resolving binders via Windsor
	/// 
	///Configuration is much the same as the WindsorControllerFactory:
	/// 
	/// container.Register(AllTypes.Of&lt;IModelBinder&gt;()
	///		.FromAssembly(typeof(MyModelBinder).Assembly)
	///		.Configure(c => c.LifeStyle.Singleton.Named(c.Implementation.Name.ToLower()))); 
	/// </summary>
	public class WindsorModelBinder : IModelBinder
	{
		private readonly IWindsorContainer _container;
		private readonly IModelBinder _defaultModelBinder;

		/// <summary>
		/// Creates a new instance of the WindsorModelBinder using the specified IWindsorContainer instance.
		/// </summary>
		/// <param name="container">The Windsor Container to use</param>
		public WindsorModelBinder(IWindsorContainer container)
			: this(container, new DefaultModelBinder())
		{
		}

		/// <summary>
		/// Creates a new instance of the WindsorModelBinder using the specified IWindsorContainer instance and the specified default binder
		/// </summary>
		/// <param name="container">The Windsor container to use</param>
		/// <param name="defaultModelBinder">The default model binder to delegate to (if the binder cannot be found in Windsor)</param>
		public WindsorModelBinder(IWindsorContainer container, IModelBinder defaultModelBinder)
		{
			_container = container;
			_defaultModelBinder = defaultModelBinder;
		}

	    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
	    {
			string componentName = bindingContext.ModelName.ToLower() + "modelbinder";

			if (_container.Kernel.HasComponent(componentName)) {
				var binderFromWindsor = _container.Resolve(componentName) as IModelBinder;

				if (binderFromWindsor == null) {
					throw new InvalidOperationException(string.Format("Expected component with key {0} to be an IModelBinder.", componentName));
				}

				return binderFromWindsor.BindModel(null, bindingContext);
			}

			// Delegate to the base binder if the type hasn't been registered in Windsor (also does String, Int32 etc if we're using DefaultModelBinder)
			return _defaultModelBinder.BindModel(null, bindingContext);
	    }
	}
}