using GlmSharp;
using SharpTracer.Engine.Scene;

namespace SharpTracer.Scripting
{
    class CubeScript : IScript
	{
		// Add yor script vars for persistence
		Entity Entity;
		float time;

		void IScript.Initialise(Entity entity)
		{
			Entity = entity;
			Entity.Transform.Translation = new vec3(0.0f);
			Entity.Transform.Scale = new vec3(1.0f);
			Entity.Material.PointSize = 2;
		}

		void IScript.Run(float delta)
		{
			time += delta;

			Entity.Rotate( 0, delta, 0);
		}

		void IScript.Destroy()
		{
			//throw new NotImplementedException();
		}
	}
}
