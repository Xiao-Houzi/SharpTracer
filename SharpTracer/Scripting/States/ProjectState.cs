using SharpTracer.Engine;
using SharpTracer.Engine.Graphics;
using GlmSharp;
using SharpTracer.Engine.Scene;
using SharpTracer.Engine.Scene.RenderGeometry;

namespace SharpTracer.Scripting.States
{
    public class ProjectState : State
	{


        public ProjectState()
		{
			SetCameraLayer("Model");
		}

		public override void Initialise()
		{
			BackColour = new Colour(0.0f, 0.0f, 0.1f, 1.0f);
			Layers["Background"].AddEntity(new Entity("Background", new Geometry(new MeshPlane()), new Material())); ;


			Layers["Foreground"].AddEntity(new Entity("Compass", new Geometry(new Gizmo()), new Material(), new CompassScript())); ;
			Layers["Foreground"].Entities[0].Transform.Translation = new vec3(0f, 0f, 0f);

			Entity centroid = new Entity("Centroid", new Geometry(new Gizmo()), new Material(), null);
			centroid.Transform.Scale = new vec3(1f);
			centroid.Transform.Translation = new vec3(0f);

			Layers["Model"].AddEntity(centroid);
			Layers["Model"].AddEntity(new Entity("Sphere", new Geometry(new MeshSphere()), new Material(), new SphereScript()));
			Camera.Zoom = 1;
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