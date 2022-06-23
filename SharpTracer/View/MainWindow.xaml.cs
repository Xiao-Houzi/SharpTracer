using System.Windows;
using SharpTracer.ViewModels;

namespace SharpTracer.View
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e )
		{
			if(! ((MainWindowVM)DataContext).AppCloseRequested)
				e.Cancel = true;
		}

		private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if(e.Key == System.Windows.Input.Key.W)
			{
				SharpTracerModel.Keys["Forward"] = true;
			}
			if(e.Key == System.Windows.Input.Key.S)
			{
				SharpTracerModel.Keys["Backward"] = true;
			}
			if(e.Key == System.Windows.Input.Key.Q)
			{
				SharpTracerModel.Keys["LeftTilt"] = true;
			}
			if(e.Key == System.Windows.Input.Key.E)
			{
				SharpTracerModel.Keys["RightTilt"] = true;
			}
			if(e.Key == System.Windows.Input.Key.A)
			{
				SharpTracerModel.Keys["LeftStrafe"] = true;
			}
			if(e.Key == System.Windows.Input.Key.D)
			{
				SharpTracerModel.Keys["RightStrafe"] = true;
			}
		}

		private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if(e.Key == System.Windows.Input.Key.W)
			{
				SharpTracerModel.Keys["Forward"] = false;
			}
			if(e.Key == System.Windows.Input.Key.S)
			{
				SharpTracerModel.Keys["Backward"] = false;
			}
			if(e.Key == System.Windows.Input.Key.Q)
			{
				SharpTracerModel.Keys["LeftTilt"] = false;
			}
			if(e.Key == System.Windows.Input.Key.E)
			{
				SharpTracerModel.Keys["RightTilt"] = false;
			}
			if(e.Key == System.Windows.Input.Key.A)
			{
				SharpTracerModel.Keys["LeftStrafe"] = false;
			}
			if(e.Key == System.Windows.Input.Key.D)
			{
				SharpTracerModel.Keys["RightStrafe"] = false;
			}
		}
	}
}
