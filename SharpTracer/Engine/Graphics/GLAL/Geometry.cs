using System;
using SharpGL;

namespace SharpEngine.Engine.Graphics
{
	public enum DisplayType
	{
		DISPLAY_POINTS,
		DISPLAY_WIRES,
		DISPLAY_SOLID,
	};

	public class Geometry
	{
		public int indices;
		public float pointSize;
		protected string _name;

		public uint[] vao;
		public uint[] buffers;

		public Geometry(String name)
		{
			_name = name;
			vao = new uint[] { uint.MaxValue };
			buffers = new uint[2];

			pointSize = 1;
			indices = 0;
		}

		~Geometry()
		{
			if (vao[0] != uint.MaxValue)
			{
				//GLLayer.GL.DeleteBuffers(2, buffers);
				//GLLayer.GL.DeleteVertexArrays(1, vao);
				vao[0] = uint.MaxValue;
			}
			return; // TODO gracefully delete buffers from opengl
		}

		public virtual void InitiliseGeometry()
		{
			if(vao[0] == uint.MaxValue)
			{
				Initialise();
			}
		}

		public virtual void Initialise()
		{

		}

		void LoadMesh(string filepath)
		{
			// TODO implement loading geometry
		}

		void SetPointSize(float Size)
		{
			pointSize = Size;
		}

		int GetIndexCount()
		{
			return indices;
		}
	}
}
