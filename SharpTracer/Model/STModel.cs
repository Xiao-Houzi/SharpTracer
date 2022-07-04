using System;
using System.ComponentModel;
using SharpTracer.Model.Events;
using SharpGL;
using System.Threading.Tasks;
using GlmSharp;
using System.Collections.Generic;
using System.Windows;
using SharpTracer.Base;
using SharpTracer.Model;
using SharpTracer.View.Controls;
using SharpTracer.Engine.Scene;

namespace SharpTracer
{

    public partial class SharpTracerModel : NotificationBase
    {
        #region Properties
        public Project Project
        { get => _project; }
        public List<Entity> Geometry
        {
            get => Project.Entities; set
            {
                Project.Entities = value; NotifyPropertyChanged("Geometry");
            }
        }
        public OpenGL GL
        { get; internal set; }
        private Task GLAcquiredDelegate;
        public static Entity? SelectedEntity
        { get; set; }
        public static float EntityPointsize
        { get; set; }
        public bool Isolate
        { get; internal set; }
        public static Dictionary<string, bool> Keys
        {
            get;
            internal set;
        }

        #endregion

        public SharpTracerModel()
        {
            _project = new Project();
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

            SharpMessenger.UIEvent += SharpTracerMessenger_UIEvent;
            SharpMessenger.ModelEvent += SharpTracerMessenger_ModelEvent;
        }

        internal void ResetView()
        {
            Project.ResetViewCamera();
        }

        private void SharpTracerMessenger_ModelEvent(object sender, SharpTracerModelArgs args)
        {
            switch (args.Reason)
            {
                case SharpTracerModelArgs.EventReason.GLIsNull:
                    GLAcquiredDelegate = ((Task)args.DataObject);
                    SharpTracerEvent.Model(this, SharpTracerModelArgs.EventReason.AcquireGL, "");
                    break;

                case SharpTracerModelArgs.EventReason.GAcquired:
                    GL = (OpenGL)args.DataObject;
                    GLAcquiredDelegate.Start();
                    break;
            }
        }

        private void SharpTracerMessenger_UIEvent(object sender, SharpTracerUIArgs args)
        {
            switch (args.Reason)
            {
                case SharpTracerUIArgs.EventReason.CommandCloseApp:
                    SharpTracerEvent.Model(this, SharpTracerModelArgs.EventReason.SafeToClose, "");
                    break;

                case SharpTracerUIArgs.EventReason.CommandClear:
                    ClearProject();
                    break;

                case SharpTracerUIArgs.EventReason.CommandSettings:
                    break;

                case SharpTracerUIArgs.EventReason.CommandOpenProjectBinary:
                    OpenProject((Uri)args.DataObject);
                    break;

                case SharpTracerUIArgs.EventReason.CommandOpenProjectPTX:
                    OpenProject((Uri)args.DataObject, false);
                    break;

                case SharpTracerUIArgs.EventReason.CommandCloseProject:
                    break;

                case SharpTracerUIArgs.EventReason.CommandExportProject:
                    Project.ExportData();
                    break;

                case SharpTracerUIArgs.EventReason.AdjustmentMatrixChanged:
                    break;

                case SharpTracerUIArgs.EventReason.AdjustmentAdjustChanged:
                    break;
                case SharpTracerUIArgs.EventReason.CommandCleanupProject:
                    FilterGeometrys();
                    break;
            }
        }

        private void FilterGeometrys()
        {

        }

        private void ProcessingComplete()
        {
            MessageBox.Show("Processing Complete");
        }

        private void ClearProject()
        {
            Project.Entities.Clear();
        }

        private void OpenProject(Uri path, bool binary = true)
        {
            this.path = path;

            if (GL == null)
            {
                if (binary)
                    SharpTracerEvent.Model(this, SharpTracerModelArgs.EventReason.GLIsNull, new Task(() => { SharpTracerEvent.UI(this, SharpTracerUIArgs.EventReason.CommandOpenProjectBinary, path); }));
                else
                    SharpTracerEvent.Model(this, SharpTracerModelArgs.EventReason.GLIsNull, new Task(() => { SharpTracerEvent.UI(this, SharpTracerUIArgs.EventReason.CommandOpenProjectPTX, path); }));
                return;
            }
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