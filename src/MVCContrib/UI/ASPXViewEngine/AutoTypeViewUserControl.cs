using System.Web.Mvc;

namespace MvcContrib.UI.ASPXViewEngine
{
	public class AutoTypeViewUserControl<TViewData> : ViewUserControl<TViewData> where TViewData : class
	{
		protected override void SetViewData(ViewDataDictionary viewData)
		{
			base.SetViewData(AutoTypingHelper.PerformLooseTypecast<TViewData>(viewData));
		}
	}
}
