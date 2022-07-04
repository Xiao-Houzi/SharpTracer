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
using SharpTracer.Engine;

namespace SharpEngine
{
    /// <summary>
    /// Interaction logic for GL3DView.xaml
    /// </summary>
    public partial class GL3DView : UserControl
    {
        #region fields
        private bool IsInitialised = false;
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
            Renderer.Initialise(GL);
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
            Renderer?.SetSize((int)ActualWidth, (int)ActualHeight);
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Renderer?.MouseDown(Mouse.GetPosition(null));
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Renderer?.MouseUp(Mouse.GetPosition(null));
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            Renderer?.MouseEnter(Mouse.GetPosition(null));
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            Renderer?.MouseLeave(Mouse.GetPosition(null));
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            Renderer?.MouseMove(Mouse.GetPosition(null));
        }

        private void UserControl_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Renderer?.Scroll(e.Delta);
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Renderer?.SetSize((int)ActualWidth, (int)ActualHeight);
        }

        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int x, int y);
    }
}
