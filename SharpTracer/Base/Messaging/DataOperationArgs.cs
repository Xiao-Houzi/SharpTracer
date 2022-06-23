


using System;

namespace SharpTracer.Base.Messaging
{
	public enum OperationType
	{
		Disk,
		HTTP,
	}

	public enum Operation
	{
		Create,
		Read,
		Update,
		Delete
	}

	public class DataOperationArgs
    {
		public OperationType OperationType { get; }
		public Operation Operation { get; }
		public Uri Uri { get; }
		public string Message { get;  }
		public int ProgressID { get; }

		public DataOperationArgs( OperationType type, Operation operation, Uri uri, string message, int progressID )
		{
			OperationType = type;
			Operation = operation;
			Uri = uri;
			Message = message;
			ProgressID = progressID;
		}
    }
}