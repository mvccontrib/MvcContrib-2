using System;
using System.ComponentModel.DataAnnotations;
using MvcContrib.UI.InputBuilder.Attributes;

namespace Web.Models
{
	public class SampleInput
	{
		public Guid Guid { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		[Label("Number of Types")]
		[PartialView("RadioButtons")]
		public NumberOfTypeEnum EnumAsRadioButton { get; set; }

		public NumberOfTypeEnum Enum { get; set; }

		[Range(3, 15)]
		public int IntegerRangeValue { get; set; }

		[Example("mm/dd/yyyy hh:mm PM")]
		public DateTime TimeStamp { get; set; }

		[DataType(DataType.MultilineText)]
		public string Html { get; set; }

		public bool IsNeeded { get; set; }

		public ChildInput[] ChildrenForms { get; set; }
		[NoAdd]
		public ChildInput[] ChildrenFormsNoAdd { get; set; }
		[NoDelete]
		public ChildInput[] ChildrenFormsNoDelete { get; set; }
		
		[CanDeleteAll]
		public ChildInput[] ChildrenFormsCanDeleteAll { get; set; }

	}

	public class ChildInput
	{
		public string Name { get; set; }
		public bool IsActive { get; set; }
		public NumberOfTypeEnum TheEnum { get; set; }
	}
}