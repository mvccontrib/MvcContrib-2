using MvcContrib.UI.InputBuilder.Views;

namespace MvcContrib.UI.InputBuilder
{
	public static class ArrayTypeViewModelExtensions
	{
		public static bool HasDeleteButton(this TypeViewModel model)
		{
			return !(model.AdditionalValues.ContainsKey(ArrayPropertyConvention.HIDE_DELETE_BUTTON) && (bool)model.AdditionalValues[ArrayPropertyConvention.HIDE_DELETE_BUTTON]);
		
		}
		public static bool HasAddButton(this TypeViewModel model)
		{
			return !(model.AdditionalValues.ContainsKey(ArrayPropertyConvention.HIDE_ADD_BUTTON) && (bool)model.AdditionalValues[ArrayPropertyConvention.HIDE_ADD_BUTTON]);

		}
	}
}