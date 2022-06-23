using System;
using System.ComponentModel;
using SharpTracer.Model.Infrastructure;
using SharpGL;
using System.Threading.Tasks;
using GlmSharp;
using System.Collections.Generic;
using System.Windows;
using SharpTracer.Base;
using SharpEngine.Engine.Graphics;
using SharpTracer.Model;
using SharpTracer.RendererScripting.States;
using SharpTracer.RendererScripting;
using SharpTracer.View.Controls;

namespace SharpTracer
{

	public partial class SharpTracerModel : NotificationBase
	{
		#region Properties
		public Project Project
		{
			get
			{
				return _project;
			}
		}
		public List<Entity> Geometry
        {
            get => Project.Entities;
            set
            {
                Project.Entities = value; NotifyPropertyChanged("Geometry");
            }
        }
        public Adjustments Adjust
		{
			get { return adjust; }
			set
			{
				adjust = value;
				NotifyPropertyChanged();
			}
		}
		public OpenGL GL
		{
			get;
			internal set;
		}
		private Task GLAcquiredDelegate;


		public ViewRenderer Renderer
		{
			get; internal set;
		}
		public static Entity SelectedEntity
		{
			get;
			set;
		}
		public static float EntityPointsize
		{
			get;
			set;
		}
		public bool Isolate
		{
			get;
			internal set;
		}
		public bool Single
		{
			get; internal set;
		}

		public int GeometryFreq
		{
			get;
			internal set;
		}

		public static float Speed
		{
			get;
			internal set;
		}
		public static Dictionary<string, bool> Keys
		{
			get;
			internal set;
		}
		public static float Confidence
		{
			get;
			internal set;
		}
		public static int Near
		{
			get;
			internal set;
		}
		public static int Far
		{
			get;
			internal set;
		}
		#endregion


		public SharpTracerModel(string logName)
		{
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
			EntityPointsize = 1.0f;
			Speed = 2;
			GeometryFreq = 1;
			Renderer = new ViewRenderer(this);
			Renderer.AddState(new ViewState());
			Renderer.SetCurrentState(typeof(ViewState));
			_project = new Project();
			_project.CurrentEntity = new Entity();

			_cloudViewControls = new GeometryViewControls();

			Adjust.PropertyChanged += AdjustChanged;
			SharpMessenger.UIEvent += SharpTracerMessenger_UIEvent;
			SharpMessenger.ModelEvent += SharpTracerMessenger_ModelEvent;

			Near = 100;
			Far = 8000;
		}

		private void AdjustChanged(object sender, PropertyChangedEventArgs e)
		{
			NotifyPropertyChanged(nameof(Adjust));
		}

		internal void ResetView()
		{
			Renderer.CurrentState.Camera.Reset();
			Renderer.CurrentState.Layers["Foreground"].Entities[0].Transform.Orientation = new quat(0, 0, 0, 1);
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
					Adjust.Matrix = (mat4)args.DataObject;
					ViewState.CurrentMatrix = Adjust.Matrix;
					break;

				case SharpTracerUIArgs.EventReason.AdjustmentAdjustChanged:
					Adjust.DepthAdjustment = (mat3)args.DataObject;
					mat4 temp = new mat4();
					temp[0, 0] = Adjust.DepthAdjustment[0, 0];
					temp[0, 1] = Adjust.DepthAdjustment[0, 1];
					temp[0, 2] = Adjust.DepthAdjustment[0, 2];
					temp[1, 0] = Adjust.DepthAdjustment[1, 0];
					temp[1, 1] = Adjust.DepthAdjustment[1, 1];
					temp[1, 2] = Adjust.DepthAdjustment[1, 2];
					temp[2, 0] = Adjust.DepthAdjustment[2, 0];
					temp[2, 1] = Adjust.DepthAdjustment[2, 1];
					temp[2, 2] = Adjust.DepthAdjustment[2, 2];
					ViewState.CurrentMatrix = temp;
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
			if (Renderer.CurrentState is ProjectState)
			{
				Renderer.CurrentState.Layers["PTX"].Clear();
				Renderer.CurrentState.Layers["Bin"].Clear();
				Project.Entities.Clear();
				Renderer.SetCurrentState(typeof(ViewState));
			}
		}

		private void OpenProject(Uri path, bool binary = true)
		{
			this.path = path;
			Renderer.SetCurrentState(typeof(ViewState));
			if (GL == null)
			{
				if (binary)
					SharpTracerEvent.Model(this, SharpTracerModelArgs.EventReason.GLIsNull, new Task(() => { SharpTracerEvent.UI(this, SharpTracerUIArgs.EventReason.CommandOpenProjectBinary, path); }));
				else
					SharpTracerEvent.Model(this, SharpTracerModelArgs.EventReason.GLIsNull, new Task(() => { SharpTracerEvent.UI(this, SharpTracerUIArgs.EventReason.CommandOpenProjectPTX, path); }));
				return;
			}

			if (!Renderer.HasState(typeof(ProjectState)))
			{
				Renderer.AddState(Project.ProjectState);
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
		private Adjustments adjust = new Adjustments();
		#endregion
	}
}