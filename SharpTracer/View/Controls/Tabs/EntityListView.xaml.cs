using System.Windows;
using System.Windows.Controls;

namespace SharpTracer.View.Controls
{
	/// <summary>
	/// Interaction logic for EntityList.xaml
	/// </summary>
	public partial class GeometryListView : UserControl
	{
		public GeometryListView()
		{
			InitializeComponent();
		}

		private void DataGrid_Selected(object sender, RoutedEventArgs e)
		{
		}

		private void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
		{
		}
	}
}
