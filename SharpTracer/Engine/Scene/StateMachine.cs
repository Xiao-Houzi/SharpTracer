using System;
using System.Collections.Generic;
using System.Linq;
using SharpGL;
using SharpTracer.Engine;

namespace SharpEngine.Engine
{
	public class StateMachine
	{
		public State CurrentState { get; internal set; }
		public Dictionary<Type, State> States { get; set; }

		//
		public StateMachine()
		{
			States = new Dictionary<Type, State>();
			CurrentState = new State();
		}

		public void Initialise(OpenGL gl)
		{
			foreach (State state in States.Values)
				state.InitialiseState();
		}

		internal void Update(float delta)
		{
			if (CurrentState != null)
				CurrentState.Process(delta);
			else
				throw new RendererException("No currentstate");
		}

		internal void Render(OpenGL gl)
		{
			if (CurrentState != null)
				CurrentState.Render(gl);
			else
				throw new RendererException("No currentstate");
		}

		internal void Cleanup()
		{
			throw new NotImplementedException();
		}
	}
}
