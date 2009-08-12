using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MvcContrib.UI.Tags
{
	public class InputElementList<T> : ScriptableElement, IEnumerable<T> where T : Input
	{
		private readonly List<T> _elements = new List<T>();

		//Have to specify an element name, even though it won't ever render...
		public InputElementList(IDictionary attributes) : base("div", attributes)
		{
		}

		public string TextField { get; set; }

		public string ValueField { get; set; }

		public string Name
		{
			get { return NullGet("name"); }
			set { NullSet("name", value); }
		}

		public void Add(T element)
		{
			foreach(var attribute in _attributes)
			{
				if(!element.Attributes.ContainsKey(attribute.Key))
				{
					element.Attributes.Add(attribute.Key, attribute.Value);
				}
			}

			_elements.Add(element);	
		}

		public new IEnumerator<T> GetEnumerator()
		{
			return _elements.GetEnumerator();
		}

		public override string ToString()
		{
			var builder = new StringBuilder();

			foreach(var element in _elements)
			{
				builder.Append(element.ToString());
			}

			return builder.ToString();
		}

		public string ToFormattedString(string format)
		{
			var builder = new StringBuilder();

			foreach (var element in _elements)
			{
				builder.AppendFormat(format, element);
			}

			return builder.ToString();
		}
	}
}
