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
		private AdjustmentControlVM adjustmentControls;
		private CameraControlVM cloudPanel;
		private Command _showGizmos;

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

		public AdjustmentControlVM AdjustmentControls
		{
			get
			{
				return adjustmentControls;
			}
			set
			{
				adjustmentControls = value;
				NotifyPropertyChanged();
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


		public ControlsVM(MainWindowVM vm)
		{
			VM = vm;

			adjustmentControls = new AdjustmentControlVM(vm.Adjustments);
			cloudPanel = new CameraControlVM(vm);

			_showGizmos = new Command(CanShowGizmos, ExecuteShowGizmos);
		}

	

		private void ExecuteShowGizmos(object Parameter)
		{
			VM.IsolateGizmos = !VM.IsolateGizmos;
		}

		private bool CanShowGizmos()
		{
			return true;
		}
	}
}
