using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTracer.Engine.Scene
{
    /// <summary>
    /// Colour class for 3D graphics storing values as floats
    /// </summary>
    public class Colour
    {
        private float[] _rgba;

        public float R { get { return _rgba[0]; } set { _rgba[0] = value; } }
        public float G { get { return _rgba[1]; } set { _rgba[1] = value; } }
        public float B { get { return _rgba[2]; } set { _rgba[2] = value; } }
        public float A { get { return _rgba[3]; } set { _rgba[3] = value; } }

        /// <summary>
        /// Colour values as an array of bytes
        /// </summary>
        public byte[] Bytes
        {
            get
            {
                byte[] bytes = new byte[4];
                for (int i = 0; i < 4; i++)
                    bytes[i] = (byte)(_rgba[i] * 255);
                return bytes;
            }
            set
            {
                for (int i = 0; i < 4; i++)
                    _rgba[i] = value[i] / 255.0f;
            }
        }

        /// <summary>
        /// Default constructior initialises a colour as white
        /// </summary>
        public Colour()
        {
            _rgba = new float[4];
            for (int i = 0; i < 4; i++)
                _rgba[i] = 1.0f;
        }

        /// <summary>
        /// initialise a colour with specific values
        /// </summary>
        /// <param name="r">red value</param>
        /// <param name="g">green value</param>
        /// <param name="b">blue value</param>
        /// <param name="a">alpha (opacity) value</param>
        public Colour(float r, float g, float b, float a) : this()
        {
            _rgba[0] = r;
            _rgba[1] = g;
            _rgba[2] = b;
            _rgba[3] = a;
        }
    }
}
