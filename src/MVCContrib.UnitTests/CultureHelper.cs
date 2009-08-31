using System;
using System.Globalization;
using System.Threading;

namespace MvcContrib.UnitTests
{
	public static class CultureHelper
	{
		public static IDisposable EnUs()
		{
			return new CultureDisposable("en-us");
		}

		private class CultureDisposable : IDisposable
		{
			private CultureInfo _originalCulture;
			private CultureInfo _originalUiCulture;

			public CultureDisposable(string culture)
			{
				_originalCulture = Thread.CurrentThread.CurrentCulture;
				_originalUiCulture = Thread.CurrentThread.CurrentUICulture;

				Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
				Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
			}


			public void Dispose()
			{
				Thread.CurrentThread.CurrentCulture = _originalCulture;
				Thread.CurrentThread.CurrentUICulture = _originalUiCulture;
			}
		}
	}
}