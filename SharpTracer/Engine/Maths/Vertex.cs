using SharpEngine.Engine.Graphics;
using GlmSharp;
using System;
using System.IO;

namespace SharpEngine.Maths
{
	public class Vertex
	{
		private vec3 _position;
		private vec4 _colour;
		private float _intensity;

		public vec3 Position
		{
			get
			{
				return _position;
			}
			set
			{
				_position = value;
			}
		}

		public float Intensity
		{
			get
			{
				return _intensity;
			}
			set
			{
				_intensity = value;
			}
		}
		public vec4 Colour
		{
			get
			{
				return _colour;
			}
			set
			{
				_colour = value;
			}
		}

		public Vertex()
		{

		}

		public Vertex(vec3 vertex)
		{
			_position = vertex;
		}

		public Vertex(Vertex other)
		{
			_position = other._position;
			_intensity = other._intensity;
			_colour = other._colour;
		}

		public Vertex(vec3 position, float intensity, vec4 colour)
		{
			_position = position;
			_intensity = intensity;
			_colour = colour;
		}

		public void DeSerialise(BinaryReader stream)
		{
			throw new NotImplementedException();
		}

		public void Serialise(BinaryWriter fs)
		{
			throw new NotImplementedException();
		}
	}
}
