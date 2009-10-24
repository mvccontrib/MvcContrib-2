namespace MvcContrib.PortableAreas
{
	public interface IEventMessage
	{
	}
	public interface ICommandMessage<TResult> : IEventMessage
	{
		TResult Result { get; }
	}

}