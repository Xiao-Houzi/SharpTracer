
using System;

namespace SharpTracer.Base.Messaging
{
	[Obsolete("Create an override.")]
	public class UIArgs
    {
		public object Reason { get; }
		public object DataObject { get; }

		[Obsolete("Create an override.")]
		public UIArgs(object reason, object dataObject)
		{
			throw new Exception("This method is only a template and should be overridden");
		}
    }

	[Obsolete("Create an override.")]
	public class ModelArgs
	{
		public class Reasons : Enumeration
		{
			public static Reasons CommandClose = new Reasons(1, "CommandClose");


			public Reasons(int id, string name)	: base(id, name)
			{
			}
		}

		public Reasons Reason { get; }
		public object DataObject { get; }

		public ModelArgs(object reason, object dataObject)
		{
			throw new Exception("This method is only a template and should be overridden");
		}
	}
}