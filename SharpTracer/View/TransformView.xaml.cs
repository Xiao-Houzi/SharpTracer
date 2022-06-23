using SharpTracer.ViewModels;
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

namespace SharpTracer
{
	/// <summary>
	/// Interaction logic for MatrixCalculator.xaml
	/// </summary>
	public partial class TransforrmView : Window
	{
		public TransforrmView()
		{
			this.Topmost = true;
			InitializeComponent();
		}

		private void Click_First(object sender, MouseButtonEventArgs e)
		{
		
		}

		private void Click_Second(object sender, MouseButtonEventArgs e)
		{
		
		}

		private void Click_Result(object sender, MouseButtonEventArgs e)
		{
		
		}

		MatrixVM SetMatrix(string matrix)
		{
			if(matrix == null) return null;
			MatrixVM temp = new MatrixVM(null);
			int i = 0;
			matrix = matrix.Remove(matrix.Length - 2, 2);
			foreach(string s in matrix.Split(new char[] { ',' }))
			{
				temp[i++] = Convert.ToSingle(s);
			}
			return temp;
		}

	}
}
