using MvcContrib.TestHelper.Ui;

namespace MvcContrib.TestHelper
{
	public class NavigationLinkIdentifier : NameAndDescriptionIdentifier
	{
		public NavigationLinkIdentifier(string embeddableName, string description)
			: base(embeddableName, description)
		{
		}

		public NavigationLinkIdentifier(string description)
			: base(description)
		{
		}
	}


	public class RecordIdentifier : NameAndDescriptionIdentifier
	{
		public RecordIdentifier(string embeddableName, string description)
			: base(embeddableName, description)
		{
		}

		public RecordIdentifier(string description)
			: base(description)
		{
		}
	}

	public class SectionIdentifier : NameAndDescriptionIdentifier
	{
		public SectionIdentifier(string embeddableName, string description)
			: base(embeddableName, description)
		{
		}

		public SectionIdentifier(string description)
			: base(description)
		{
		}
	}

	public class ElementIdentifier : NameAndDescriptionIdentifier
	{
		public ElementIdentifier(string embeddableName, string description)
			: base(embeddableName, description)
		{
		}
	}


	public class NameAndDescriptionIdentifier
	{
		private readonly string _embeddableName;
		private readonly string _description;
		private readonly string _reservedCharacters = "\\$&+,/:;=?@ <>#%{}|^~[]`\'\"";

		public NameAndDescriptionIdentifier(string embeddableName, string description)
		{
			_embeddableName = embeddableName;
			_description = description;
		}

		public NameAndDescriptionIdentifier(string description)
		{
			_description = description;
			var cleaned = Clean(description);
			_embeddableName = cleaned.Substring(0, 1).ToLower() + cleaned.Substring(1);
		}

		private string Clean(string description)
		{
			var cleaned = description;

			foreach (var rc in _reservedCharacters.ToCharArray())
			{
				cleaned = cleaned.Replace(rc.ToString(), "");
			}

			return cleaned;
		}

		public string EmbeddableName
		{
			get { return _embeddableName; }
		}

		public string Description
		{
			get { return _description; }
		}

		public override string ToString()
		{
			return _embeddableName;
		}

		public static implicit operator string(NameAndDescriptionIdentifier thisGuy)
		{
			return thisGuy.ToString();
		}
	}
}