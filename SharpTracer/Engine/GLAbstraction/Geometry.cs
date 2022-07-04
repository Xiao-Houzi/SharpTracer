using System;
using SharpGL;
using SharpTracer.Engine.RayTracing;
using SharpTracer.Engine.Scene;
using SharpTracer.Maths;

namespace SharpTracer.Engine.GLAbstraction
{
	public enum DisplayType
	{
		DISPLAY_POINTS,
		DISPLAY_WIRES,
		DISPLAY_SOLID,
	};

	public class Geometry
	{
		public int IndexCount
		{ get { return _indices; } }
		public uint[] VAO
		{ get { return _vao; } }

		public Geometry(String name)
		{
			_name = name;
			_vao = new uint[] { uint.MaxValue };
			_buffers = new uint[2];
			_indices = 0;
		}

		~Geometry()
		{
			if (_vao[0] != uint.MaxValue)
			{
				//GLLayer.GL.DeleteBuffers(2, buffers);
				//GLLayer.GL.DeleteVertexArrays(1, vao);
				_vao[0] = uint.MaxValue;
			}
			return; // TODO gracefully delete buffers from opengl
		}

		public virtual bool Test(Ray ray, Transform transform, Material material, float min, float max, ref Hit h)
		{
			return false;
		}

		void LoadMesh(string filepath)
		{
			LoadMesh(_name);
		}

		#region Private
		string _name;
		protected int _indices;
		protected uint[] _vao;
		protected uint[] _buffers;
		#endregion
	}

}
