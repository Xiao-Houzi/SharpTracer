using SharpTracer.Engine;
using SharpTracer.Engine.Graphics;
using System.Windows;
using System.Collections.Generic;
using GlmSharp;
using SharpGL;
using SharpTracer.Scripts;
using SharpTracer.Engine.Scene;
using SharpTracer.Engine.Scene.RenderGeometry;

namespace SharpTracer.Model
{
    public class ProjectRenderer
    {
        public List<Entity> Entities
        { get => _project.Entities; set => _project.Entities = value; }
        public Camera ViewCamera
        { get => _project.ViewCamera; set => _project.ViewCamera = value; }
        public Camera SceneCamera
        { get => _project.SceneCamera; set => _project.SceneCamera = value; }
        public bool Orthographic
        { get; set; }
        public Colour BackColour;
        public float Time
        { get; set; }
        public int CanvasWidth { get => _canvasWidth; }
        public int CanvasHeight { get => _canvasHeight; }
        public float AspectRatio
        { get => _canvasHeight / _canvasWidth; }


        public ProjectRenderer(Project project)
        {
            _project = project;
        }

        public void Initialise()
        {
            BackColour = new Colour(0.0f, 0.0f, 0.1f, 1.0f);

            _gizmo.Transform.Translation = new vec3(0f, 0f, 0f);
            _centroid.Transform.Scale = new vec3(1f);
            _centroid.Transform.Translation = new vec3(0f);
            AddEntity(_background);
            AddEntity(_gizmo); ;
            AddEntity(_centroid);


            AddEntity(new Entity("Sphere", new Geometry(new MeshSphere()), new Material(), new SphereScript()));
        }

        internal void Update(float delta)
        {
            foreach (Entity entity in Entities)
                entity.Run(delta);

            Run(delta);
        }

        public void Run(float delta)
        {
        }
        public virtual void Render(OpenGL GL)
        {
            // Render entitys

            foreach (Entity entity in Entities)
            {
                if (Orthographic)
                {

                    GL.Disable(OpenGL.GL_DEPTH_TEST);
                    GL.DepthMask(0);
                }
                else
                {
                    //GL.Enable(OpenGL.GL_DEPTH_TEST);
                    GL.Clear(OpenGL.GL_DEPTH_BUFFER_BIT);
                    GL.DepthMask(0);
                    GL.ClearStencil(0);
                }
                entity.Render(this);
            }
        }

        public void Cleanup()
        {

        }

        public void AddEntity(Entity entity)
        {
            Entities.Add(entity);
            entity.Initialise();
        }
        public void Clear()
        {
            Entities.Clear();
        }

        internal void SetSize(int width, int height)
        {
            _canvasWidth = width;
            _canvasHeight = height;
        }


        internal virtual void MouseMove(Point position, Point delta)
        {
        }
        internal virtual void MouseScroll(int delta)
        {
        }
        internal virtual void MouseEnter(Point point)
        {
        }
        internal virtual void MouseEnter()
        {

        }
        internal virtual void MouseLeave()
        {

        }
        internal virtual void Resized(int width, int height)
        {
        }


        #region Private
        private int _canvasWidth;
        private int _canvasHeight;
        Entity _background = new Entity("Background", new Geometry(new MeshPlane()), new Material());
        Entity _gizmo = new Entity("Compass", new Geometry(new Gizmo()), new Material(), new CompassScript());
        Entity _centroid = new Entity("Centroid", new Geometry(new Gizmo()), new Material(), null);
        private Project _project;
        #endregion
    }
}