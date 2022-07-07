using System;

namespace SharpTracer.Model.Base.Messaging
{ 
	public class LogArgs
	{
		public enum DebugLevel
        {
			Information,
			Debug,
			Warning,
			Error
        }
		public enum LogDestination
        {
			Console,
			File
        }
		public Exception Exception
		{
			get;
		}
		public DebugLevel Level
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

		public LogArgs(string message, Exception exception, DebugLevel level, LogDestination destination)
		{
			Level = level;
			Message = message;
			Exception = exception;
			Destination = destination;
		}
	}
}