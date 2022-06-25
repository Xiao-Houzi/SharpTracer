using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGL;

namespace SharpEngine.Engine.Graphics
{
	public enum mode
	{
		Triangles,
		Points
	}

	public class InstructionBeginGroup : IDrawInstruction
	{
		SharpGL.Enumerations.BeginMode _mode;

		public InstructionBeginGroup(mode mode)
		{
			switch (mode)
			{
				case mode.Triangles:
					_mode = SharpGL.Enumerations.BeginMode.Triangles;
					break;
				case mode.Points:
					_mode = SharpGL.Enumerations.BeginMode.Points;
					break;
			}
		}

		public void Execute(OpenGL gl)
		{
			gl.Begin(_mode);
		}
	}
}
