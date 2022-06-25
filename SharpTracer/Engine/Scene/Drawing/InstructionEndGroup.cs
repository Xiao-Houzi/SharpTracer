using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGL;

namespace SharpEngine.Engine.Graphics
{
	public class InstructionEndGroup : IDrawInstruction
	{
		public void Execute(OpenGL gl)
		{
			gl.End();
		}
	}
}
