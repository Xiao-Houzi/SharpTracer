using SharpTracer;
using GlmSharp;
using System;
using System.Timers;
using SharpTracer.Base;

namespace SharpTracer.ViewModels
{
	public class StatusVM : NotificationBase
	{
		MainWindowVM _vm;
		private Timer aTimer;

		public vec3 CameraRot
		{
			get
			{
				vec3 ea = new vec3(
					_vm.Renderer.CurrentState.Camera.Direction * (float)(180 / Math.PI),
					_vm.Renderer.CurrentState.Camera.Incline * (float)(180 / Math.PI),
					_vm.Renderer.CurrentState.Camera.Tilt * (float)(180 / Math.PI));
				return ea;
			}
		}

		public vec3 CameraLoc
		{
			get
			{
				return _vm.Renderer.CurrentState.Camera.Position;
			}
		}

		public float FPS
		{
			get
			{
				return _vm.Renderer.FPS;
			}
		}

		public float DX
		{
			get
			{
				return (float)_vm.Renderer.MouseMove.X;
			}
		}

		public float DY
		{
			get
			{
				return (float)_vm.Renderer.MouseMove.Y;
			}
		}

		public StatusVM(MainWindowVM vm)
		{
			_vm = vm;

			aTimer = new System.Timers.Timer(333);
			// Hook up the Elapsed event for the timer. 
			aTimer.Elapsed += OnTimedEvent;
			aTimer.AutoReset = true;
			aTimer.Enabled = true;
		}

		void OnTimedEvent(Object source, ElapsedEventArgs e)
		{
			NotifyPropertyChanged("CameraRot");
			NotifyPropertyChanged("CameraLoc");
			NotifyPropertyChanged("FPS");
			NotifyPropertyChanged("DX"); NotifyPropertyChanged("DY");
		}

	}
}
