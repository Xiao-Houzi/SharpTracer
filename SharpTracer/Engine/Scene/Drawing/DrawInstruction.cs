using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGL;

namespace SharpEngine.Engine.Graphics
{
	public interface IDrawInstruction
	{
		void Execute(OpenGL gl);
	}
}
