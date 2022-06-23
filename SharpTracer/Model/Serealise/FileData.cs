using SharpEngine.Engine.Graphics;
using SharpEngine.Maths;
using System.Collections.Generic;

namespace SharpTracer.model
{
	internal class FileData
	{
		public List<Entity> Geometry { get; internal set; }
		public Transform Pose { get; internal set; }
	}
}