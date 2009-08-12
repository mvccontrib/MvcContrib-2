using System.Web.Mvc;

namespace MvcContrib.UI.ASPXViewEngine
{
	public class AutoTypeViewPage<TViewData> : ViewPage<TViewData> where TViewData : class
	{
		protected override void SetViewData(ViewDataDictionary viewData)
		{
			base.SetViewData(AutoTypingHelper.PerformLooseTypecast<TViewData>(viewData));
		}
	}
}
