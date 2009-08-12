namespace MvcContrib.UnitTests.UI.Html
{
	public class Person
	{
		public string Name { get; set; }
		public int Id { get; set; }

		public Person(string name, int id)
		{
			Name = name;
			Id = id;
		}
	}
}