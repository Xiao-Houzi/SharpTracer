using SharpEngine.Maths;
using SharpTracer.Engine.Scene;
using SharpTracer.Maths;
using System.Collections.Generic;

namespace SharpTracer.model
{
    internal class FileData
	{
		public List<Entity> Geometry { get; internal set; }
		public Transform Pose { get; internal set; }
	}
}