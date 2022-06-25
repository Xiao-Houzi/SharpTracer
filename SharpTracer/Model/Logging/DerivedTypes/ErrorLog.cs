using System.Windows;

namespace SharpTracer.Base
{
    public class ErrorLog : LogList
	{
		protected static ErrorLog _instance = null;

		private ErrorLog()
		{
			Title = "Log";
		}

		public static ErrorLog GetInstance()
		{
			if (_instance == null)
			{
				_instance = new ErrorLog();
			}

			return (ErrorLog)_instance;
		}

		public override void AddEntry(Level logLevel, string message)
		{
			ErrorLogItem lvi = new ErrorLogItem(logLevel, message);
			if (lvi.LogLevel > Status)
			{
				Status = lvi.LogLevel;
            base.NotifyPropertyChanged("Icon");
         }

         Application.Current.Dispatcher.Invoke(
            () =>
            {
               Items.Add( lvi);
            });
		}
   }
}
