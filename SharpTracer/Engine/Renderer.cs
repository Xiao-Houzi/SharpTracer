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
using SharpTracer.Model.Base.Messaging;
using SharpTracer.Scripts;

namespace SharpTracer.Engine
{
    public class Renderer
    {
        public Dictionary<string, Tuple<string, string>> Shaders { get => _shaders; set { _shaders = value; } }
        public Dictionary<string, uint> Textures { get => _textures; set {  _textures=value; } }
        public List<Entity> Entities
        { get => _project.Entities; set => _project.Entities = value; }
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
        public bool RenderingCanvas { get;  set; }
        public bool CanvasComplete { get; set; }


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
            LoadShader("Canvas");

            AddTexture("Canvas");

            GLLayer.Shaders.CompileShaders(this);

            _canvas = new Entity("Canvas", new MeshPlane(), new Material() { Shader="Canvas"} );
            _canvas.Material.AddTexture(Textures["Canvas"]);
            _canvas.Transform.Scale = new vec3(1.0f, 1.0f, 0);

            RaiseEvent.Model(this, EventReason.RendererInitialised, null);
        }

        private void AddTexture(string name)
        {
            Textures.Add(name, GLLayer.GenerateTexture());
        }

        public virtual void Update()
        {
            GetDelta();

            foreach (Entity entity in _project.Entities)
                entity.Update(_delta);

            if (RenderingCanvas)
            {

            }
        }
        public void Render(OpenGL gl)
        {
            GLLayer.BeginFrame(this);

            // Render entitys
            foreach (Entity entity in Entities)
            {
                GLLayer.RenderSolid();
                GLLayer.Render(this, Camera, entity);
                GLLayer.RenderWireframe();
                GLLayer.Render(this, Camera, entity);
            }

            //render tools

            //render canvas
            if(RenderingCanvas || CanvasComplete)
            {
                GLLayer.RenderSolid();
                GLLayer.Render(this, Camera, _canvas);
            }

            GLLayer.EndFrame();
        }

        internal void UpdateCanvasImage(byte[] bytes)
        {
            uint textureSlot = _canvas.Material.Texture[0];
            GLLayer.UploadTexture(CanvasWidth, CanvasHeight, bytes, textureSlot);
        }

        public void Cleanup()
        {
            GLLayer.Shutdown();
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

        public void SetViewSize(int width, int height)
        {
            GLLayer.UpdateWindowSize(width, height);
            _viewWidth = width;
            _viewHeight = height;
            RecalculateCanvasDimensions();
        }
        public void SetCanvasSize(int width, int height)
        {
            _canvasWidth = width;
            _canvasHeight = height;
            RecalculateCanvasDimensions();
        }

        #region Private
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
        private void RecalculateCanvasDimensions()
        {
            // keep aspect ratio canvas within view region
            float viewAR = (float)_viewWidth / _viewHeight;
            float canvasAR = (float)_canvasWidth / _canvasHeight;
            float AR;
            if (viewAR > canvasAR)
            {
                AR = canvasAR / viewAR;
                _canvas.Transform.Scale = new vec3(AR, 1.0f, 0);
            }
            else
            {
                AR = viewAR / canvasAR;
                _canvas.Transform.Scale = new vec3(1.0f, AR, 0);
            }
            _canvas.Transform.Scale *= 0.95f;
        }

        private Dictionary<string, Geometry> _geometry = new Dictionary<string, Geometry>();
        private Dictionary<string, Tuple<string, string>> _shaders = new Dictionary<string, Tuple<string, string>>();
        private Dictionary<string, uint> _textures = new Dictionary<string, uint>();
        private Entity _canvas;
        private const float ticksize = 1 / (float)TimeSpan.TicksPerSecond;
        private double _time;
        protected float _delta;
        private List<float> _frameTimes;
        private Point _lastPosition;
        private int _canvasWidth=1, _canvasHeight=1, _viewWidth = 1, _viewHeight = 1;
        private Project _project;
        private Camera _camera;
        #endregion
    }


}
