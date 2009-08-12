using MvcContrib.UI.Grid;

namespace MvcContrib.Samples.UI.Models
{
	public class PeopleGridModel : GridModel<Person>
	{
		public PeopleGridModel()
		{
			Column.For(x => x.Id).Named("Person ID");
			Column.For(x => x.Name);
			Column.For(x => x.Gender);
			Column.For(x => x.DateOfBirth).Format("{0:d}");
		}
	}
}