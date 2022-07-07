using SharpEngine.Maths;
using SharpTracer;
using GlmSharp;
using SharpTracer.ViewModels;
using System;
using System.ComponentModel;
using SharpTracer.Base;
using SharpTracer.View.Controls;
using SharpTracer.Model.Base.Messaging;

namespace SharpTracer.ViewModels
{
    public class ControlsVM : NotificationBase, ITabPage
    {
        public string Visibility { get; set; }
        public float RenderPercentage
        {
            get => _renderPercentage;
            set
            {
                RenderPercentage = value;
                NotifyPropertyChanged();
            }
        }
        public string Title => "Controls";
        public string Icon { get; set; }
        public RibbonVM Ribbon { get; set; }
        public ExpandablePanelVM Parent { get; set; }
        public object Content => this;
        internal CameraControlVM GeometryControl
        {
            get
            {
                return cloudPanel;
            }
            set
            {
                cloudPanel = value;
                NotifyPropertyChanged();
            }
        }

        public Command CommandShowGizmos
        {
            get;
            set;
        }

        public ControlsVM(SharpTracerModel model)
        {
            _model = model;

            cloudPanel = new CameraControlVM(model);

            CommandShowGizmos = new Command(CanShowGizmos, ExecuteShowGizmos);

            Ribbon = new RibbonVM();
            Icon = "";
            Ribbon.Groups.Add(new RibbonGroupVM() { Title = "Rendering" });
            Ribbon.Add(new RibbonCommandVM("Rendering", "Render", "X", "Begins rendering a ray traced image.", new Command(model.CanRender, (x) => RaiseEvent.UI(this, EventReason.CommandRender, x))));
        }

        private void ExecuteShowGizmos(object Parameter)
        {
            _model.ShowTools = !_model.ShowTools;
        }

        private bool CanShowGizmos()
        {
            return true;
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        private CameraControlVM cloudPanel;
        private Command _showGizmos;
        private SharpTracerModel _model;
        private float _renderPercentage;
    }
}
