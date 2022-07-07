using System;

namespace SharpTracer.Model.Base.Messaging
{
	public enum Status
	{
		Start,
		Increment,
		Stop,
		Clear,
        Update
    }
	public class ProgressArgs
	{

		public int ProgressID { get; }
		public UInt64 EventID { get; }
		public int Max { get;  }
		public string Message { get; }
		public string Caption { get; }
		public Status Status { get; }

		public ProgressArgs(int progressID, UInt64 eventID,  string caption, string message, Status status,  int max)
		{
			ProgressID = progressID;
			EventID = eventID;
			Caption = caption;
			Message = message;
			Status = status;
			Max = max;
		}
	}
}