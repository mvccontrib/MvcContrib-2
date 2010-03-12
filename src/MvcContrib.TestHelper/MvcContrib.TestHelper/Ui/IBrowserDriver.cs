using System;
using System.Collections.Generic;

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
    	void ClickLink(string value);
    	int GetRowCount<T>(string tableName, List<RowFilter<T>> filters);
    	void ClickRowLink<T>(string tableName, List<RowFilter<T>> filters, string relId);
    	void CaptureScreenShot(string testname);
    	object EvaluateScript(string script);
    }
}