using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using GlmSharp;
using SharpGL;
using SharpTracer.Engine.GLAbstraction;
using SharpTracer.Engine.Scene;
using SharpTracer.Engine.Scene.RenderGeometry;
using SharpTracer.Model;
using SharpTracer.Scripts;

namespace SharpTracer.Engine
{
    public class Renderer
    {
        public Dictionary<string, Tuple<string, string>> Shaders { get => _shaders; set { _shaders = value; } }
        public List<Entity> Entities
        { get => _project?.Entities; set => _project.Entities = value; }
        public Camera Camera
        { get => _camera; set => _camera = value; }
        public bool Orthographic
        { get; set; }
        public float Time
        { get; set; }
        public int CanvasWidth { get => _canvasWidth; }
        public int CanvasHeight { get => _canvasHeight; }
        public float AspectRatio
        { get => _canvasHeight / _canvasWidth; }


        public float Delta
        {
            get
            {
                return _delta;
            }
        }

        public float FPS
        {
            get; set;
        }

        public Renderer()
        {
            _frameTimes = new List<float>();
            _delta = 0;
            SetProject(new Project());
        }

        public void SetProject(Project project)
        {
            _project = project;
            _camera = project.ViewCamera;
        }

        public virtual void Initialise(OpenGL gl)
        {
            GLLayer.Initialise(this, gl);

            LoadGeometry("Gizmo", new Gizmo());
            LoadGeometry("Grid", new Gizmo());
            LoadGeometry("Camera", new Gizmo());
            LoadGeometry("Plane", new Gizmo());
            LoadGeometry("Cube", new Gizmo());
            LoadGeometry("Sphere", new Gizmo());

            LoadShader("Default");
            LoadShader("Texture");
            LoadShader("Hex");

            GLLayer.Shaders.CompileShaders(this);

            // test only remove in production
            Entity sphere = new Entity("Sphere", new MeshSphere(), new Material(), new SphereScript());
            sphere.Transform.Translation = new vec3(0f, 0f, 0f);
            AddEntity(sphere);
        }

        public virtual void Update()
        {
            GetDelta();

            foreach (Entity entity in _project.Entities)
                entity.Update(_delta);
        }
        public void Render(OpenGL gl)
        {
            GLLayer.BeginFrame(this);

            // Render entitys
            foreach (Entity entity in Entities)
            {
                if (Orthographic)
                {
                    gl.Disable(OpenGL.GL_DEPTH_TEST);
                    gl.DepthMask(0);
                }
                else
                {
                    //GL.Enable(OpenGL.GL_DEPTH_TEST);
                    gl.Clear(OpenGL.GL_DEPTH_BUFFER_BIT);
                    gl.DepthMask(0);
                    gl.ClearStencil(0);
                }
                GLLayer.Render(this, Camera, entity);
            }
            GLLayer.EndFrame();
        }
        public void Cleanup()
        {
            GLLayer.Shutdown();
        }
        private void GetDelta()
        {
            double currentTime = (double)DateTime.Now.Ticks * ticksize;

            _delta = (float)(currentTime - _time);

            _frameTimes.Add(_delta);
            if (_time == 0) _delta = 0;
            if (_delta > .1f) _delta = .1f;

            if (_frameTimes.Count > 30) _frameTimes.RemoveAt(0);
            FPS = 1 / _frameTimes.Average();

            _time = currentTime;
        }
        public void SetSize(int width, int height)
        {
            _canvasWidth = width;
            _canvasHeight = height;
            GLLayer.UpdateWindowSize(width, height);
        }

        public void LoadShader(string v)
        {
            StreamReader sr = new StreamReader(File.OpenRead(string.Format("Resources/Shaders/{0}.vert", v)));
            string vs = sr.ReadToEnd();
            sr = new StreamReader(File.OpenRead(string.Format("Resources/Shaders/{0}.frag", v)));
            string fs = sr.ReadToEnd();
            _shaders.Add(v, new System.Tuple<string, string>(vs, fs));
        }
        public void LoadGeometry(string name, Geometry geometry)
        {
            _geometry.Add(name, geometry);
        }
        internal void MouseEnter(Point point)
        {
        }
        internal void MouseLeave(Point point)
        {
        }
        internal void MouseUp(Point point)
        {
        }
        internal void MouseDown(Point point)
        {
        }
        internal void Scroll(int delta)
        {

        }
        internal void MouseMove(Point position)
        {
            Point delta = (Point)(position - _lastPosition);
            _lastPosition = position;
        }
        internal virtual void MouseMove(Point position, Point delta)
        {
        }
        internal virtual void MouseScroll(int delta)
        {
        }


        internal void Update(float delta)
        {
            foreach (Entity entity in Entities)
                entity.Update(delta);
        }

        public void AddEntity(Entity entity)
        {
            Entities?.Add(entity);
            entity.Initialise();
        }
        

        public void Clear()
        {
            Entities?.Clear();
        }

        internal virtual void Resized(int width, int height)
        {
        }

        #region Private
        private Dictionary<string, Geometry> _geometry = new Dictionary<string, Geometry>();
        private Dictionary<string, Tuple<string, string>> _shaders = new Dictionary<string, Tuple<string, string>>();
        private const float ticksize = 1 / (float)TimeSpan.TicksPerSecond;
        private double _time;
        protected float _delta;
        private List<float> _frameTimes;
        private Point _lastPosition;
        private int _canvasWidth;
        private int _canvasHeight;
        private Project _project;
        private Camera _camera;
        #endregion
    }


}
