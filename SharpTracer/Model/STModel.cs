using System;
using System.ComponentModel;
using SharpGL;
using System.Threading.Tasks;
using GlmSharp;
using System.Collections.Generic;
using System.Windows;
using SharpTracer.Base;
using SharpTracer.Model;
using SharpTracer.View.Controls;
using SharpTracer.Engine.Scene;
using SharpTracer.Engine;
using SharpTracer.Engine.RayTracing;
using SharpTracer.Model.Base.Messaging;
using SharpTracer.Engine.GLAbstraction;
using System.Reflection;
using System.Linq;

namespace SharpTracer
{

    public partial class SharpTracerModel : NotificationBase
    {
        #region Properties
        public Project Project
        { get => _project; }
        public Renderer Renderer
        { get; set; }
        public Canvas Canvas
        { get; set; }
        public List<Entity> Entities
        {
            get => Project.Entities; set
            {
                Project.Entities = value; NotifyPropertyChanged("Geometry");
            }
        }

        public static Dictionary<string, bool> Keys
        {
            get;
            internal set;
        }


        public static Entity? SelectedEntity
        { get; set; }
        public static float EntityPointsize
        { get; set; }
        public bool Isolate
        { get; internal set; }
        public bool ShowTools { get; set; }
        public bool IsRendering { get; private set; }
        public bool RenderComplete { get; private set; }
        public float RenderPercent { get; private set; }

        #endregion

        public SharpTracerModel()
        {
            _project = new Project();
            Canvas = new Canvas();
            Canvas.Initialise(_project);
            Keys = new Dictionary<string, bool>()
            {
                {"Forward", false },
                {"Backward", false },
                {"LeftTilt", false },
                {"RightTilt", false },
                {"LeftStrafe", false },
                {"RightStrafe", false },

                {"", false },
            };

            _project.CurrentEntity = new Entity();
            _cloudViewControls = new GeometryViewControls();

            Messenger.UIEvent +=HandleUIEvent;
            Messenger.ModelEvent += HandleModelEvent;
        }

        #region Commands
        internal bool CanRender()
        {
            return _project.Entities.Count > 0;
        }

        #endregion

        private void HandleModelEvent(object sender, ModelArgs args)
        {
            switch (args.Reason)
            {
                case EventReason.RendererInitialised:
                    _project.Load();
                    break;
                case EventReason.GLIsNull:
                    //GLAcquiredDelegate = ((Task)args.DataObject);
                    Event.Model(this, EventReason.AcquireGL, "");
                    break;

                case EventReason.GLAcquired:

                    break;

                case EventReason.RenderStarted:
                    IsRendering = true;
                    Renderer.RenderingCanvas = true;
                    break;

                case EventReason.RenderUpdated:
                    RenderPercent = (float)args.DataObject;
                    Renderer.UpdateCanvasImage(Canvas.GetData());
                    break;

                case EventReason.RenderEnded:
                    IsRendering = false;
                    RenderComplete = true;
                    Renderer.RenderingCanvas = false;
                    Renderer.CanvasComplete = true;
                    break;
            }
        }

        private void HandleUIEvent(object sender, UIArgs args)
        {
            switch (args.Reason)
            {
                case EventReason.CommandCloseApp:
                    
                    break;

                case EventReason.CommandClear:
                    ClearProject();
                    break;

                case EventReason.CommandSettings:
                    break;

                case EventReason.CommandOpenProject:
                    OpenProject((Uri)args.DataObject);
                    RaiseEvent.Model(this, EventReason.LoadProject, null);
                    break;

                case EventReason.CommandCloseProject:
                    break;

                case EventReason.CommandSaveProject:
                    // check nothing is preventing saving
                    RaiseEvent.Model(this, EventReason.SaveProject, null);
                    break;

                case EventReason.CommandRender:
                    Renderer.SetCanvasSize(Project.FrameWidth, Project.FrameHeight);
                    Canvas.Render();
                    break;

                case EventReason.CommandAddEntity:
                    Geometry geometry = (from Geometry g in GLLayer.Geometry where  g.GetType() == args.DataObject  select g).ToList().FirstOrDefault();
                    RaiseEvent.Model(this, EventReason.AddedEntity, geometry);
                    break;
            }
        }

        private void ClearProject()
        {
            Project.Entities.Clear();
        }

        private void OpenProject(Uri path, bool binary = true)
        {
            this.path = path;
            
        }

        private void ProgressCB(int current, int total)
        {

        }

        private void CompleteCB()
        {

        }

        #region Fields
        private Project _project;
        private GeometryViewControls _cloudViewControls;
        private Uri path;
        private System.Threading.Mutex loadMutex = new System.Threading.Mutex();
        #endregion
    }
}