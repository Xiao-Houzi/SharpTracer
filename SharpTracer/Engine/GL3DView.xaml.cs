using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using SharpEngine.Engine;
using SharpEngine.Maths;
using System.Runtime.InteropServices;
using GlmSharp;
using SharpGL;
using SharpGL.WPF;

namespace SharpEngine
{
	/// <summary>
	/// Interaction logic for GL3DView.xaml
	/// </summary>
	public partial class GL3DView : UserControl
	{
		#region fields
		private bool IsInitialised = false;
		private bool drag = false;
		private double sx, sy, dx, dy;
		#endregion

		public static readonly DependencyProperty RendererProperty =
				 DependencyProperty.Register(
				 "Renderer", typeof(Renderer),
				 typeof(GL3DView));
		public Renderer Renderer
		{ 
			get
			{
				
				return (Renderer)GetValue(RendererProperty);
			}
			set
			{
				SetValue(RendererProperty, value);
			}
		}

		public static readonly DependencyProperty GLProperty =
				 DependencyProperty.Register(
				 "GL", typeof(OpenGL),
				 typeof(GL3DView));
		public OpenGL GL
		{
			get
			{

				return (OpenGL)GetValue(GLProperty);
			}
			set
			{
				SetValue(GLProperty, value);
			}
		}
				

		public double Delta { get { return Renderer.Delta; } }

		public GL3DView()
		{
			InitializeComponent();

			GLC.OpenGLDraw += Render;
			GLC.OpenGLInitialized += Initialise;
		}

		private void Initialise(object sender, OpenGLRoutedEventArgs args)
		{
			GL = args.OpenGL;
		}

		private void Render(object sender, OpenGLRoutedEventArgs args)
		{
			GL = args.OpenGL;

			if (Renderer != null)
			{
				Renderer.Update();
				Renderer.Render(GL);
			}
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs args)
		{
			if (RendererProperty != null && !IsInitialised)
			{
				Renderer.Initialise( GL );
				IsInitialised = true;
			}
			Renderer.SetSize((int)ActualWidth, (int)ActualHeight);
		}

		private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
		{
			drag = true;
			var pos = Mouse.GetPosition(null); //position relative to screen
			Cursor = Cursors.None;

			sx = pos.X;
			sy = pos.Y;
		}

		private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
		{
			Point loc = PointToScreen(new Point(0, 0));
			PresentationSource source = PresentationSource.FromVisual(this);
			Point targetPoints = source.CompositionTarget.TransformFromDevice.Transform(loc);

			SetCursorPos((int)(sx + targetPoints.X), (int)(sy + targetPoints.Y));
			Cursor = Cursors.Arrow;

			drag = false;
		}

		private void UserControl_MouseEnter(object sender, MouseEventArgs e)
		{

		}

		private void UserControl_MouseLeave(object sender, MouseEventArgs e)
		{
			//drag = false;
		}

		private void UserControl_MouseMove(object sender, MouseEventArgs e)
		{
			var pos = Mouse.GetPosition(null); //position relative to view
			
			if (drag)
			{
				dx = pos.X - sx;
				dy = pos.Y - sy;

				Renderer.MouseMove = new Point(dx, dy);
				SetCursorPos((int)sx, (int)sy);
				if (e.MiddleButton == MouseButtonState.Pressed)
				{
					Renderer.CurrentState.Camera.Translate(new vec3( (float)dx/500, (float)-dy/500, 0 ));
				}
				if (e.RightButton == MouseButtonState.Pressed)
				{
					//Renderer.CurrentState.Camera.Orbit((float)dx/500);
					//Renderer.CurrentState.Camera.Incline((float)dy / 500);
				}
			}
		}

		private void UserControl_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			//Renderer.CurrentState.Camera.CameraZoom(e.Delta/1000.0f);
		}

		private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			Renderer?.SetSize((int)ActualWidth, (int)ActualHeight);
		}

		[DllImport("User32.dll")]
		private static extern bool SetCursorPos(int x, int y);
	}
}
