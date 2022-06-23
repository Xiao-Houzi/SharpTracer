using SharpEngine.Engine;
using SharpEngine.Engine.Graphics;
using GlmSharp;
using SharpTracer;
using SharpGL;

namespace SharpTracer.RendererScripting.States
{
	public class ProjectState : ViewState
	{
		public ProjectState()
		{
			SetCameraLayer("Model");
		}

		public override void Initialise()
		{
			BackColour = new Colour(0.0f, 0.0f, 0.1f, 1.0f);
			Layers["Background"].AddEntity(new Entity("Background", new Mesh(new Billboard()), new Material(), null)); ;
			Layers["Background"].Entities[0].Transform.Translation = new vec3(-0.0f, -0.0f, -0.0f);
			Layers["Background"].Entities[0].Transform.Scale = new vec3(1.8f, 1f, 0.0f);

			Layers["Foreground"].AddEntity(new Entity("Compass", new Mesh(new Gizmo()), new Material(), new CompassScript())); ;
			Layers["Foreground"].Entities[0].Transform.Translation = new vec3(-1.40f, -0.85f, 0.2f);

			Entity centroid = new Entity("Centroid", new Mesh(new Gizmo()), new Material(), null);
			centroid.Transform.Scale = new vec3(2f);
			centroid.Transform.Translation = new vec3(0f);

			Layers["Model"].AddEntity(centroid); ;

			Camera.Zoom = 5;
		}

		public override void Run(float delta)
		{
			base.Run(delta);
			Layers["Model"].Camera = Camera;
		}

		public void ClearAgents()
		{
			Layers["Model"].Clear();

		}
	}
}