using System.Collections.Generic;

namespace MvcContrib.Samples.WindsorControllerFactory.Models
{
	public interface IService
	{
		IList<int> GetNumbers();
	}

	public class Service : IService
	{
		public IList<int> GetNumbers()
		{
			return new List<int> { 1, 2, 3 };
		}
	}
}