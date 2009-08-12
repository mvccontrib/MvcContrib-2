namespace MvcContrib.XsltViewEngine
{
	public enum MessageType
	{
		Info,
		InfoHtml,
		Error,
		ErrorHtml,
		Alert,
		AlertHtml
	}

	public enum Execute
	{
		Before,
		After,
		Both
	}

	public enum SecureFor
	{
		None,
		Anonymous,
		PerUser
	}
}