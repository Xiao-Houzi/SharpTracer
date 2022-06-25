using GlmSharp;
using SharpTracer.Engine.Scene;

namespace SharpTracer.Scripting
{
    internal class CompassScript : IScript
	{
		Entity entity;
		public void Initialise(Entity entity)
		{
			this.entity = entity;
		}

		public void Run(float delta)
		{
			entity.Transform.Translate(new vec3( -1 *delta , 0, 0));
		}

		public void Destroy()
		{
			
		}
	}
}