using System;

namespace MvcContrib.TestHelper.Ui
{
    public interface IBrowserDriver {
        string Url { get; }
        void ScreenCaptureOnFailure(Action action);
        void ClickButton(string value);
        void ExecuteScript(string script);
        string GetValue(string name);
        void SetValue(string name, string value);
        IBrowserDriver Navigate(string url);
        void Dispose();
    }
}