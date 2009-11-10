namespace MvcContrib.PortableAreas
{
	public interface IEventMessage {}

	public interface ICommandMessage<TResult> : IEventMessage where TResult : ICommandResult
	{
		TResult Result { get; }
	}

	public interface ICommandResult
	{
		bool Success { get; set; }
	}

	public interface IQueryMessage<TResult> : IEventMessage
	{
		TResult Result { get; }
	}
}