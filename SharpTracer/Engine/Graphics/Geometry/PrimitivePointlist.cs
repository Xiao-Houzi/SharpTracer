using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Engine.Graphics
{
	public class PrimitivePointlist : Geometry
	{
		public PrimitivePointlist(OpenGL gl) : base("Cube")
		{

		}

		public override void Initialise()
		{
			OpenGL gl = GLLayer.GL;
			// initialise geometry
			float[] vertexPositions =
				  {// X			Y			Z			R			G			B			A			U			V			Nx		Ny		nZ	
						+0.0f,	+0.1f,	+0.0f,	0.0f,		0.0f,		1.0f,		0.1f,
						+0.1f,	+0.1f,	+0.0f,	1.0f,		0.0f,		1.0f,      0.2f,
						+0.2f,	+0.1f,	+0.0f,	1.0f,		1.0f,     1.0f,      0.3f,
						+0.3f,	+0.1f,	+0.0f,	0.0f,		1.0f,		1.0f,      0.4f,
						+0.4f,	+0.1f,	+0.0f,	0.0f,		0.0f,      0.0f,     0.5f,
						+0.5f,	+0.1f,	+0.0f,	1.0f,		0.0f,      0.0f,     0.6f,
						+0.6f,	+0.1f,	+0.0f,	1.0f,		1.0f,      0.0f,     0.7f,
						+0.7f,	+0.1f,	+0.0f,	0.0f,		1.0f,      0.0f,		0.8f,
			   };
			
			ulong[] tempPoints =
				  {
				  0,1,2,3,4,5,6,7,8,9
			   };
			indices = tempPoints.Length;

			// set the VERTEX buffer
			IntPtr vertexPtr = GCHandle.Alloc(vertexPositions, GCHandleType.Pinned).AddrOfPinnedObject();
			IntPtr indexPtr = GCHandle.Alloc(tempPoints, GCHandleType.Pinned).AddrOfPinnedObject();

			gl.GenVertexArrays(1, vao);
			gl.GenBuffers(2, buffers);

			gl.BindVertexArray(vao[0]);
			// 2. copy our vertices array in a vertex buffer for OpenGL to use
			gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, buffers[0]);
			gl.BufferData(OpenGL.GL_ARRAY_BUFFER, vertexPositions.Length * sizeof(float), vertexPtr, OpenGL.GL_STATIC_DRAW);

			// 3. copy our index array in a element buffer for OpenGL to use
			gl.BindBuffer(OpenGL.GL_ELEMENT_ARRAY_BUFFER, buffers[1]);
			gl.BufferData(OpenGL.GL_ELEMENT_ARRAY_BUFFER, indices * sizeof(ulong), indexPtr, OpenGL.GL_STATIC_DRAW);

			gl.EnableVertexAttribArray(0);
			gl.EnableVertexAttribArray(1);
			gl.VertexAttribPointer(0, 3, OpenGL.GL_FLOAT, false, 28, IntPtr.Zero);
			gl.VertexAttribPointer(1, 4, OpenGL.GL_FLOAT, false, 28, new IntPtr(sizeof(float)*3));

			
		}
	}
}
