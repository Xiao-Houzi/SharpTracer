namespace SharpTracer.Base
{
	public class VMBase : NotificationBase
	{
		/// <summary>
		/// Sets wether this window can be closed
		/// </summary>
		public bool Closeable { get; set; } = true;

		private Command CommandClose { get; set; }

		public VMBase()
		{
			CommandClose = new Command(CanClose, Command_Close);
		}
		/// <summary>
		/// Executes an action, when user closes a window, displaying this instance, using system menu.
		/// </summary>
		protected virtual void Command_Close(object sender)
		{
		}

		/// <summary>
		/// Detects whether user can close a window
		/// </summary>
		/// <returns>
		/// <see langword="true"/>, if window can be closed;
		/// otherwise <see langword="false"/>.
		/// </returns>
		protected virtual bool CanClose()
		{
			return Closeable;
		}
	}
}
