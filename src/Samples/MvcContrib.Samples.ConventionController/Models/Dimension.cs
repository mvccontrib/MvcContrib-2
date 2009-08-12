namespace MvcContrib.Samples.Models
{
	public class Dimension
	{
		public UnitOfMeasure Units { get; set; }
		public double Length { get; set; }
		public double Width { get; set; }
		public double Height { get; set; }
	}

	public enum UnitOfMeasure
	{
		English,
		Metric
	}
}