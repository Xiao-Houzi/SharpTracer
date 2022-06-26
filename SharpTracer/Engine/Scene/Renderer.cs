using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using SharpGL;
using SharpTracer.Engine.Graphics;
using SharpTracer.Model;

namespace SharpTracer.Engine
{
    public class Renderer
	{
		public Dictionary<string, Tuple<string, string>> shaders
		{
			get;set;
		}

		public float Delta
		{
			get
			{
				return _delta;
			}
		}

        public float FPS
		{
			get; set;
		}

		public Renderer(ProjectRenderer projectRenderer)
		{
			shaders = new Dictionary<string, Tuple<string, string>>();
			LoadShader("Default");
			LoadShader("Texture");
			LoadShader("Hex");

			_frameTimes = new List<float>();
			_projectRenderer = projectRenderer;
			_delta = 0;
		}

		public Point MousePosition
		{
			get;
			internal set;
		}

		public virtual void Initialise(OpenGL gl)
		{
			GLLayer.Initialise(this, gl);
			Application.Current.Dispatcher.Invoke(
				() =>
				_projectRenderer.Initialise()
				);
		}

		/// <summary>
		/// Renderer executed after runnung the active state
		/// </summary>
		public virtual void RendererUpdate()
		{
		}

		public virtual void Update()
		{
			GetDelta();

			_projectRenderer.Update(_delta);
			RendererUpdate();
		}

		public void Render(OpenGL gl)
		{
			GLLayer.BeginFrame(_projectRenderer);
			_projectRenderer.Render(gl);
			GLLayer.EndFrame();
		}

		
		public void Cleanup()
		{
			_projectRenderer.Cleanup();
		}

		private void GetDelta()
		{
			double currentTime = (double)DateTime.Now.Ticks * ticksize;

			_delta = (float)(currentTime - time);

			_frameTimes.Add(_delta);
			if(time == 0) _delta = 0;
			if(_delta > .1f) _delta = .1f;

			if(_frameTimes.Count > 30) _frameTimes.RemoveAt(0);
			FPS = 1/_frameTimes.Average();

			time = currentTime;
		}




		public void InitialiseState(Type state)
		{
			_projectRenderer.Initialise();
		}

		public void SetSize(int width, int height)
		{
			GLLayer.UpdateWindowSize(width, height);
			_projectRenderer.SetSize(width, height);
		}

		public void LoadShader(string v)
		{
			StreamReader sr = new StreamReader(File.OpenRead(string.Format("Resources/Shaders/{0}.vert", v)));
			string vs = sr.ReadToEnd();
			sr = new StreamReader(File.OpenRead(string.Format("Resources/Shaders/{0}.frag", v)));
			string fs = sr.ReadToEnd();
			shaders.Add(v, new System.Tuple<string, string>(vs, fs));
		}
        internal void MouseEnter()
        {
			_projectRenderer.MouseEnter();
        }
        internal void MouseLeave()
        {
			_projectRenderer.MouseLeave();
		}
        internal void MouseUp(Point point)
        {
			_projectRenderer.MouseEnter(point);
		}
        internal void MouseDown(Point point)
        {
			_projectRenderer.MouseEnter(point);
		}
        internal void Scroll(int delta)
        {
			_projectRenderer.MouseScroll(delta);
		}
		internal void MouseMove(Point position)
        {
			Point delta = (Point)(position - _lastPosition);
			_projectRenderer.MouseMove(position, delta);
			_lastPosition = position;
		}

		private const float ticksize = 1 / (float)TimeSpan.TicksPerSecond;
		private double time;
		protected ProjectRenderer _projectRenderer;
		protected float _delta;
		private List<float> _frameTimes;
		private Point _lastPosition;
	}
}
