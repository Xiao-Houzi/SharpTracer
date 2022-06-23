using SharpEngine.Engine;
using SharpEngine.Engine.Graphics;
using GlmSharp;
using SharpTracer.RendererScripting.Scripts;

namespace SharpTracer.RendererScripting.States
{
	public class ViewState : State
	{
		//uint image = 0;
		//uint depth = 0;

		public ViewState()
		{
			Layers.Add("Model", new Layer());
			SetCameraLayer("Model");
		}

		public static mat4 CurrentMatrix { get; set; }

		public override void Initialise()
		{
			CurrentMatrix = new mat4();
			BackColour = new Colour(0.0f, 0.0f, 0.1f, 1.0f);
			Layers["Background"].AddEntity(new Entity("Background", new Mesh(new Billboard()), new Material(), null));
			Layers["Background"].Entities[0].Transform.Translation = new vec3(-0.0f, -0.0f, -0.0f);
			Layers["Background"].Entities[0].Transform.Scale = new vec3(1.8f, 1f, 0.0f);

			Layers["Foreground"].AddEntity(new Entity("Compass", new Mesh(new Gizmo()), new Material(), new CompassScript())); ;
			Layers["Foreground"].Entities[0].Transform.Translation = new vec3(-1.40f, -0.85f, 0.2f);

			Layers["Model"].AddEntity(new Entity("Cube1", new Mesh(new PrimitiveCube()), new Material(), new CubeScript()));
			Layers["Model"].Entities[0].Transform.Translation = new vec3(0, 0, 0);

			Camera.Zoom = 5;
		}

		public override void Run(float delta)
		{

			if (SharpTracerModel.Keys["Forward"])
				Layers["Model"].Camera.Advance(SharpTracerModel.Speed * delta);
			if(SharpTracerModel.Keys["Backward"])
				Layers["Model"].Camera.Advance(-SharpTracerModel.Speed * delta);
			if(SharpTracerModel.Keys["LeftTilt"])
				Layers["Model"].Camera.CameraTilt(-1f * delta);
			if(SharpTracerModel.Keys["RightTilt"])
				Layers["Model"].Camera.CameraTilt(1f * delta);
			if(SharpTracerModel.Keys["LeftStrafe"])
				Layers["Model"].Camera.CameraPan(-1f * delta, 0);
			if(SharpTracerModel.Keys["RightStrafe"])
				Layers["Model"].Camera.CameraPan(1f * delta, 0);

			Layers["Foreground"].Entities[0].Transform.Orientation = Camera.Matrix.ToQuaternion;

			float[,] data = new float[4,4]{ { 0,0,0,0 }, {0,0,0,0 }, {0,0,0,0 }, {0,0,0,0 } };

			data[1, 0] = (float)CurrentMatrix[0, 0];
			data[1, 1] = (float)CurrentMatrix[0, 1];
			data[1, 2] = (float)CurrentMatrix[0, 2];
			data[2, 0] = (float)CurrentMatrix[1, 0];
			data[2, 1] = (float)CurrentMatrix[1, 1];
			data[2, 2] = (float)CurrentMatrix[1, 2];
			data[3, 0] = (float)CurrentMatrix[2, 0];
			data[3, 1] = (float)CurrentMatrix[2, 1];
			data[3, 2] = (float)CurrentMatrix[2, 2];

			//Layers["Foreground"].Agents[1].ShaderData = data;

			base.Run(delta);
		}
	}
}
