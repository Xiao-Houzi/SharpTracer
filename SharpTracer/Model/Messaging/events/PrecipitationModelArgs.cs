namespace SharpTracer.Model.Events
{
	public class SharpTracerModelArgs
	{
		public enum EventReason
		{
			SafeToClose,
			GLIsNull,
			GAcquired,
			AcquireGL,
			EntitysUpdated,
			EntitySelected,
			ProjectLoaded
		};
		public EventReason Reason
		{
			get;
		}
		public object DataObject
		{
			get;
		}

		public SharpTracerModelArgs( EventReason reason, object dataObject )
		{
			Reason = reason;
			DataObject = dataObject;
		}
	}
}