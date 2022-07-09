using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTracer.Engine.Scene.Textures
{
    public interface ITexture
    {
        public abstract vec4 Value(float u, float v, vec3 p);
    }
}
