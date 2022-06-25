using SharpTracer.Base;
using System;

namespace SharpTracer.Base
{
    public class ErrorLogItem : NotificationBase, ILogItem
	{
		private string _message;
		private Level _level;

        public string MessageIcon
		{
			get
			{
                string fn = null;
				switch (_level)
				{
					case Level.INFO:
						fn = @"";
						break;
					case Level.WARN:
						fn = @"";
						break;
					case Level.ERROR:
						fn = @"";
						break;
					default:
						throw new Exception("Invalid LogLevel for log item");
				}
				return fn;
			}
		}
		public Level LogLevel
		{
			get
			{
				return _level;
			}
			set
			{
				_level = value;
				NotifyPropertyChanged("MessageIcon");
			}
		}
		public string Message
		{
			get
			{
				return _message;
			}
			set
			{
				_message = value;
				NotifyPropertyChanged("Message");
			}
		}

		public ErrorLogItem()
		{ }

        public ErrorLogItem(Level logLevel, string message)
        {
            LogLevel = logLevel;
            Message = message;
        }
    }
}
