using GlmSharp;
using SharpTracer.Engine.Scene;

namespace SharpTracer.Scripts
{
    internal class CompassScript : IScript
	{
		public void Initialise(Entity entity)
		{
			_entity = entity;
		}

		public void Run(float delta)
		{
			_entity.Transform.Translate(new vec3( -1 *delta , 0, 0));
		}

		public void Destroy()
		{
			
		}

		private Entity _entity;
	}
}