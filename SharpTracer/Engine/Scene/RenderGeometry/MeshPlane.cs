using SharpGL;
using SharpTracer.Engine.GLAbstraction;
using System;
using System.Runtime.InteropServices;

namespace SharpTracer.Engine.Scene.RenderGeometry
{
	public class MeshPlane : Geometry
	{

		public MeshPlane() : base("Plane")
		{
			OpenGL gl = GLLayer.GL;
			// initialise geometry
			float[] vertexPositions =
				  {// X			Y			Z			R			G			B			A			U			V			Nx		Ny		nZ	
						-1f,        -1f,        +0f,        .1f,        .15f,       .2f,        0.5f,       0.0f,       0.0f,
						+1f,        -1f,        +0f,        .1f,        .15f,       .2f,        1.0f,      1.0f,      0.0f,
						+1f,        +1f,        +0f,        .1f,        .15f,       .2f,        0.5f,      1.0f,      1.0f,
						-1f,        +1f,        +0f,        .1f,        .15f,       .2f,        1.0f,      0.0f,      1.0f,
			   };
			uint[] tempTris =
					{
					0,2,3,  0,1,2,  // face 1
	            };

			_indices = tempTris.Length;

			// set the VERTEX buffer
			IntPtr vertexPtr = GCHandle.Alloc(vertexPositions, GCHandleType.Pinned).AddrOfPinnedObject();
			IntPtr edgePtr = GCHandle.Alloc(tempTris, GCHandleType.Pinned).AddrOfPinnedObject();

			gl.GenVertexArrays(1, _vao);
			gl.GenBuffers(2, _buffers);

			gl.BindVertexArray(_vao[0]);
			// 2. copy our vertices array in a vertex buffer for OpenGL to use
			gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, _buffers[0]);
			gl.BufferData(OpenGL.GL_ARRAY_BUFFER, vertexPositions.Length * sizeof(float), vertexPtr, OpenGL.GL_STATIC_DRAW);

			// 3. copy our index array in a element buffer for OpenGL to use
			gl.BindBuffer(OpenGL.GL_ELEMENT_ARRAY_BUFFER, _buffers[1]);
			gl.BufferData(OpenGL.GL_ELEMENT_ARRAY_BUFFER, _indices * sizeof(uint), edgePtr, OpenGL.GL_STATIC_DRAW);

			gl.EnableVertexAttribArray(0);
			gl.EnableVertexAttribArray(1);
			gl.EnableVertexAttribArray(2);
			gl.VertexAttribPointer(0, 3, OpenGL.GL_FLOAT, false, 36, IntPtr.Zero);
			gl.VertexAttribPointer(1, 4, OpenGL.GL_FLOAT, false, 36, new IntPtr(12));
			gl.VertexAttribPointer(2, 2, OpenGL.GL_FLOAT, false, 36, new IntPtr(28));
		}

	}
}
