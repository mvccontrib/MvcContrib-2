using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Web.Mvc;
using MvcContrib.UI.Grid;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.UI.Grid
{
	[TestFixture]
	public class GridModelTester
	{
		private GridModel<Person> _model;

		[SetUp]
		public void Setup()
		{
			_model = new GridModel<Person>();
		}

		[Test]
		public void Should_define_columns_using_ColumnFor()
		{
			_model.Column.For(x => x.Name);
			AsGridModel.Columns.Count.ShouldEqual(1);	
		}

		[Test]
		public void Should_define_sections()
		{
			AsGridModel.Sections.RowStart(x => "foo");
			AsGridModel.Sections.Row
				.StartSectionRenderer(
					new GridRowViewData<Person>(new Person(), false), 
					GridRendererTester.FakeRenderingContext())
				.ShouldBeTrue();
		}

		[Test]
		public void Should_define_empty_text()
		{
			_model.Empty("Foo");
			AsGridModel.EmptyText.ShouldEqual("Foo");
		}

		[Test]
		public void Should_store_attributes()
		{
			_model.Attributes(new Dictionary<string, object>() { {"foo", "bar"} });
			AsGridModel.Attributes["foo"].ShouldEqual("bar");
		}

		[Test]
		public void Should_store_attributes_with_lambdas()
		{
			_model.Attributes(foo=>"bar");
			AsGridModel.Attributes["foo"].ShouldEqual("bar");
		}

		[Test]
		public void Should_be_able_to_specify_renderer()
		{
			var renderer = MockRepository.GenerateStub<IGridRenderer<Person>>();
			_model.RenderUsing(renderer);
			AsGridModel.Renderer.ShouldBeTheSameAs(renderer);
		}

		private IGridModel<Person> AsGridModel
		{
			get { return _model; }
		}

	
	}
}