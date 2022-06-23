using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Engine.Graphics
{
	public interface IScript
	{
		void Initialise(Entity entity);

		void Run(float delta);

		void Destroy();
	}
}
