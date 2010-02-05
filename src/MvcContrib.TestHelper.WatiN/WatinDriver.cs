using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.Expando;
using System.Windows.Forms;
using MvcContrib.TestHelper.Ui;
using WatiN.Core;

namespace MvcContrib.TestHelper.WatiN
{
    public class WatinDriver : IBrowserDriver
    {
        private readonly string _baseurl;

        public WatinDriver(IE ie, string baseurl)
        {
            _baseurl = baseurl;
            IE = ie;
            IE.ShowWindow(NativeMethods.WindowShowStyle.Maximize);
        }

        private IE IE { get; set; }

        public string Url
        {
            get { return IE.Url.Replace(_baseurl, ""); }
        }

        public void ScreenCaptureOnFailure(Action action)
        {
            try
            {
                action();
            }
            catch (Exception)
            {
                CaptureScreenShot(GetTestname());
                throw;
            }
        }

        public virtual void CaptureScreenShot(string testname)
        {
			
            new ScreenCapture().CaptureWindowToFile(IE.hWnd, @".\" + testname + ".jpg", ImageFormat.Jpeg);
        }

        public virtual string GetTestname()
        {
            var stack = new StackTrace();
            StackFrame testMethodFrame =
                stack.GetFrames().Where(
                    frame => frame.GetMethod().ReflectedType.Assembly != GetType().Assembly).
                    FirstOrDefault();
            return testMethodFrame.GetMethod().Name;
        }

        public virtual void ClickButton(string value)
        {
            IE.Button(Find.By("value", value)).Click();
        }

        public void ExecuteScript(string script)
        {
            IE.RunScript(script);
        }
        public object EvaluateScript(string script)
        {
            return JavaScriptExecutor.Eval(IE, script);
        }

        public string GetValue(string name)
        {
            TextField textField = IE.TextField(Find.ByName(name));
            if (textField == null)
                throw new Exception(string.Format("Could not find field '{0}' on form.", name));
            return textField.Value;
        }

        public void SetValue(string name, string value)
        {            
            var textField = IE.TextField(Find.ByName(name));
            if (textField.Exists)
            {
                textField.Value = value;
                return;
            }
            var select = IE.SelectList(Find.ByName(name));
            if(select.Exists)
            {
                select.Select(value);
                return;
            }
            throw new InvalidOperationException("Could not find a HTML Elelment by the name " + name);
        }

        public IBrowserDriver Navigate(string url)
        {
            IE.GoTo(_baseurl + url);
            return this;
        }

        public void Dispose()
        {
            IE.Dispose();
            IE = null;
        }
    }

    public static class JavaScriptExecutor
    {
        public static object Eval(Document document, string code)
        {
            IExpando window = JavaScriptExecutor.GetWindow(document);
            PropertyInfo property = JavaScriptExecutor.GetOrCreateProperty(window, "__lastEvalResult");

            document.RunScript("window.__lastEvalResult = " + code + ";");

            return property.GetValue(window, null);
        }

        private static PropertyInfo GetOrCreateProperty(IExpando expando, string name)
        {
            PropertyInfo property = expando.GetProperty(name, BindingFlags.Instance);
            if (property == null)
                property = expando.AddProperty(name);

            return property;
        }

        private static IExpando GetWindow(Document document)
        {
            return document.HtmlDocument.parentWindow as IExpando;
        }
    }
}