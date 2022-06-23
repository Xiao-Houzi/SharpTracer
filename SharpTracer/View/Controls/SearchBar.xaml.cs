using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using SharpTracer.Base;

namespace SharpTracer.View.Controls
{
	/// <summary>
	/// Interaction logic for SearchBar.xaml
	/// </summary>
	public partial class SearchBar : UserControl
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

		public static readonly DependencyProperty CommandSearchProperty =
		DependencyProperty.Register( "CommandSearch", typeof(Command), typeof(SearchBar));

		public static readonly DependencyProperty SearchTextProperty =
		DependencyProperty.Register("SearchText", typeof(string), typeof(SearchBar));

		public Command CommandSearch
		{
			get => (Command)GetValue( CommandSearchProperty );
			set
			{
				SetValue( CommandSearchProperty, value);
				NotifyPropertyChanged();
			}
		}

		public string SearchText 
		{
			get => (string)GetValue(SearchTextProperty);
			set
			{
				SetValue( SearchTextProperty, value);
				NotifyPropertyChanged();
			} 
		}


		public SearchBar()
		{
			InitializeComponent();
		}
	}
}
