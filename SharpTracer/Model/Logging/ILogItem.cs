namespace SharpTracer.Base
{
    public interface ILogItem
	{
        string MessageIcon
		{
			get;
		}

        string Message
		{
			get;
			set;
		}

		Level LogLevel
		{
			get;
			set;
		}
	}
}
