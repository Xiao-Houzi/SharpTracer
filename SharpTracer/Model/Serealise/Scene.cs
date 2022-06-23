using SharpEngine.Engine.Graphics;
using System.Collections.Generic;

namespace SharpTracer.Model
{
    public class Scene
    {
        public IEnumerable<Entity> Entities { get; internal set; }
    }
}