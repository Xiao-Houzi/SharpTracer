using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using GlmSharp;
using SharpGL;
using SharpTracer.Engine.Graphics;
using SharpTracer.Engine.Scene;
using SharpTracer.Model;
using SharpTracer.Scripts;
using SharpTracer.Engine.Scene.RenderGeometry;
using Geometry = SharpTracer.Engine.Scene.RenderGeometry.Geometry;

namespace SharpTracer.Engine
{
    public class Renderer
    {
        public Dictionary<string, Tuple<string, string>> shaders
        {
            get; set;
        }

        public List<Entity> Entities
        { get => _project.Entities; set => _project.Entities = value; }
        public Camera ViewCamera
        { get => _project.ViewCamera; set => _project.ViewCamera = value; }
        public Camera SceneCamera
        { get => _project.SceneCamera; set => _project.SceneCamera = value; }
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
            shaders = new Dictionary<string, Tuple<string, string>>();
            LoadShader("Default");
            LoadShader("Texture");
            LoadShader("Hex");

            _frameTimes = new List<float>();
            _delta = 0;

        }

        public void SetProject(Project project)
        {
            _project = project;
        }

        public virtual void Initialise(OpenGL gl)
        {
            GLLayer.Initialise(this, gl);
            Application.Current.Dispatcher.Invoke(
                () =>
                Initialise()
                );
        }

        public virtual void Update()
        {
            GetDelta();
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
                GLLayer.Render(this, ViewCamera, entity);
            }
            GLLayer.EndFrame();
        }


        public void Cleanup()
        {

        }

        private void GetDelta()
        {
            double currentTime = (double)DateTime.Now.Ticks * ticksize;

            _delta = (float)(currentTime - time);

            _frameTimes.Add(_delta);
            if (time == 0) _delta = 0;
            if (_delta > .1f) _delta = .1f;

            if (_frameTimes.Count > 30) _frameTimes.RemoveAt(0);
            FPS = 1 / _frameTimes.Average();

            time = currentTime;
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
            shaders.Add(v, new System.Tuple<string, string>(vs, fs));
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

        public void Initialise()
        {
            Entity sphere = new Entity("Sphere", new Geometry(new MeshSphere()), new Material(), new SphereScript());
            sphere.Transform.Translation = new vec3(0f, 0f, 0f);
            AddEntity(sphere);
        }

        internal void Update(float delta)
        {
            foreach (Entity entity in Entities)
                entity.Run(delta);
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

        internal virtual void Resized(int width, int height)
        {
        }

        #region Private

        private const float ticksize = 1 / (float)TimeSpan.TicksPerSecond;
        private double time;
        protected float _delta;
        private List<float> _frameTimes;
        private Point _lastPosition;
        private int _canvasWidth;
        private int _canvasHeight;
        private Project _project;
        #endregion
    }


}
