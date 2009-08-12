using System;
using System.Text;

namespace MvcContrib.UI
{
	public class ElementRenderTool
	{
		private IElement _element;
		private const string CLOSING_TAG = "</{0}>";
		private const string OPENING_TAG = "<{0} {1}{2}>";

		public ElementRenderTool(IElement el)
		{
			_element = el;
		}

		public override string ToString()
		{
			return RenderOpenTag() + RenderInnerContents() + RenderCloseTag();
		}

		protected bool ShouldCloseTag
		{
			get
			{
				if (!_element.UseFullCloseTag && !string.IsNullOrEmpty(_element.InnerText))
				{
					return true;
				}

				return _element.UseFullCloseTag;
			}
		}

		public virtual string RenderOpenTag()
		{
			if (string.IsNullOrEmpty(_element.Tag))
			{
				throw new ArgumentException("tag must contain a value");
			}

			return string.Format(OPENING_TAG,
				_element.Tag,
				RenderAttributes(_element.Attributes),
				(!ShouldCloseTag ? "/" : "")
			);
		}

		public virtual string RenderCloseTag()
		{
			if (!ShouldCloseTag)
			{
				return null;
			}

			return string.Format(CLOSING_TAG, _element.Tag);
		}

		public virtual string RenderInnerContents()
		{
			if (!ShouldCloseTag)
			{
				return null;
			}

			string innerTextformat = "{0}";

			if (_element.EscapeInnerText && (!string.IsNullOrEmpty(_element.InnerText)))
			{
				innerTextformat = "/*<![CDATA[*/\r\n{0}\r\n//]]>";
			}

			return string.Format(innerTextformat, _element.InnerText);
		}

		protected string RenderAttributes(IHtmlAttributes attribs)
		{
			if (attribs.Count > 4)
			{
				int totalLength = attribs.GetEstLength();
				var sb = new StringBuilder(totalLength + 10);
				foreach (var attrib in attribs)
				{
					//format " [attribute]="[value encoded]""
					sb.Append(" ").Append(attrib.Key).Append("=\"");
					if (attrib.Key == "id" || ( _element.Tag == "label" && attrib.Key == "for"))
						sb.Append(EncodeAttribute(attrib.Value.Replace('.', '-'))).Append("\"");
					else
						sb.Append(EncodeAttribute(attrib.Value)).Append("\"");
				}
				return sb.ToString().Trim();
			}
			else
			{
				string val = string.Empty;
				foreach (var attrib in attribs)
				{
					if (attrib.Key == "id" || (_element.Tag == "label" && attrib.Key == "for"))
						val += string.Format(" {0}=\"{1}\"", attrib.Key, EncodeAttribute(attrib.Value.Replace('.', '-')));
					else
						val += string.Format(" {0}=\"{1}\"", attrib.Key, EncodeAttribute(attrib.Value));
				}
				return val.Trim();
			}
		}

		protected string EncodeAttribute(string attributeValue)
		{
			return System.Web.HttpUtility.HtmlEncode(attributeValue);
		}
	}
}
