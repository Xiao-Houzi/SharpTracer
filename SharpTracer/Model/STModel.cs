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

namespace SharpTracer
{

    public partial class SharpTracerModel : NotificationBase
    {
        #region Properties
        public Project Project
        { get => _project; }
        public Renderer Renderer { get; set; }
        public Canvas Canvas { get; set; }
        public List<Entity> Geometry
        {
            get => Project.Entities; set
            {
                Project.Entities = value; NotifyPropertyChanged("Geometry");
            }
        }


        public static Entity? SelectedEntity
        { get; set; }
        public static float EntityPointsize
        { get; set; }
        public bool Isolate
        { get; internal set; }
        public bool ShowTools { get; set; }
        public static Dictionary<string, bool> Keys
        {
            get;
            internal set;
        }

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

            Messenger.UIEvent += SharpTracerMessenger_UIEvent;
            Messenger.ModelEvent += SharpTracerMessenger_ModelEvent;
            Canvas.CanvasChanged += Canvas_RenderingEvent;
            Canvas.RenderComplete += Canvas_RenderComplete;
        }

        private void Canvas_RenderComplete(object? sender, EventArgs e)
        {
            // show render has completed and save file
        }

        private void Canvas_RenderingEvent(object? sender, float e)
        {
            // update image and update progress bar
        }

        #region Commands
        internal bool CanRender()
        {
            return true;
        }
        internal void Render(object parameter)
        {
            Canvas.Render();
        }
        #endregion

        internal void ResetView()
        {
            Project.ResetViewCamera();
        }

        private void SharpTracerMessenger_ModelEvent(object sender, ModelArgs args)
        {
            switch (args.Reason)
            {
                case EventReason.GLIsNull:
                    //GLAcquiredDelegate = ((Task)args.DataObject);
                    Event.Model(this, EventReason.AcquireGL, "");
                    break;

                case EventReason.GAcquired:
                    //GLAcquiredDelegate.Start();
                    break;
            }
        }

        private void SharpTracerMessenger_UIEvent(object sender, UIArgs args)
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
                    break;

                case EventReason.CommandCloseProject:
                    break;

                case EventReason.CommandSaveProject:
                    Project.Serialise();
                    break;

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