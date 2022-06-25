using SharpTracer.Engine.Graphics;
using SharpTracer.Maths;
using SharpTracer.Engine.RayTracing;

namespace SharpTracer.Engine.Scene.RenderGeometry
{
    public class Geometry
    {
        public GLMesh Mesh
        {
            get => _mesh;
        }

        public Geometry(GLMesh mesh)
        {
            _mesh = mesh;
        }

        public virtual bool Test(Ray ray, Transform transform, Material material, float min, float max, ref Hit h)
        {
            return false;
        }

        public void Initialise()
        {
            _mesh.Initialise();
        }

        #region Private
        GLMesh _mesh;
        #endregion
    }
}
