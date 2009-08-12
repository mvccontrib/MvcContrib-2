using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using System.Collections;
using System.Reflection;
using System.Web.Mvc.Html;

namespace MvcContrib.UI.Html
{
	public static class ModelVisualizerExtension
	{
		
		/// <summary>
		/// Snipped that allows the rendered table to be collapsed
		/// </summary>
		public static string HtmlContainer = @"
<div class=""ToHtmlTableContainer"" >
<div class=""ToHtmlTableClosedContainer""  style=""cursor: pointer"" >- Click to open model information
</div>
<div class=""ToHtmlTableOpenedContainer"">
<div class=""ToHtmlTableCloseText"" style=""cursor: pointer"" >
+ Click to close model information
</div>
<table border=1  >{0}</table></div></div>
";
		public static string OpenCloseScript = @"
<script language=""javascript"" >
      $(document).ready(function() {
          setToHtmlControlHandlers();
          defaultToHtmlUI();
      });

      function openToHtmlTable() {
        $(this).parent().children("".ToHtmlTableOpenedContainer"").show();
        $(this).hide();
      }

      function closeToHtmlTable() {
        $(this).parent().hide();
        $(this).parent().parent().children("".ToHtmlTableClosedContainer"").show();
      }

      //Events der Buttons registrieren
      function setToHtmlControlHandlers() {
          $("".ToHtmlTableClosedContainer"").bind('click', openToHtmlTable );
          $("".ToHtmlTableCloseText"").bind('click', closeToHtmlTable);
      }

      function defaultToHtmlUI() {
         $("".ToHtmlTableOpenedContainer"").hide();
         $("".ToHtmlTableClosedContainer"").show();
      }

</script> ";

		/// <summary>
		/// Renders Model as table
		/// </summary>
		/// <param name="htmlHelper"></param>
		/// <returns></returns>
		public static string ModelVisualizer(this HtmlHelper htmlHelper)
		{
			if (htmlHelper.ViewContext.ViewData.Model != null)
			{
				//render Model of strongly typed View
				return ModelToHtmlTable(htmlHelper.ViewContext.ViewData.Model);
			}
			else
			{
				//render untyped ViewData collection
				return ModelToHtmlTable(htmlHelper.ViewContext.ViewData);
			}
		}

		/// <summary>
		/// render items of SelectList (ie for DropDown)
		/// </summary>
		/// <param name="selectListToRender"></param>
		/// <returns></returns>
		private static string RenderSelectListTable(System.Collections.Generic.IEnumerable<SelectListItem> selectListToRender)
		{
			var result = new StringBuilder();

			result.AppendFormat("<tr>");
			result.AppendFormat(@"<td colspan=""3"">SelectList</td>");
			result.AppendFormat("</tr>");

			result.AppendFormat("<tr>");
			result.AppendFormat("<td>Name</td>");
			result.AppendFormat("<td>Value</td>");
			result.AppendFormat("<td>Selected</td>");
			result.AppendFormat("</tr>");

			foreach (SelectListItem selectItem in selectListToRender)
			{
				result.AppendFormat("<tr>");
				result.AppendFormat("<td>{0}</td>", HttpUtility.HtmlEncode(selectItem.Text));
				result.AppendFormat("<td>{0}</td>", HttpUtility.HtmlEncode(selectItem.Value));
				result.AppendFormat("<td>{0}</td>", selectItem.Selected ? "selected" : "");
				result.AppendFormat("</tr>");
			}

			string retvalue = result.ToString();
			retvalue = String.Format(HtmlContainer, retvalue);
			return retvalue;
		}

		/// <summary>
		/// Render properties of an object as table
		/// </summary>
		/// <param name="objectToRender"></param>
		/// <returns></returns>
		private static string RenderObjectTable(object objectToRender)
		{
			string result = @"<table border=""1"" >";
			Type t = objectToRender.GetType();
			result += @"<tr><td colspan=""2"" >" + t.ToString() + "</td></tr>";
			Dictionary<string, object> dataDic = new Dictionary<string, object>();

			foreach (System.Reflection.PropertyInfo pi in t.GetProperties())
			{
				dataDic.Add(pi.Name, pi.GetValue(objectToRender, null));
			}
			string rows = RenderDictionaryTable(dataDic);
			result += rows;
			result += "</table>";
			return result;
		}

		/// <summary>
		/// Render list of objects
		/// </summary>
		/// <param name="listToRender"></param>
		/// <returns></returns>
		private static string RenderGenericList(IEnumerable listToRender)
		{
			var result = new StringBuilder();
			var propertyArray = new System.Reflection.PropertyInfo[0];
			bool isFirstRow = true;

			foreach (object listItem in listToRender)
			{
				if (isFirstRow)
				{
					propertyArray = listItem.GetType().GetProperties();
					result.Append("<tr>");
					foreach (PropertyInfo colObject in propertyArray)
					{
						result.AppendFormat("<td><b>{0}</b></td>", colObject.Name);
					}
					result.Append("</tr>");
					isFirstRow = false;
				}

				result.Append("<tr>");
				foreach (PropertyInfo colObject in propertyArray)
				{
					result.AppendFormat("<td>{0}</td>", colObject.GetValue(listItem, null));
				}
				result.Append("</tr>");
			}
			string htmlRows = result.ToString();

			string completeTable = string.Format(HtmlContainer, htmlRows);
			return completeTable;
		}

		/// <summary>
		/// Framework types are rendered with "ToString"
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		static bool IsMicrosoftType(Type type)
		{
			object[] attrs = type.Assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
			return attrs.OfType<AssemblyCompanyAttribute>().Any(attr => attr.Company == "Microsoft Corporation");
		}


		/// <summary>
		/// Enables Rendering of typed Model and untyped ViewData
		/// </summary>
		/// <param name="viewData"></param>
		/// <returns></returns>
		static string RenderDictionaryTable(IDictionary<string, object> viewData)
		{
			var htmlRows = new StringBuilder();

			foreach (string dictionaryKey in viewData.Keys.OrderBy(val => val).ToList())
			{
				object value = viewData[dictionaryKey];
				htmlRows.AppendFormat("<tr>");
				htmlRows.AppendFormat("<td>{0}</td>", HttpUtility.HtmlEncode(dictionaryKey));

				if (value == null)
				{
					htmlRows.AppendFormat("<td>null</td>");
				}
				else if (value is Enum)
				{
					htmlRows.AppendFormat("<td>{0}</td>", value ?? String.Empty);
				}
				else if (!IsMicrosoftType(value.GetType()))
				{
					htmlRows.AppendFormat("<td>{0}</td>", RenderObjectTable(value));
				}
				else if (value is IEnumerable<System.Web.Mvc.SelectListItem>)
				{
					htmlRows.AppendFormat("<td>{0}</td>", RenderSelectListTable((IEnumerable<System.Web.Mvc.SelectListItem>)value));
				}
				else if (value.GetType().ToString().StartsWith("System.Collections.Generic.List"))
				{
					htmlRows.AppendFormat("<td>{0}</td>", RenderGenericList((IEnumerable)value));
				}
				else
				{
					htmlRows.AppendFormat("<td>{0}</td>", value ?? String.Empty);
				}

				htmlRows.AppendFormat("</tr>");
			}
			return htmlRows.ToString();
		}

		/// <summary>
		/// render object as table
		/// </summary>
		/// <param name="objectToRender"></param>
		/// <returns></returns>
		private static string ModelToHtmlTable(object objectToRender)
		{
			var result = new StringBuilder();
			result.Append(OpenCloseScript);
			IDictionary<string, object> dataDic = null;

			if (objectToRender is ViewDataDictionary)
			{
				//Handle untyped ViewData
				dataDic = (IDictionary<string, object>)objectToRender;
			}
			else
			{
				//Handle Model of strongly typed View
				dataDic = new Dictionary<string, object>();
				var propertyArray = objectToRender.GetType().GetProperties();
				foreach (var prop in propertyArray)
				{
					dataDic.Add(prop.Name, prop.GetValue(objectToRender, null));
				}
			}

			string rows = "";
			if (dataDic.Count > 0)
			{
				rows = RenderDictionaryTable(dataDic);
			}
			else
			{
				rows = "<tr><td>There is no data in ViewData</td></tr>";
			}
			string tableAndRows = string.Format(HtmlContainer, rows);
			result.Append(tableAndRows);

			string retvalue = result.ToString();
			return retvalue;
		}
	}
}
