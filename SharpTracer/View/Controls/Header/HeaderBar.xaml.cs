using SharpTracer.Base;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;


namespace SharpTracer.View.Controls
{
	public partial class HeaderBar : UserControl, INotifyPropertyChanged
	{
		#region NotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
		{
			Application.Current?.Dispatcher?.Invoke(
			   () =>
			   {
				   PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			   });
		}
		#endregion
		private string _icon;
		private string _title;
		private string _subTitle;
		private string searchText = "";
		public static readonly DependencyProperty CommandAboutProperty =
		DependencyProperty.Register("CommandAbout", typeof(Command), typeof(SearchBar));

		public string Title
		{
			get => _title;
			set
			{
				_title = value; NotifyPropertyChanged();
			}
		}
		public string SubTitle
		{
			get
			{
				return _subTitle;
			}
			set
			{
				_subTitle = value; NotifyPropertyChanged(); InvalidateVisual();
			}
		}

		public string SearchText
		{
			get => searchText;
			set
			{
				searchText = value;
				CommandTextChanged?.Execute(this);
			}
		}
		public ProgressBar Progress { get; set; } = new ProgressBar();

		public Command CommandAbout
		{
			get => (Command)GetValue(CommandAboutProperty);
			set
			{
				SetValue(CommandAboutProperty, value);
				NotifyPropertyChanged();
			}
		}
		public Command CommandSearch
		{
			get;
			set;
		}
		public Command CommandTextChanged
		{
			get;
			set;
		}
		public string Icon { get => _icon; set => _icon = value; }

		public HeaderBar()
		{
			DataContext = this;

			Icon = "";
			Title = "Aelfcraeft App";
			SubTitle = "...";

			InitializeComponent();
		}
	}
}
