using GlmSharp;
using SharpGL;
using SharpTracer.Engine.GLAbstraction;
using SharpTracer.Engine.RayTracing;
using SharpTracer.Maths;
using System;
using System.Runtime.InteropServices;


namespace SharpTracer.Engine.Scene.RenderGeometry
{
	public class MeshSphere : Geometry
	{
		public float Radius
		{ get { return _radius; } }

		public MeshSphere() : base("Sphere")
		{
			OpenGL gl = GLLayer.GL;
			// initialise geometry
			float[] vertexPositions =
				 {// X			Y			Z			R			G			B			A			U			V			Nx		Ny		nZ	
					-0.5f,      -0.5f,      +0.5f,  0.0f,       0.0f,       1.0f,       0.5f,       0,          0,
					+0.5f,  -0.5f,      +0.5f,  1.0f,      0.0f,        1.0f,       0.5f,       1,          0,
					+0.5f,  +0.5f,  +0.5f,  0.0f,      1.0f,        1.0f,       0.5f,       1,          1,
					-0.5f,      +0.5f,  +0.5f,  1.0f,       1.0f,       1.0f,       0.5f,       0,          1,
					-0.5f,      -0.5f,      -0.5f,      0.0f,       0.0f,       0.0f,       0.5f,       1,          0,
					+0.5f,  -0.5f,      -0.5f,      1.0f,      0.0f,        0.0f,       0.5f,       0,          0,
					+0.5f,  +0.5f,  -0.5f,      0.0f,      1.0f,        0.0f,       0.5f,       0,          1,
					-0.5f,      +0.5f,  -0.5f,      1.0f,       1.0f,       0.0f,       0.5f,       1,          1,
			   };
			uint[] tempTris =
				  {
					0,2,3,  0,1,2,  // face 1
					1,5,6,  6,2,1,  // face 2
					4,0,3,  3,7,4,  // face 3
					4,5,1,  1,0,4,  // face 4
					3,2,6,  6,7,3,  // face 5
					5,4,7,  7,6,5,  // face 6
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
		public override bool Test(Ray ray, Transform transform, Material material, float min, float max, ref Hit hit)
		{
			vec3 OC = ray.Origin - transform.Translation;

			float a = vec3.Dot(ray.Direction, ray.Direction);
			float b = vec3.Dot(OC, ray.Direction);
			float c = vec3.Dot(OC, OC) - _radius * _radius;

			float d = b * b - a * c;

			float temp;
			if (d > 0)
			{
				temp = (-b - (float)Math.Sqrt(d)) / a;
				if (temp < max && temp > min)
				{
					hit.Material = material;
					hit.t = temp;
					hit.p = ray.PointAt(hit.t);
					hit.n = (hit.p - transform.Translation) / _radius;
					return true;
				}
				temp = (-b + (float)Math.Sqrt(d)) / a;
				if (temp < max && temp > min)
				{
					hit.Material = material;
					hit.t = temp;
					hit.p = ray.PointAt(hit.t);
					hit.n = (hit.p - transform.Translation) / _radius;
					return true;
				}
			}
			return false;
		}

		#region Private
		float _radius;
		#endregion
	}
}
