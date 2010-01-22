using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcContrib.UnitTests.FluentHtml.Fakes
{
	public class FakeModel
	{
		[Required]
		public int Id { get; set; }
		
		public FakeChildModel Person { get; set; }
		
		[Range(0, 200)] 
		public string Title { get; set; }
		
		public DateTime Date { get; set; }
		
		public bool Done { get; set; }

		[Required]
		public decimal? Price { get; set; }

		public IList<int> Numbers { get; set; }

		private IList<FakeChildModel> customers = new List<FakeChildModel>();
		public IList<FakeChildModel> Customers
		{
			get { return customers; }
			set { customers = value; }
		}

		private IList<FakeModel> fakeModelList = new List<FakeModel>();
		public IList<FakeModel> FakeModelList
		{
			get { return fakeModelList; }
			set { fakeModelList = value; }
		}

		public FakeModel[] FakeModelArray { get; set; }

		public string Password { get; set; }
	}
}
