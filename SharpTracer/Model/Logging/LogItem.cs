using Newtonsoft.Json;
using SharpTracer.Base;
using System;

namespace SharpTracer.Base
{
	[JsonObject(MemberSerialization.OptIn)]
	public class LogItem : NotificationBase, ILogItem
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
					case Level.DEFAULT:
						fn = @"";
						break;
					default:
						throw new Exception("Invalid LogLevel for log item");
				}
				return fn;
			}
		}
		[JsonProperty]
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
		[JsonProperty]
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

		public  LogItem(Level status, string message)
		{
			_message = message;
			_level = status;
		}
	}
}
