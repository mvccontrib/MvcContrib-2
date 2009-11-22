using System;
using System.Web.Mvc;
using Castle.Components.Binder;
using MvcContrib.Attributes;

namespace MvcContrib.Castle
{
	/// <summary>
	/// Parameter binder that uses the Castle DataBinder to bind action parameters.
	/// Example:
	/// <![CDATA[
	/// public ActionResult Save([CastleBind] Customer customer) {
	///  //...
	/// }
	/// ]]>
	/// </summary>
	[Obsolete("Consider using System.Web.Mvc.DefaultModelBinder instead.")]
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false), Serializable]
	public class CastleBindAttribute : AbstractParameterBinderAttribute
	{
		/// <summary>
		/// Properties to exclude from binding
		/// </summary>
		public string Exclude { get; set; }

		/// <summary>
        /// Creates a new CastleBind attribute with the specified parameter prefix.
        /// The model will be bound from Request.Form.
        /// </summary>
        /// <param name="prefix">Prefix to use when extracting from the request store.</param>
        public CastleBindAttribute(string prefix) : this(prefix, RequestStore.Form)
        {
        }

        /// <summary>
        /// Creates a new CastleBind attribute. The name of the parameter will be used as the request prefix.
        /// The model will be bound from Request.Form
        /// </summary>
		public CastleBindAttribute() : this(null, RequestStore.Form)
        {
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="requestStore">The requst store that should be used to bind the model</param>
		/// <param name="prefix">Prefix to use when extract </param>
		public CastleBindAttribute(string prefix, RequestStore requestStore) : base(prefix, requestStore)
		{
		}

		/// <summary>
		/// Creates a new instance of the CastleBind attribute. The name of the parameter will be used as the request prefix.
		/// </summary>
		/// <param name="requestStore">The requst store that should be used to bind the model</param>
		public CastleBindAttribute(RequestStore requestStore) : this(null, requestStore)
		{
		}

		/// <summary>
		/// Binds the model object using a castle IDataBinder
		/// </summary>
		/// <param name="controllerContext"></param>
		/// <param name="bindingContext">The current binding context</param>
		/// <returns>A ModelBinderResult containing the bound object</returns>
	    public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
            IDataBinder binder = LocateBinder(controllerContext);
            string modelName = Prefix ?? bindingContext.ModelName;
			var tree = new TreeBuilder().BuildSourceNode(GetStore(controllerContext));

			if(tree.GetChildNode(modelName) == null)
			{
				return null;	
			}

			object instance = binder.BindObject(bindingContext.ModelType, modelName, Exclude, null, tree);
            return instance;
		}


		/// <summary>
		/// Finds the binder to use. If the controller implements ICastleBindingContainer then its binder is used. Otherwise, a new DataBinder is created.
		/// </summary>
		/// <returns></returns>
		protected virtual IDataBinder LocateBinder(ControllerContext context)
		{
			var bindingContainer = context.Controller as ICastleBindingContainer;

			if(bindingContainer != null)
			{
				if(bindingContainer.Binder == null)
				{
					bindingContainer.Binder = CreateBinder();
				}
				return bindingContainer.Binder;
			}

			return CreateBinder();
		}

        /// <summary>
        /// Creates the binder to use.
        /// </summary>
        /// <returns>IDataBinder</returns>
        protected virtual IDataBinder CreateBinder()
        {
            return new DataBinder();
        }
	}
}