using GlmSharp;
using SharpTracer.Engine.Graphics;
using SharpTracer.Engine.Maths;
using SharpTracer.Engine.RayTracing;
using SharpTracer.Engine.Scene.Textures;

namespace SharpTracer.Engine.Scene
{
    public class Material
    {
        public string Shader
        { get; set; }
        public ShaderData ShaderData
        { get; set; }
        public uint[] Texture
        {
            get { return _textures; }
            set { _textures = value; }
        }
        public vec4 Colour
        {
            get;
            set;
        }
        public int PointSize { get; internal set; }


        public Material()
        {
            Shader = "Default";
            _textures = new uint[] { 0, 1, 2 };
        }
        public void SetColour(vec4 colour, int slot = 0)
        {
            float[,] val = new float[4, 4];
            val[slot, 0] = colour.r;
            val[slot, 1] = colour.g;
            val[slot, 2] = colour.b;
            val[slot, 3] = colour.a;

        }

        public virtual bool Scattered(Ray ray, Hit hit, ref vec4 attenuation, ref Ray scattered)
        {
            vec3 target = hit.p + hit.n + Noise.Random3();
            scattered = new Ray(hit.p, target - hit.p, ray.Time);
            attenuation = albedo.Value(0, 0, hit.p);
            return true;
        }
        public virtual vec4 Emmited(float u, float v, ref vec3 p)
        {
            return emit.Value(u, v, p);
        }

        ITexture albedo;
        ITexture emit;

        private uint[] _textures;
    }
}