using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGL;
using SharpEngine.Maths;

namespace SharpEngine.Engine.Graphics
{
	public class Mesh
	{
		private bool initialised;

		public Entity Parent
		{
			get; set;
		}

		public float Scale
		{
			get; set;
		}

		public Geometry Geometry
		{
			get; set;
		}
		public DisplayType DisplayMode
		{
			get; set;
		}

		public Mesh(Geometry g, float scale = 1.0f)
		{
			Scale = scale;
			Geometry = g;
			initialised = false;
		}

		public void Initialise()
		{
			if(!initialised)
				Geometry.InitiliseGeometry();
			initialised = true;
		}
	}
}
