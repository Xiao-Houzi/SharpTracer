using System;
using System.Collections.Generic;
using SharpGL;
using SharpEngine.Engine.Graphics;
using GlmSharp;

namespace SharpEngine.Engine
{
	public class State
	{
		public Dictionary<string, Layer> Layers;
		public Colour BackColour;
		private string _camLayer;

		public Camera Camera
		{
			get
			{
				return Layers[_camLayer].Camera;
			}
			set
			{
				Layers[_camLayer].Camera = value;
			}
		}

		public float Time
		{
			get;
			set;
		}

		public State()
		{
			BackColour = new Colour() { R = 0, G = 0, B = 0, A = 0 };
			Layers = new Dictionary<string, Layer>();
			Layers.Add("Background", new Layer(true));
			Layers.Add("Foreground", new Layer());
			Layers["Foreground"].IsFlat = true;
		}

		public void Clear()
		{
			foreach(KeyValuePair<string, Layer> L in Layers)
			{
				L.Value.Clear();
			}
		}

		public vec3 SetCameraPose(object p)
		{
			throw new NotImplementedException();
		}

		public void SetCameraLayer(string layer)
		{
			_camLayer = layer;
		}

		public void InitialiseState()
		{
			Initialise();
			foreach (Layer layer in Layers.Values)
				layer?.Initialise();
		}
		public virtual void Initialise()
		{
			throw new NotImplementedException("Please override initialise in your derived class.");
		}

		internal void Process(float delta)
		{
			foreach(Layer layer in Layers.Values)
			foreach (Entity entity in layer.Entities)
				entity.Run(delta);

			Run(delta);
		}

		public virtual void Run(float delta)
		{
		}

		public virtual void Render(OpenGL GL)
		{
			// Render entitys
			foreach(Layer layer in Layers.Values)
				foreach(Entity entity in layer.Entities)
				{
					if(layer.IsFlat)
					{

						GL.Disable(OpenGL.GL_DEPTH_TEST);
						GL.DepthMask(0);
					}
					else
					{
						//GL.Enable(OpenGL.GL_DEPTH_TEST);
						GL.Clear(OpenGL.GL_DEPTH_BUFFER_BIT);
						GL.DepthMask(0);
						GL.ClearStencil(0);
					}
					entity.Render(layer, this);
				}
		}
	}
}
