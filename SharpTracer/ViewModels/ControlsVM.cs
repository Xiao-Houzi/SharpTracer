using SharpEngine.Maths;
using SharpTracer;
using GlmSharp;
using SharpTracer.ViewModels;
using System;
using System.ComponentModel;
using SharpTracer.Base;

namespace SharpTracer
{
	public class ControlsVM : NotificationBase
	{
		public Command ShowGizmos
		{
			get
			{
				return _showGizmos;
			}
			set
			{
				_showGizmos = value; ; NotifyPropertyChanged();
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

		public MainWindowVM VM
		{
			get; 
			set;
		}


		public ControlsVM(SharpTracerModel model)
		{
			_model = model;

			cloudPanel = new CameraControlVM(model);

			_showGizmos = new Command(CanShowGizmos, ExecuteShowGizmos);
		}

	

		private void ExecuteShowGizmos(object Parameter)
		{
			VM.ShowGizmos = !VM.ShowGizmos;
		}

		private bool CanShowGizmos()
		{
			return true;
		}

		private CameraControlVM cloudPanel;
		private Command _showGizmos;
		private SharpTracerModel _model;
	}
}
