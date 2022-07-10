using GlmSharp;
using SharpGL;
using SharpTracer.Engine.GLAbstraction;
using SharpTracer.Engine.RayTracing;
using SharpTracer.Maths;
using System;
using System.Runtime.InteropServices;

using System.Collections.Generic;



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

            const float X = .525731112119133606f;
            const float Z = .850650808352039932f;
            const float N = 0.0f;



            float[] vertexPositions =
                 {// X			Y			Z			R			G			B			A			U			V			Nx		Ny		nZ	
				  -X,N,Z, 1,1,0,1,0,0,-X,N,Z,
                  X,N,Z,  1,1,0,1,0,0, X,N,Z,
                  -X,N,-Z,  1,1,0,1,0,0, -X,N,-Z,
                  X,N,-Z, 1,1,0,1,0,0,X,N,-Z,
                  N,Z,X,  1,1,0,1,0,0, N,Z,X,
                  N,Z,-X, 1,1,0,1,0,0,N,Z,-X,
                  N,-Z,X, 1,1,0,1,0,0, N,-Z,X,
                  N,-Z,-X, 1,1,0,1,0,0,N,-Z,-X,
                  Z,X,N, 1,1,0,1,0,0,Z,X,N,
                  -Z,X, N, 1,1,0,1,0,0,-Z,X, N,
                  Z,-X,N, 1,1,0,1,0,0,Z,-X,N,
                  -Z,-X, N, 1,1,0,1,0,0,-Z,-X, N,

               };
            uint[] tempTris =
                  {
                     0,4,1,0,9,4,9,5,4,4,5,8,4,8,1,
  8,10,1,8,3,10,5,3,8,5,2,3,2,7,3,
  7,10,3,7,6,10,7,11,6,11,0,6,0,1,6,
  6,1,10,9,0,11,9,11,2,9,2,5,7,2,11
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
            gl.EnableVertexAttribArray(2);
            gl.VertexAttribPointer(0, 3, OpenGL.GL_FLOAT, false, 48, IntPtr.Zero);
            gl.VertexAttribPointer(1, 4, OpenGL.GL_FLOAT, false, 48, new IntPtr(12));
            gl.VertexAttribPointer(2, 2, OpenGL.GL_FLOAT, false, 48, new IntPtr(28));
            gl.VertexAttribPointer(2, 2, OpenGL.GL_FLOAT, false, 48, new IntPtr(36));
        }


        Index VertexForEdge(Dictionary<KeyValuePair<Index, Index>, Index> lookup, List<vec3> vertices, Index first, Index second)
        {
            KeyValuePair<Index, Index> key = new KeyValuePair<Index, Index>(first, second);
            if (key.Key.Value > key.Value.Value)
                key = new KeyValuePair<Index, Index>(second, first);

            var inserted = lookup[key] = vertices.Count;
            if (false)
            {
                var edge0 = vertices[first];
                var edge1 = vertices[second];
                var point = new vec3(edge0 + edge1).Normalized;
                vertices.Add(point);
            }

            return inserted.Value;
        }

        List<Triangle> Subdivide(List<vec3> vertices, List<Triangle> triangles)
        {
            Dictionary<KeyValuePair<Index, Index>, Index> lookup = new Dictionary<KeyValuePair<Index, Index>, Index>();
            List<Triangle> result = new List<Triangle>();

            foreach (Triangle triangle in triangles)
            {
                vec3[] mid = new vec3[3];
                //for (Index edge = 0; edge.Value < 3; ++edge)
                //{
                //    //mid[edge] = VertexForEdge(lookup, vertices,
                //    // edge, edge.Value + 1 % 3);
                //}

                result.Add(new Triangle(triangle.vertex[0], mid[0], mid[2]));
                result.Add(new Triangle(triangle.vertex[1], mid[1], mid[0]));
                result.Add(new Triangle(triangle.vertex[2], mid[2], mid[1]));
                result.Add(new Triangle(mid[0], mid[1], mid[2]));
            }

            return result;
        }

        KeyValuePair<List<vec3>, List<Triangle>> MakeIcosphere(int subdivisions)
        {
            List<vec3> vertices = new List<vec3>();
            List<Triangle> triangles = new List<Triangle>();

            for (int i = 0; i < subdivisions; ++i)
            {
                triangles = Subdivide(vertices, triangles);
            }

            return new KeyValuePair<List<vec3>, List<Triangle>>( vertices, triangles);
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
        float _radius = 1.0f;
        #endregion
    }

    struct Triangle
    {
        public Triangle(vec3 v1, vec3 v2, vec3 v3)
        {
            vertex[0] = v1;
            vertex[1] = v2;
            vertex[2] = v3;
        }

        public vec3[] vertex = new vec3[3];
    };
}
