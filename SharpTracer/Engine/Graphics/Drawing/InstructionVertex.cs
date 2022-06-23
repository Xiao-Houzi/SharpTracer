using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGL;

namespace SharpEngine.Engine.Graphics
{
	public class InstructionVertex : IDrawInstruction
	{
		float _x, _y, _z;

		public InstructionVertex(float x, float y, float z)
		{
			_x = x;
			_y = y;
			_z = z;
		}

		public void Execute(OpenGL gl)
		{
			gl.Vertex(_x, _y, _z);
		}
	}
}
