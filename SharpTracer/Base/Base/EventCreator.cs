using System;

namespace SharpTracer.Base
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name=""></param>
	/// <param name=""></param>
	public delegate void EventHandler(object type, object arg);

	public class GeneralEvent
	{
		/// <summary>
		/// Delegate variable backing the SomeEvent event.
		/// </summary>
		EventHandler handler;
		/// <summary>
		/// Lock for SomeEvent delegate access.
		/// </summary>
		readonly object eventLock = new object();

		/// <summary>
		/// Description for the event
		/// </summary>
		public event EventHandler SomeEvent
		{
			add
			{
				lock (eventLock)
				{
					handler += value;
				}
			}
			remove
			{
				lock (eventLock)
				{
					handler -= value;
				}
			}
		}

		/// <summary>
		/// Raises the SomeEvent event
		/// </summary>
		protected virtual void OnEvent(EventArgs e)
		{
			EventHandler h;
			lock (eventLock)
			{
				h = handler;
			}
			h?.Invoke(this, e);
		}
	}
}


