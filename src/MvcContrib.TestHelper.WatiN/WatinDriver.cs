using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.Expando;
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

        protected IE IE { get; set; }

        #region IBrowserDriver Members

		public virtual int GetRowCount<T>(string tableName, List<RowFilter<T>> filters)
		{
			List<TableRow> filteredRows = GetFilteredRows(tableName, filters);
			return filteredRows.Count;
		}

		private List<TableRow> GetFilteredRows<T>(string tableName, List<RowFilter<T>> filters)
		{
			Table table = IE.Table(tableName);
			var rows = table.TableRows;

			var filteredRows = new List<TableRow>();
			foreach (var filter in filters)
			{
				filteredRows.AddRange(
					rows.Where(row => row.TableCells.Any(cell => cell.Text != null ? cell.Text.Contains(filter.Value) : false)));
			}
			return filteredRows;
		}

		public virtual void ClickRowLink<T>(string tableName, List<RowFilter<T>> filters, string relId)
		{
			TableRow filteredRow = GetFilteredRows(tableName, filters)[0];
			var link = filteredRow.Link(Find.By("rel", relId));
			link.Click();
		}

		public virtual string Url
        {
            get { return IE.Url.Replace(_baseurl, ""); }
        }

		public virtual void ScreenCaptureOnFailure(Action action)
        {
            try
            {
                action();
            }
            catch(Exception)
            {
                CaptureScreenShot(GetTestname());
                throw;
            }
        }

        public virtual void ClickButton(string value)
        {
			IE.Button(Find.ById(value)).Click();
        }

		public virtual void ClickLink(string value)
		{
			IE.Link(Find.By("rel", value)).Click();
		}

		public virtual void ExecuteScript(string script)
        {
            IE.RunScript(script);
        }

		public virtual string GetValue(string name)
        {
            TextField textField = IE.TextField(Find.ByName(name));
            if(textField == null)
            {
                throw new Exception(string.Format("Could not find field '{0}' on form.", name));
            }
            return textField.Value;
        }

		public virtual void SetValue(string name, string value)
        {
            TextField textField = IE.TextField(Find.ByName(name));
            if(textField.Exists)
            {
                textField.Value = value;
                return;
            }
            SelectList select = IE.SelectList(Find.ByName(name));
            if(select.Exists)
            {
                select.Select(value);
                return;
            }
            throw new InvalidOperationException("Could not find a HTML Element by the name " + name);
        }

		public virtual IBrowserDriver Navigate(string url)
        {
            IE.GoTo(_baseurl + url);
            return this;
        }

		public virtual void Dispose()
        {
            IE.Dispose();
            IE = null;
        }

        #endregion

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

		public virtual object EvaluateScript(string script)
        {
            return JavaScriptExecutor.Eval(IE, script);
        }
    }

    public static class JavaScriptExecutor
    {
        public static object Eval(Document document, string code)
        {
            IExpando window = GetWindow(document);
            PropertyInfo property = GetOrCreateProperty(window, "__lastEvalResult");

            document.RunScript("window.__lastEvalResult = " + code + ";");

            return property.GetValue(window, null);
        }

        private static PropertyInfo GetOrCreateProperty(IExpando expando, string name)
        {
            PropertyInfo property = expando.GetProperty(name, BindingFlags.Instance);
            if(property == null)
            {
                property = expando.AddProperty(name);
            }

            return property;
        }

        private static IExpando GetWindow(Document document)
        {
            return document.HtmlDocument.parentWindow as IExpando;
        }
    }
}