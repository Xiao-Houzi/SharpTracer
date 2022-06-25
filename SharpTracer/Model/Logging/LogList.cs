using SharpTracer.View.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using SharpTracer.Base;

namespace SharpTracer.Base
{
	[JsonObject(MemberSerialization.OptIn)]
	public class LogList : NotificationBase, ITabPage
	{

		#region fields
		private Level _status;
		protected bool _viewopen;
		private  List<ILogItem> _items;
        #endregion

        #region Properties
        public string Title
		{ get; set; }
		public virtual string Icon
		{
			get
			{
				string ic = "⭍";
				switch (_status)
				{
					case Level.DEFAULT:
						ic = @"";
						break;
					case Level.INFO:
						ic = @"";
						break;
					case Level.WARN:
						ic = @"";
						break;
					case Level.ERROR:
						ic = @"";
						break;
				}
				return ic;
			}
			set { }
		}
		public RibbonVM Ribbon
		{
			get
			{
				return new RibbonVM();
			}

			set
			{
				throw new NotImplementedException();
			}
		}
        public string Visibility { get; set; }
        public ExpandablePanelVM Parent { get; set; }

		public object Content
		{
			get { return this; }
		}
		protected Level Status
		{
			get
			{
				return _status;
			}
			set
			{
				_status = value;
				NotifyPropertyChanged(nameof(Icon));
			}
		}

		[JsonProperty]
		public virtual List<ILogItem> Items
		{
			get { return _items; }
			set { _items = value; NotifyPropertyChanged();}
		}

		public Command CommandTabClicked
		{
			get;
			set;
		}
		#endregion
		// Construction
		public LogList()
		{
			Items = new List<ILogItem>();
			Status = 0;
			_viewopen = false;

			Title = "Basic Log";
		}

		// methods
		public void Logview(bool open)
		{
			if (_viewopen = open)
				_status = 0;
			NotifyPropertyChanged(nameof(Icon));
		}

		public virtual void AddEntry(Level logLevel, string message)
		{
            LogItem lvi = new LogItem(logLevel, message);
            Items.Add(lvi);
			NotifyPropertyChanged(nameof(Icon));
			NotifyPropertyChanged(nameof(Content));
		}

		protected void RefreshItems()
		{
			NotifyPropertyChanged(nameof(Items));
			NotifyPropertyChanged(nameof(Content));
		}

		public void Update()
		{
			NotifyPropertyChanged( nameof( Items ) );
		}

		public virtual void Clear()
		{
			Items.Clear();
			Status = 0;
		}
	}
}
