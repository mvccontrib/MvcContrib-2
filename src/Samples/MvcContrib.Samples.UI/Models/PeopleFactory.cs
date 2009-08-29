using System;
using System.Collections.Generic;

namespace MvcContrib.Samples.UI.Models
{
	public class PeopleFactory
	{
		public IEnumerable<Person> CreatePeople()
		{
			var startDate = new DateTime(1980,1,1);
			for(int i = 0; i < 25; i++ )
			{
				yield return new Person
				{
					Id = i,
					Name = "Person " + i,
					Gender = i%2 == 0 ? "M" : "F",
					DateOfBirth = startDate.AddDays(i)
				};
			}
		}
	}
}