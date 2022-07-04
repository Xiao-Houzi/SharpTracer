using SharpTracer.Engine.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SharpTracer.View.Controls
{
    /// <summary>
    /// Interaction logic for EntityView.xaml
    /// </summary>
    public partial class EntityView : UserControl
	{
		public EntityView()
		{
			InitializeComponent();
		}

		private void Grid_Matrix(object sender, MouseButtonEventArgs e)
		{
			float[] vm = ((Entity)DataContext)?.Transform.ToArray();
			if(vm == null) return;
			string matrix = "";

			foreach(float f in vm)
				matrix += f.ToString() + ", ";

			Clipboard.SetData("Matrix", matrix);
		}

		private void Grid_Other(object sender, MouseButtonEventArgs e)
		{
			float[] vm = ((Entity)DataContext)?.Transform.ToArray();
			if(vm == null) return;
			string matrix = "";

			foreach(float f in vm)
				matrix += f.ToString() + ", ";

			Clipboard.SetData("Matrix", matrix);
		}
	}
}
