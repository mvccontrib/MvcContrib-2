namespace MvcContrib.FluentHtml.Behaviors
{
	public interface ISupportsAutoLabeling 
	{
		/// <summary>
		/// If no label before has been explicitly set, set the label before using the element name.
		/// </summary>
		void SetAutoLabel();

		/// <summary>
		/// If no label after has been explicitly set, set the label after using the element name.
		/// </summary>
		void SetAutoLabelAfter();
	}
}