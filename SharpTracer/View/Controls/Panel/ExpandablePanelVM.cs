using System.Linq;
using System;
using System.Windows;
using System.Collections.Generic;
using SharpTracer.Base;

namespace SharpTracer.View.Controls
{
	public class ExpandablePanelVM : NotificationBase
	{
		#region fields
		private bool _visible;
		private bool _first = false;
		private List<ITabPage> _tabs;
		private ITabPage _selectedTab = null;
		#endregion

		#region properties
		public static readonly DependencyProperty aWidthProperty =
				 DependencyProperty.Register("AvailableWidth", typeof(Int32), typeof(ExpandablePanel));


		public List<ITabPage> Tabs
		{
			get
			{
				return _tabs;
			}
			protected set
			{
				_tabs = value;
				NotifyPropertyChanged();
			}
		}
		public ITabPage SelectedTab
		{
			get => _selectedTab;
			set { _selectedTab = value; NotifyPropertyChanged(); if (_selectedTab!=null)TogglePanel();
			}
		}
		public Int32 AvailableWidth
		{
			get { return (Int32)GetValue(aWidthProperty); }
			set { SetValue(aWidthProperty, value); }
		}
		public bool Expanded
		{
			get;
			set;
		}
		public int Width
		{
			get;
			private set;
		}
		public bool Open
		{
			get;set;
		}
		public Visibility Visibility
		{
			get;
			private set;
		}
		#endregion

		#region command handlers
		public void OnDockExpander_Click(object sender, EventArgs e)
		{
			if(Width == 0) Width = AvailableWidth;
			if(_visible = !_visible)
				Visibility = Visibility.Visible;
			else
			{
				SelectedTab = null;
				Visibility = Visibility.Collapsed;
			}

			SelectedTab.Icon = "*";
		}
		#endregion


		#region constructors
		public ExpandablePanelVM(int startingwidth=335)
		{
			_tabs = new List<ITabPage>();
			Visibility = Visibility.Visible;
			AvailableWidth = startingwidth;
			SelectedTab = null;
		}
		#endregion

		#region public methods
		public bool AddTab(ITabPage tab)
		{
			Tabs.Add(tab);
			if (tab == Tabs.First())
			{
				// this is the visible and selected one
				tab.Visibility = "Visible";
				SelectedTab = tab;
			}
			NotifyPropertyChanged("Tabs");
			return true;
		}
		#endregion

		#region private methods
		private void TogglePanel()
		{
			if(!_first)
			{
				_first = true;
				return;
			}

			if(Width == 0) Width = AvailableWidth;
			if(_visible = !_visible)
				Visibility = Visibility.Visible;

			Expanded = true;

			SelectedTab.Icon = "*";
			SelectedTab.Visibility = "Visible";
			NotifyPropertyChanged(nameof(Expanded));
		}
		#endregion
	}
}
