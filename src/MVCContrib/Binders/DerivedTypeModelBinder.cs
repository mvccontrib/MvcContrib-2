using System;
using System.Web.Mvc;

namespace MvcContrib.Binders
{
	/// <summary>
	/// This model binder extends the default model binder to detect alternate runtime types
	/// on a page and allow the binder adapt to derived types.
	/// </summary>
	public class DerivedTypeModelBinder : DefaultModelBinder
	{
		/// <summary>
		/// An override of CreateModel that focuses on detecting alternate types at runtime
		/// </summary>
		/// <param name="controllerContext">the controller context</param>
		/// <param name="bindingContext">the binding context</param>
		/// <param name="modelType">the target type to be instantiated by this method and rehydrated by 
		/// the default model binder</param>
		/// <returns>instance of the target model type</returns>
		protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
		{
			var instantiationType = DetectInstantiationType(controllerContext, bindingContext, modelType);

			if (instantiationType == modelType)
				return base.CreateModel(controllerContext, bindingContext, modelType);

			bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, instantiationType);
			// set up the binding context to acknowledge our derived concrete instance
			//bindingContext.ModelMetadata = new CustomModelMetadata(instantiationType, bindingContext.ModelMetadata);

			return Activator.CreateInstance(instantiationType);
		}

		protected Type DetectInstantiationType(ControllerContext controllerContext, ModelBindingContext bindingContext, Type typeToCreate)
		{
			var typeValue = DetectTypeStamp(bindingContext);

			if (String.IsNullOrEmpty(typeValue))
				return typeToCreate;

			foreach (var derivedType in DerivedTypeModelBinderCache.GetDerivedTypes(typeToCreate))
				if (typeValue == derivedType.FullName)
					return derivedType;

			throw new InvalidOperationException(string.Format("unable to located identified type '{0}' as a variant of '{1}'", typeValue, typeToCreate.FullName));
		}

		/// <summary>
		/// This constant need to be moved to an area that is reachable by both MvcContrib and the FluentHtml projects
		/// </summary>
		public const string TypeStampKey = "_xTypeStampx_";

		private static string DetectTypeStamp(ModelBindingContext bindingContext)
		{
			var propertyName = CreateSubPropertyName(bindingContext.ModelName, TypeStampKey);

			if (bindingContext.ValueProvider.ContainsPrefix(propertyName))
			{
				var value = bindingContext.ValueProvider.GetValue(propertyName);
				if (value.RawValue is String[])
					return (value.RawValue as String[])[0];

				throw new InvalidOperationException(
					string.Format("TypeStamp found for type {0} on path {1}, but format is invalid.",
					              bindingContext.ModelType.Name, propertyName));
			}

			return string.Empty;
		}
	}

}


