using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTracer.Engine.Scene.Textures
{
    internal class TextureConstant : ITexture
    {
        public TextureConstant(vec4 colour)
        {
            _colour = colour;
        }
        public vec4 Value(float u, float v, vec3 p)
        {
            return _colour;
        }

        vec4 _colour;
    }
}
