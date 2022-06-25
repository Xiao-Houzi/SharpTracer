using GlmSharp;
using SharpTracer.Engine.Graphics;
using SharpTracer.Engine.RayTracing;
using SharpTracer.Maths;
using System;


namespace SharpTracer.Engine.Scene.RenderGeometry
{
    internal class Sphere : Geometry
    {
        public float Radius
        { get { return _radius; } }


        public Sphere(GLMesh mesh) : base(mesh)
        {
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
