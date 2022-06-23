using GlmSharp;

namespace SharpEngine.Engine.Graphics
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
            _textures = new uint[] { 0,1,2};
        }
        public void SetColour(vec4 colour, int slot = 0)
        {
            float[,] val = new float[4, 4];
            val[slot, 0] = colour.r;
            val[slot, 1] = colour.g;
            val[slot, 2] = colour.b;
            val[slot, 3] = colour.a;

        }

        private uint[] _textures;
    }
}