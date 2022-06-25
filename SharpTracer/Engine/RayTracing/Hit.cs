using GlmSharp;
using SharpTracer.Engine.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTracer.Engine.RayTracing
{
    public class Hit
    {
        public float t, u, v;
        public vec3 p,n,c;
        public Material Material;
    }
}
