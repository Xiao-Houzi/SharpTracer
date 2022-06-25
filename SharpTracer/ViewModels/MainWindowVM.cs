using SharpEngine.Engine.Graphics;
using SharpTracer.View.Controls;
using SharpTracer.Model.Events;
using GlmSharp;
using SharpGL;
using System.Collections.ObjectModel;
using SharpTracer.Base;
using SharpTracer.Scripting;
using SharpTracer.Engine.Scene;
using SharpTracer.Engine.Graphics;

namespace SharpTracer.ViewModels
{
    public class MainWindowVM : NotificationBase
	{
		public SharpTracerModel Model;
		private ExpandablePanelVM expandablePanel;
		private ControlsVM controls;
		private StatusVM status;

		public OpenGL GL
		{
			get
			{
				return GLLayer.GL;
			}
		}
		public RibbonVM Ribbon
		{
			get; set;
		} = new RibbonVM();
		private HeaderBar Header
		{
			get; set;
		} = new HeaderBar()
		{
			Title = "SharpTracer - Geometry Viewer",
			CommandAbout = new Command(
			() => true,
			( x ) => { SharpTracerEvent.UI( null, SharpTracerUIArgs.EventReason.CommandAbout, "" ); },
			"About" )
		};
		public ViewRenderer Renderer
		{
			get
			{
				return Model.Renderer;
			}
		}
		public ExpandablePanelVM ExpandablePanel
		{
			get => expandablePanel; set
			{
				expandablePanel = value; NotifyPropertyChanged();
			}
		}
		public ControlsVM Controls
		{
			get
			{
				return controls;
			}
			set
			{
				controls = value; NotifyPropertyChanged();
			}
		}
		public StatusVM Status
		{
			get => status; set
			{
				status = value; NotifyPropertyChanged();
			}
		}

		public ObservableCollection<Entity> Entitys
		{
			get; set;
		}
		public Command CommandCloseApp
		{
			get;
			private set;
		}

		public bool IsolateGizmos
		{
			get
			{
				return Model.Isolate;
			}
			set
			{
				Model.Isolate = value;
				NotifyPropertyChanged();
			}
		}

		public bool SingleEntity
		{
			get
			{
				return Model.Single;
			}
			set
			{
				Model.Single = value;
				NotifyPropertyChanged();
			}
		}

		public Entity SelectedEntity
		{
			get
			{
				return SharpTracerModel.SelectedEntity;
			}
			set
			{
				SharpTracerModel.SelectedEntity = value;		
				NotifyPropertyChanged();
			}
		}

		public float Pointsize
		{
			get
			{
				return SharpTracerModel.EntityPointsize;
			}
			set
			{
				SharpTracerModel.EntityPointsize = value;
				NotifyPropertyChanged();
			}
		}

		public float Confidence
		{
			get
			{
				return SharpTracerModel.Confidence;
			}
			set
			{
				SharpTracerModel.Confidence = value;
				NotifyPropertyChanged();
			}
		}

		public bool AppCloseRequested
		{
			get;
			internal set;
		}
		public Adjustments Adjustments
		{
			get
			{
				return Model.Adjust;
			}
			set
			{
				Model.Adjust = value;
				NotifyPropertyChanged();
			}
		}

		public mat3 DepthAdj
		{
			get
			{
				return Model.Adjust.DepthAdjustment;
			}
			set
			{
				Model.Adjust.DepthAdjustment = value;
			}
		}

		public int Near
		{
			get
			{
				return SharpTracerModel.Near;
			}
			set
			{
				SharpTracerModel.Near = value;
				NotifyPropertyChanged();
			}
		}
		public int Far
		{
			get
			{
				return SharpTracerModel.Far;
			}
			set
			{
				SharpTracerModel.Far = value;
				NotifyPropertyChanged();
			}
		}

		public MainWindowVM( SharpTracerModel model )
		{
			Model = model;
			Renderer.SetVM(this);

			Controls = new ControlsVM(this);
			Status = new StatusVM( this );
			Entitys = new ObservableCollection<Entity>();

			ExpandablePanel = new ExpandablePanelVM(340);

			CommandCloseApp = new Command( () => true,
				( x ) =>
				{
					AppCloseRequested = true;
					SharpTracerEvent.UI( this, SharpTracerUIArgs.EventReason.CommandCloseApp, "CloseApp" );
				} );

			Command CommandClear = new Command( () => true,
				( x ) => { SharpTracerEvent.UI( this, SharpTracerUIArgs.EventReason.CommandClear, "" ); }, "Clear" );
			Command CommandOpenProjectJson = new Command(() => true,
				( x ) => { OpenProjectJson(); }, "OpenProjectJson");
			Command CommandCloseProject = new Command( () => true,
				( x ) => { SharpTracerEvent.UI( this, SharpTracerUIArgs.EventReason.CommandCloseProject, "" ); }, "CloseProject" );
			Command CommandExport = new Command( () => true,
				( x ) => { SharpTracerEvent.UI( this, SharpTracerUIArgs.EventReason.CommandExportProject, "" ); }, "ExportProject" );
			Command CommandClean = new Command( () => true,
				( x ) => { SharpTracerEvent.UI( this, SharpTracerUIArgs.EventReason.CommandCleanupProject, "" ); }, "CleanupProject" );
			Command CommandSettings = new Command( () => true,
				( x ) => { SharpTracerEvent.UI( this, SharpTracerUIArgs.EventReason.CommandSettings, "" ); }, "Settings" );
			Command CommandRender = new Command(() => true,
				( x ) => { SharpTracerEvent.UI(this, SharpTracerUIArgs.EventReason.CommandRender, ""); }, "Render");


			Ribbon.Add( new RibbonCommandVM( "Project", "Clear", "", "Clear Data", CommandClear ) );
			Ribbon.Add(new RibbonCommandVM( "Project", "Open JSON", "", "Open a project from disk.", CommandOpenProjectJson));
			Ribbon.Add( new RibbonCommandVM( "Project", "Save", "", "Select an export method to output clouds or models.", CommandExport ) );
			Ribbon.Add( new RibbonCommandVM( "Project", "Cleanup", "", "Remove noise and correct alignment.", CommandClean ) );
			Ribbon.Add( new RibbonCommandVM( "General", "Settings", "", "Change settings for the project.", CommandSettings ) );
			Ribbon.Add(new RibbonCommandVM("Tools", "Render", "", "Calcuklate the product of two matrices.", CommandRender));

			SharpMessenger.UIEvent += SharpTracerMessenger_UIEvent;
			SharpMessenger.ModelEvent += SharpTracerMessenger_ModelEvent;
		}

		private void SharpTracerMessenger_ModelEvent( object sender, SharpTracerModelArgs args )
		{
			App.Current?.Dispatcher.Invoke( () =>
			{
				switch (args.Reason)
				{
					case SharpTracerModelArgs.EventReason.SafeToClose:
						if (AppCloseRequested)
						{
							( (App)App.Current ).log.CloseLog();
							App.Current.MainWindow?.Close();
						}
						break;
					case SharpTracerModelArgs.EventReason.AcquireGL:
						while(GL == null) { }
						SharpTracerEvent.Model(this, SharpTracerModelArgs.EventReason.GAcquired, GL);
						break;
					case SharpTracerModelArgs.EventReason.ProjectLoaded:
						expandablePanel.Tabs[0].Update();
						break;
				}
			} );
		}

		public void UpdateControls()
		{
			Controls.GeometryControl.Update();
		}

		private void SharpTracerMessenger_UIEvent( object sender, SharpTracerUIArgs args )
		{
			switch(args.Reason)
			{
				case SharpTracerUIArgs.EventReason.CommandAbout:
					break;
				case SharpTracerUIArgs.EventReason.CommandRender:
					break;
				case SharpTracerUIArgs.EventReason.EntitySelected:
					
					break;
			}
		}

		private void OpenProjectJson()
		{
			// use a dialog to open a project (cloud folder)
			//FolderBrowserDialog dlg = new FolderBrowserDialog()
			//{
			//	ShowNewFolderButton = false,
			//	RootFolder = Environment.SpecialFolder.Desktop
			//};

			//bool result = dlg.ShowDialog() == DialogResult.OK;

			//if(result)
			//	SharpTracerEvent.UI(this, SharpTracerUIArgs.EventReason.CommandOpenProjectPTX, new Uri(dlg.SelectedPath));
			//else
			//	SharpTracerEvent.UI(this, SharpTracerUIArgs.EventReason.NoSelectedProjectFile, "");
		}
	}
}
