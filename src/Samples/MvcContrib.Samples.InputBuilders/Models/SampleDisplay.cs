using System;
using MvcContrib.UI.InputBuilder.Attributes;

namespace Web.Models
{
	public class SampleDisplay
	{
		public string Name { get; set; }
		[Label("Some timestamp")] public DateTime TimeStamp { get; set; }
		public string Html { get; set; }
		public bool IsNeeded { get; set; }
	}
}