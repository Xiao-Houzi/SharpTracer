using SharpEngine.Engine.Graphics;
using SharpTracer.View.Controls;
using GlmSharp;
using SharpGL;
using System.Collections.ObjectModel;
using SharpTracer.Base;
using SharpTracer.Engine.Scene;
using SharpTracer.Engine.GLAbstraction;
using SharpTracer.Engine;
using SharpTracer.Model.Base.Messaging;

namespace SharpTracer.ViewModels
{
    public class MainWindowVM : NotificationBase
    {
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
            (x) => { Event.UI(null, EventReason.CommandAbout, ""); },
            "About")
        };
        public Renderer Renderer
        {
            get
            {
                return _renderer;
            }
        }
        public ExpandablePanelVM ExpandablePanel
        {
            get => _expandablePanel; set
            {
                _expandablePanel = value; NotifyPropertyChanged();
            }
        }
        public ControlsVM Controls
        {
            get
            {
                return _controls;
            }
            set
            {
                _controls = value; NotifyPropertyChanged();
            }
        }
        public StatusVM Status
        {
            get => _status; set
            {
                _status = value; NotifyPropertyChanged();
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

        public ObservableCollection<Entity> Entitys
        {
            get; set;
        }
        public Command CommandCloseApp
        {
            get;
            private set;
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

        public bool AppCloseRequested
        {
            get;
            internal set;
        }

        public bool ShowGizmos { get; internal set; }

        public MainWindowVM(SharpTracerModel model)
        {
            _model = model;
            _renderer = new Renderer();
            _model.Renderer = _renderer;
            ExpandablePanel = new ExpandablePanelVM(340);

            Status = new StatusVM(model);
            Entitys = new ObservableCollection<Entity>();

            ExpandablePanel.Tabs.Add(new ControlsVM(model));
            ExpandablePanel.Tabs.Add(new GeometryList(model));
            ExpandablePanel.Tabs.Add(new EntityVM());


            CommandCloseApp = new Command(() => true,
                (x) => { AppCloseRequested = true; Event.UI(this, EventReason.CommandCloseApp, "CloseApp"); });
            

            Command CommandClear = new Command(() => true,
                (x) => { Event.UI(this, EventReason.CommandClear, ""); }, "Clear");
            Command CommandOpenProject = new Command(() => true,
                (x) => { Event.UI(this, EventReason.CommandOpenProject, ""); }, "OpenProject");
            Command CommandCloseProject = new Command(() => true,
                (x) => { Event.UI(this, EventReason.CommandCloseProject, ""); }, "CloseProject");
            Command CommandSaveProject = new Command(() => true,
                (x) => { Event.UI(this, EventReason.CommandSaveProject, ""); }, "SaveProject");
            Command CommandSettings = new Command(() => true,
                (x) => { Event.UI(this, EventReason.CommandSettings, ""); }, "Settings");
            Command CommandRender = new Command(() => true,
                (x) => { Event.UI(this, EventReason.CommandRender, ""); }, "Render");


            Ribbon.Add(new RibbonCommandVM("Project", "Clear", "", "Clear Data", CommandClear));
            Ribbon.Add(new RibbonCommandVM("Project", "Open", "", "Open a project from disk.", CommandOpenProject));
            Ribbon.Add(new RibbonCommandVM("Project", "Close", "", "Close the current project.", CommandCloseProject));
            Ribbon.Add(new RibbonCommandVM("Project", "Save", "", "Save a project to disk.", CommandSaveProject));
            Ribbon.Add(new RibbonCommandVM("General", "Settings", "", "Change settings for the project.", CommandSettings));
            Ribbon.Add(new RibbonCommandVM("Tools", "Render", "", "Render the current project as an image.", CommandRender));

            Messenger.UIEvent += SharpTracerMessenger_UIEvent;
            Messenger.ModelEvent += SharpTracerMessenger_ModelEvent;
        }

        private void SharpTracerMessenger_ModelEvent(object sender, ModelArgs args)
        {
            App.Current?.Dispatcher.Invoke(() =>
            {
                switch (args.Reason)
                {
                    case EventReason.SafeToClose:
                        if (AppCloseRequested)
                        {
                            ((App)App.Current).log.CloseLog();
                            App.Current.MainWindow?.Close();
                        }
                        break;
                    case EventReason.AcquireGL:
                        while (GL == null) { }
                        Event.Model(this, EventReason.GLAcquired, GL);
                        break;
                    case EventReason.ProjectLoaded:
                        _expandablePanel.Tabs[0].Update();
                        break;
                }
            });
        }

        private void SharpTracerMessenger_UIEvent(object sender, UIArgs args)
        {
            switch (args.Reason)
            {
                case EventReason.CommandAbout:
                    break;
                case EventReason.CommandRender:
                    break;
                case EventReason.EntitySelected:

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

        private SharpTracerModel _model;
        private ExpandablePanelVM _expandablePanel;
        private ControlsVM _controls;
        private StatusVM _status;
        private Renderer _renderer;
    }
}
