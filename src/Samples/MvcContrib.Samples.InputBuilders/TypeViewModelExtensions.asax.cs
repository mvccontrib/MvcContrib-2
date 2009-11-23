using MvcContrib.UI.InputBuilder.Views;

namespace MvcContrib.UI.InputBuilder
{
	public static class TypeViewModelExtensions
	{
		public static bool HasDeleteButton(this TypeViewModel model)
		{
			return !(model.AdditionalValues.ContainsKey(ArrayConvention.HIDE_DELETE_BUTTON) && (bool)model.AdditionalValues[ArrayConvention.HIDE_DELETE_BUTTON]);
		
		}
		public static bool HasAddButton(this TypeViewModel model)
		{
			return !(model.AdditionalValues.ContainsKey(ArrayConvention.HIDE_ADD_BUTTON) && (bool)model.AdditionalValues[ArrayConvention.HIDE_ADD_BUTTON]);

		}
	}
}