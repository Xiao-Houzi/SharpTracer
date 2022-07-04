using System.Windows.Controls;


namespace SharpTracer
{ 
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
public partial class GeometryControl : UserControl
	{
		public GeometryControl()
		{
			InitializeComponent();
		}

		private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			DataContext = ((ViewModels.MainWindowVM)App.Current.MainWindow.DataContext).Controls.GeometryControl;
		}
	}
}
