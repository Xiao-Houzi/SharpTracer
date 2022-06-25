using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGL;
using SharpEngine.Maths;
using SharpTracer.Engine.Scene;

namespace SharpEngine.Engine.Graphics
{
    public class InstructionColour : IDrawInstruction
	{
		private Colour _colour;

		public InstructionColour(Colour c)
		{
			_colour = c;
		}

		public void Execute(OpenGL gl)
		{
			gl.Color(_colour.R, _colour.G, _colour.B, _colour.A);
		}
	}
}
