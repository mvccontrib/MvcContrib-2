using System.Linq;
using System.Web.Mvc;

namespace MvcContrib.UI
{
	///<summary>
	/// Exactly like a tag builder, except doesn't leave empty attributes in the generated html
	/// TagBuilder: <li class=""></li>
	/// CleanTabBuilder: <li></li>
	///</summary>
	public class CleanTagBuilder : TagBuilder
	{
		public CleanTagBuilder(string tagName)
			: base(tagName)
		{
		}

		private void TrimAttributes()
		{
			var list = Attributes.ToList();
			foreach (var pair in list)
			{
				if(string.IsNullOrEmpty(pair.Value))
					Attributes.Remove(pair);
				else
					Attributes[pair.Key] = pair.Value.Trim();
			}
		}

        public new string ToString(TagRenderMode renderMode)
        {
			

			TrimAttributes();
        	return base.ToString(renderMode);
        }

		public override string ToString()
		{
			TrimAttributes();
			return base.ToString();
		}
	}
}
