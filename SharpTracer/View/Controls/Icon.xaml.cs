using System.Windows;
using System.Windows.Controls;

namespace SharpTracer.View.Controls
{
	/// <summary>
	/// Interaction logic for Icon.xaml
	/// </summary>
	public partial class Icon : UserControl
	{
		#region Dependency Properties
		public static readonly DependencyProperty TopLayerProperty =
		DependencyProperty.Register("TopLayer", typeof(string), typeof(Icon));

		public static readonly DependencyProperty BottomLayerProperty =
		DependencyProperty.Register("BottomLayer", typeof(string), typeof(Icon));

		public static readonly DependencyProperty BottomColourProperty =
		DependencyProperty.Register("BottomColour", typeof(string), typeof(Icon));

		public static readonly DependencyProperty TopColourProperty =
		DependencyProperty.Register("TopColour", typeof(string), typeof(Icon));

		public static readonly DependencyProperty SizeProperty =
		DependencyProperty.Register("Size", typeof(int), typeof(Icon));
		#endregion

		public string TopLayer
		{
			get { return (string)GetValue(TopLayerProperty); }
			set { SetValue(TopLayerProperty, value); }
		}

		public string BottomLayer
		{
			get { return (string)GetValue(BottomLayerProperty); }
			set { SetValue(BottomLayerProperty, value); }
		}
		
		public string TopColour
		{
			get { return (string)GetValue(TopColourProperty); }
			set { SetValue(TopColourProperty, value); }
		}

		public string BottomColour
		{
			get { return (string)GetValue(BottomColourProperty); }
			set { SetValue(BottomColourProperty, value); }
		}

		public int Size
		{
			get { return (int)GetValue(SizeProperty); }
			set { SetValue(SizeProperty, value); }
		}

		public Icon()
		{
			//Size = 16;
			//BottomLayer = "";
			//TopLayer = "";
			//BottomColour = "DimGray";
			//TopColour = "Black";
			DataContext = this;
			InitializeComponent();
		}
	}
}
