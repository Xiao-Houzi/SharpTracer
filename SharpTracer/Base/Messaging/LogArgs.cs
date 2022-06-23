using System;

namespace SharpTracer.Base.Messaging
{
	public class LogArgs
	{
		public Exception Exception
		{
			get;
		}
		public Level Level
		{
			get;
		}
		public string Message
		{
			get;
		}
		public LogDestination Destination
		{
			get;
		}

		public LogArgs(string message, Exception exception, Level level, LogDestination destination)
		{
			Level = level;
			Message = message;
			Exception = exception;
			Destination = destination;
		}
	}
}