using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using SharpEngine.Engine.Graphics;
using SharpGL;

namespace SharpEngine.Engine
{
	public class Renderer
	{
		const float ticksize  = 1/(float)TimeSpan.TicksPerSecond;
		private double time;
		protected StateMachine _stateMachine;
		protected float _delta;

		public Dictionary<string, Tuple<string, string>> shaders
		{
			get;set;
		}

		public State CurrentState
		{
			get
			{
				return _stateMachine.CurrentState;
			}
		}
		public float Delta
		{
			get
			{
				return _delta;
			}
		}

		private List<float> ft;
		public float FPS
		{
			get; set;
		}

		public Renderer()
		{
			shaders = new Dictionary<string, Tuple<string, string>>();
			ft = new List<float>();
			_stateMachine = new StateMachine();

			_delta = 0;
		}

		public Point MouseMove
		{
			get;
			internal set;
		}

		public virtual void Initialise(OpenGL gl)
		{
			GLLayer.Initialise(this, gl);
			InitialiseStatemachine();
		}

		public void InitialiseStatemachine()
		{
			Application.Current.Dispatcher.Invoke(
				()=>
				_stateMachine.Initialise(GLLayer.GL)
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

			_stateMachine.Update(_delta);
			RendererUpdate();
		}

		public void Render(OpenGL gl)
		{
			GLLayer.BeginFrame(_stateMachine.CurrentState);
			_stateMachine.Render(gl);
			GLLayer.EndFrame();
		}

		
		public void Cleanup()
		{
			_stateMachine.Cleanup();
		}

		private void GetDelta()
		{
			double currentTime = (double)DateTime.Now.Ticks * ticksize;

			_delta = (float)(currentTime - time);

			ft.Add(_delta);
			if(time == 0) _delta = 0;
			if(_delta > .1f) _delta = .1f;

			if(ft.Count > 30) ft.RemoveAt(0);
			FPS = 1/ft.Average();

			time = currentTime;
		}

		public void AddState(State state)
		{
			_stateMachine.States.Remove(state.GetType());
			_stateMachine.States.Add(state.GetType(), state);
		}

		public void SetCurrentState(Type state)
		{
			_stateMachine.CurrentState = _stateMachine.States[state];
		}

		public void InitialiseState(Type state)
		{
			_stateMachine.States[state].InitialiseState();
		}

		public void SetSize(int width, int height)
		{
			GLLayer.UpdateWindowSize(width, height);
		}

		public void LoadShader(string v)
		{
			StreamReader sr = new StreamReader(File.OpenRead(string.Format("Resources/Shaders/{0}.vert", v)));
			string vs = sr.ReadToEnd();
			sr = new StreamReader(File.OpenRead(string.Format("Resources/Shaders/{0}.frag", v)));
			string fs = sr.ReadToEnd();
			shaders.Add(v, new System.Tuple<string, string>(vs, fs));
		}
	}
}
