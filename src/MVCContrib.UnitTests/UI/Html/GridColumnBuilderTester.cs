using System;
using MvcContrib.UI.LegacyGrid;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
namespace MvcContrib.UnitTests.UI.Html 
{
#pragma warning disable 618,612
	[TestFixture]
	public class GridColumnBuilderTester
	{
		private GridColumnBuilder<Person> _builder;

		[SetUp]
		public void Setup()
		{
			_builder = new GridColumnBuilder<Person>();
		}

		[Test]
		public void Should_identify_column_name_from_expression()
		{
			_builder.For(p => p.Name);
			Assert.That(_builder[0].Name, Is.EqualTo("Name"));
		}

		[Test]
		public void Should_identify_column_name_from_expression_and_split_pascal_casing()
		{
			_builder.For(p => p.DateOfBirth);
			Assert.That(_builder[0].Name, Is.EqualTo("Date Of Birth"));
		}

		[Test]
		public void Should_identify_column_name_from_expression_and_not_split_pascal_casing()
		{
			_builder.For(p => p.DateOfBirth).DoNotSplit();
			Assert.That(_builder[0].Name, Is.EqualTo("DateOfBirth"));
		}

		[Test]
		public void Should_invoke_delegate_to_retrieve_value()
		{
			_builder.For(p => p.Name);
			var person = new Person { Name = "Jeremy", Id = 1, DateOfBirth = new DateTime(1987, 4, 19) };
			Assert.That(_builder[0].ColumnDelegate(person), Is.EqualTo("Jeremy"));
		}

		[Test]
		public void Should_use_alias()
		{
			_builder.For(p => p.Name, "Person Name");
			Assert.That(_builder[0].Name, Is.EqualTo("Person Name"));
		}

		[Test]
		public void Should_use_custom_formatting()
		{
			_builder.For(p => p.DateOfBirth).Formatted("{0:ddd}");
			Assert.That(_builder[0].Format, Is.EqualTo("{0:ddd}"));
		}

		[Test]
		public void Calling_DoNotEncode_should_set_Encode_to_false()
		{
			_builder.For(p => p.Name);
			_builder.For(p => p.Name).DoNotEncode();

			Assert.That(_builder[0].Encode, Is.EqualTo(true));
			Assert.That(_builder[1].Encode, Is.EqualTo(false));
		}

		[Test]
		public void Calling_CellCondition_should_set_CellCondition()
		{
			_builder.For(p => p.Name).CellCondition(p => false);
			Assert.That(_builder[0].CellCondition, Is.Not.Null);
		}

		[Test]
		public void Calling_ColumnCondition_should_set_ColumnCondition()
		{
			_builder.For(p => p.Name).ColumnCondition(() => false);
			Assert.That(_builder[0].ColumnCondition, Is.Not.Null);
		}

		[Test]
		public void Calling_Header_should_set_custom_header_renderer()
		{
			_builder.For(p => p.Name).Header(() => { });
			Assert.That(_builder[0].CustomHeader, Is.Not.Null);
		}

		[Test]
		public void Calling_Do_should_set_custom_renderer()
		{
			_builder.For("Custom").Do(p => { });
			Assert.That(_builder[0].CustomRenderer, Is.Not.Null);
		}

		[Test]
		public void Calling_RowStart_should_set_RowStartBlock()
		{
			_builder.RowStart(p => { });
			Assert.That(_builder.RowStartBlock, Is.Not.Null);
		}

		[Test]
		public void Calling_RowEnd_should_set_RowEndBlock()
		{
			_builder.RowEnd(p => { });
			Assert.That(_builder.RowEndBlock, Is.Not.Null);			
		}

		[Test]
		public void Should_not_split_if_space_already_in_name()
		{
			_builder.For("A ColumnHeading");
			Assert.That(_builder[0].Name, Is.EqualTo("A ColumnHeading"));
		}

		[Test]
		public void For_Coverage()
		{
			_builder.GetEnumerator();
			((System.Collections.IEnumerable)_builder).GetEnumerator();
		}

		[Test]
		public void Calling_HeaderAttributes_should_set_attributes_for_column()
		{
			_builder.For("test").HeaderAttributes(new Hash(style => "width: 100px"));
			Assert.That(_builder[0].HeaderAttributes["style"], Is.EqualTo("width: 100px"));
		}

		private class Person
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public DateTime DateOfBirth { get; set; }
		}

	}
#pragma warning restore 618,612
}
