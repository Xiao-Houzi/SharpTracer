using SharpEngine.Maths;
using SharpTracer;
using GlmSharp;
using SharpTracer.ViewModels;
using System;
using System.ComponentModel;
using SharpTracer.Base;
using SharpTracer.View.Controls;

namespace SharpTracer
{
    public class ControlsVM : NotificationBase, ITabPage
    {
        public string Visibility { get; set; }
        public string Title => "Controls";
        public string Icon { get; set; }
        public RibbonVM Ribbon { get; set; }
        public ExpandablePanelVM Parent { get; set; }
        public object Content => this;

        public Command ShowGizmos
        {
            get
            {
                return _showGizmos;
            }
            set
            {
                _showGizmos = value; NotifyPropertyChanged();
            }
        }

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

        public ControlsVM(SharpTracerModel model)
        {
            _model = model;

            cloudPanel = new CameraControlVM(model);

            _showGizmos = new Command(CanShowGizmos, ExecuteShowGizmos);
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
    }
}
