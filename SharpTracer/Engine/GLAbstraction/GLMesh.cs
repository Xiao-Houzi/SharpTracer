using System;
using SharpGL;

namespace SharpTracer.Engine.Graphics
{
	public enum DisplayType
	{
		DISPLAY_POINTS,
		DISPLAY_WIRES,
		DISPLAY_SOLID,
	};

	public class GLMesh
	{
		public int IndexCount
		{ get { return _indices; } }
		public uint[] VAO
		{ get { return _vao; } }

		public GLMesh(String name)
		{
			_name = name;
			_vao = new uint[] { uint.MaxValue };
			_buffers = new uint[2];
			_indices = 0;
		}

		~GLMesh()
		{
			if (_vao[0] != uint.MaxValue)
			{
				//GLLayer.GL.DeleteBuffers(2, buffers);
				//GLLayer.GL.DeleteVertexArrays(1, vao);
				_vao[0] = uint.MaxValue;
			}
			return; // TODO gracefully delete buffers from opengl
		}

		public void Initialise()
		{
			if(!_initialised)
			{
				InitialiseGeometry();
			}
		}

		public virtual void InitialiseGeometry()
        {
			LoadMesh(_name);
        }

		void LoadMesh(string filepath)
		{
			// TODO implement loading geometry
		}

		#region Private
		string _name;
		bool _initialised = false;

		protected int _indices;
		protected uint[] _vao;
		protected uint[] _buffers;
		#endregion
	}
}
