using System;
using System.Collections;
using System.Linq;

namespace MvcContrib.UI
{
	[Obsolete("The element API has been deprecated. Consider using MvcContrib.FluentHtml or System.Web.Mvc.TagBuilder instead.")]
	public class Element : IElement
    {
        public const string CLASS = "class";
        public const string ID = "id";

        private string _innerText;
        private string _tag;
		private DomQuery _selector;
        private bool _escapeInnerText;
        protected IHtmlAttributes _attributes;

        public Element()
            : this("div", Hash.Empty)
        {
        }

        public Element(string tag)
            : this(tag, Hash.Empty)
        {
        }

        public Element(string tag, IDictionary attributes)
        {
        	_escapeInnerText = false;
        	_tag = tag.ToLower();
            _attributes = new HtmlAttributes(attributes);
        }

        public virtual bool UseFullCloseTag
        {
            get { return false; }
        }

        public virtual bool EscapeInnerText
        {
            get { return _escapeInnerText; }
            set { _escapeInnerText = value; }
        }

        public virtual string Id
        {
            get { return _attributes[ID]; }
            set
            {
                _attributes[ID] = value;
                if (!string.IsNullOrEmpty(value) && (_selector == null || string.IsNullOrEmpty(_selector.ToString())))
                {
                    _selector = new DomQueryBuilder().Id(value);
                }
            }
        }

        public string Class
        {
            get { return _attributes[CLASS]; }
            set { _attributes[CLASS] = value; }
        }

        public virtual DomQuery Selector
        {
            get
            {
                if (_selector == null || string.IsNullOrEmpty(_selector.ToString()))
                {
                    return Id;
                }
                return _selector;
            }
            set
            {
                if (value != null && value.HasOnlyIds && value.Ids.Count() == 1 && Id != value.Ids.First())
                {
                    Id = value.Ids.First();
                }
                _selector = value;
            }
        }

        public virtual string InnerText
        {
            get
            {
                if (_innerText == null)
                    _innerText = string.Empty;
                return _innerText;
            }
            set { _innerText = value; }
        }

        public string Tag
        {
            get { return _tag; }
            set { _tag = value.ToLower(); }
        }

        protected string NullGet(string key)
        {
            return _attributes[key];
        }
        protected void NullSet(string key, string value)
        {
            _attributes[key] = value;
        }

        public IHtmlAttributes Attributes
        {
            get { return _attributes; }
        }

        public string this[string attributeName]
        {
            get { return _attributes[attributeName]; }
            set { _attributes[attributeName] = value; }
        }

        public virtual IEnumerator GetEnumerator()
        {
            return ((IDictionary)_attributes).GetEnumerator();
        }

        public override string ToString()
        {
			return new ElementRenderTool(this).ToString();
        }

    }
}
