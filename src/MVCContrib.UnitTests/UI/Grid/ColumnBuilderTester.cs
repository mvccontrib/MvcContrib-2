using System;
using System.Collections.Generic;
using System.Linq;
using MvcContrib.UI.Grid;
using NUnit.Framework;

namespace MvcContrib.UnitTests.UI.Grid
{
	[TestFixture]
	public class ColumnBuilderTester
	{
		private ColumnBuilder<Person> _builder;

		[SetUp]
		public void Setup()
		{
			_builder = new ColumnBuilder<Person>();
		}

		[Test]
		public void Should_define_columns()
		{
			_builder.For(x => x.Name);
			_builder.For(x => x.DateOfBirth);
			_builder.Count().ShouldEqual(2);
		}


		[Test]
		public void Should_infer_column_name_from_lambda()
		{
			_builder.For(x => x.Name);
			_builder.Single().Name.ShouldEqual("Name");
		}

        [Test]
        public void Should_infer_column_displayname_from_lambda()
        {
            _builder.For(x => x.Name);
            _builder.Single().DisplayName.ShouldEqual("Name");
        }

        [Test]
        public void Shold_build_column_with_name()
        {
            _builder.For(x => x.Name).Named("foo");
            _builder.Single().Name.ShouldEqual("Name");
        }

		[Test]
		public void Should_build_column_with_displayname()
		{
			_builder.For(x => x.Name).Named("foo");
			_builder.Single().DisplayName.ShouldEqual("foo");
		}    

		[Test]
		public void Name_should_be_null_if_no_name_specified_and_not_MemberExpression()
		{
			_builder.For(x => 1);
			_builder.Single().Name.ShouldBeNull();
		}

        [Test]
        public void DisplayName_should_be_null_if_no_name_specified_and_not_MemberExpression()
        {
            _builder.For(x => 1);
            _builder.Single().DisplayName.ShouldBeNull();
        }


		[Test]
		public void DisplayName_should_be_split_pascal_case()
		{
			_builder.For(x => x.DateOfBirth);
			_builder.Single().DisplayName.ShouldEqual("Date Of Birth");
		}

        [Test]
        public void Name_should_not_be_split_pascal_case()
        {
            _builder.For(x => x.DateOfBirth);
            _builder.Single().Name.ShouldEqual("DateOfBirth");
        }

		[Test]
		public void DisplayName_should_not_be_split_if_DoNotSplit_specified()
		{
			_builder.For(x => x.DateOfBirth).DoNotSplit();
			_builder.Single().DisplayName.ShouldEqual("DateOfBirth");
		}

		[Test]
		public void DisplayName_should_not_be_split_when_explicit_name_specified()
		{
			_builder.For(x => x.Id).Named("FOO");
			_builder.Single().DisplayName.ShouldEqual("FOO");
		}

		[Test]
		public void Should_obtain_value()
		{
			_builder.For(x => x.Name);
			_builder.Single().GetValue(new Person { Name = "Jeremy" }).ShouldEqual("Jeremy");
		}

		[Test]
		public void Should_format_item()
		{
			_builder.For(x => x.Name).Format("{0}_TEST");
			_builder.Single().GetValue(new Person{Name="Jeremy"}).ShouldEqual("Jeremy_TEST");
		}

		[Test]
		public void Should_not_return_value_when_CellCondition_returns_false()
		{
			_builder.For(x => x.Name).CellCondition(x => false);
			_builder.Single().GetValue(new Person(){Name = "Jeremy"}).ShouldBeNull();
		}

		[Test]
		public void Column_should_not_be_visible()
		{
			_builder.For(x => x.Name).Visible(false);
			_builder.Single().Visible.ShouldBeFalse();
		}

		[Test]
		public void Should_html_encode_output()
		{
			_builder.For(x => x.Name);
			_builder.Single().GetValue(new Person{Name = "<script>"}).ShouldEqual("&lt;script&gt;");
		}

		[Test]
		public void Should_not_html_encode_output()
		{
			_builder.For(x => x.Name).DoNotEncode();
			_builder.Single().GetValue(new Person{Name = "<script>"}).ShouldEqual("<script>");
		}

		[Test]
		public void Should_specify_header_attributes()
		{
			var attrs = new Dictionary<string, object> { { "foo", "bar" } };
			_builder.For(x => x.Name).HeaderAttributes(attrs);
			_builder.Single().HeaderAttributes["foo"].ShouldEqual("bar");
		}

		[Test]
		public void Should_specify_header_attributes_using_lambdas()
		{
			_builder.For(x => x.Name).HeaderAttributes(foo => "bar");
			_builder.Single().HeaderAttributes["foo"].ShouldEqual("bar");
		}

		[Test]
		public void Should_create_custom_column()
		{
			_builder.For("Name");
			_builder.Single().Name.ShouldEqual("Name");
		}

        [Test]
        public void Should_create_custom_column_with_displayname()
        {
            _builder.For("Name");
            _builder.Single().DisplayName.ShouldEqual("Name");
        }

		[Test]
		public void Should_add_column()
		{
			((ICollection<GridColumn<Person>>)_builder).Add(new GridColumn<Person>(x => null, null, null));
			_builder.Count().ShouldEqual(1);
		}

		[Test]
		public void Should_clear()
		{
			_builder.For(x => x.Name);
			((ICollection<GridColumn<Person>>)_builder).Clear();
			_builder.Count().ShouldEqual(0);
		}

		[Test]
		public void Should_contain_column()
		{
			var column = _builder.For(x => x.Name) as GridColumn<Person>;
			((ICollection<GridColumn<Person>>)_builder).Contains(column).ShouldBeTrue();
		}

		[Test]
		public void Should_copy_to_array()
		{
			var array = new GridColumn<Person>[1];

			var col = _builder.For(x => x.Name);
			((ICollection<GridColumn<Person>>)_builder).CopyTo(array, 0);
			array[0].ShouldBeTheSameAs(col);
		}

		[Test]
		public void Should_remove_column()
		{
			var column = (GridColumn<Person>) _builder.For(x => x.Name);
			((ICollection<GridColumn<Person>>)_builder).Remove(column);
			_builder.Count().ShouldEqual(0);
		}

		[Test]
		public void Should_count_columns()
		{
			_builder.For(x => x.Name);
			((ICollection<GridColumn<Person>>)_builder).Count.ShouldEqual(1);
		}

		[Test]
		public void Should_not_be_readonly()
		{
			((ICollection<GridColumn<Person>>)_builder).IsReadOnly.ShouldBeFalse();
		}

		[Test]
		public void ColumnType_should_return_the_Type_of_the_property_referenced_by_the_column() 
		{
			_builder.For(x => x.DateOfBirth);
			_builder.Single().ColumnType.ShouldEqual(typeof(DateTime));
		}
	}
}