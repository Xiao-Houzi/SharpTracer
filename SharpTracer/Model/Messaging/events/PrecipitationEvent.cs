using SharpTracer.Base.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTracer.Model.Events
{
	internal class SharpTracerEvent : RaiseEvent
	{
		protected SharpTracerEvent( object sender, object args ) : base(sender, args)		{		}

		public static void UI( object sender, SharpTracerUIArgs.EventReason eventReason, object dataObject )
		{
			Messenger.Send( new SharpTracerEvent( sender, new SharpTracerUIArgs( eventReason, dataObject ) ) );
		}

		public static void Model( object sender, SharpTracerModelArgs.EventReason eventReason, object dataObject )
		{
			Messenger.Send( new SharpTracerEvent( sender, new SharpTracerModelArgs( eventReason, dataObject ) ) );
		}
	}
}
