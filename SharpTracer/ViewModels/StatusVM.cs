using SharpTracer;
using GlmSharp;
using System;
using System.Timers;
using SharpTracer.Base;

namespace SharpTracer.ViewModels
{
	public class StatusVM : NotificationBase
	{
		

		public vec3 CameraRot
		{
			get
			{
				vec3 ea = _model.Renderer.Camera.Position;
				
				return ea;
			}
		}

		public vec3 CameraLoc
		{
			get
			{
				return _model.Renderer.Camera.Position;
			}
		}

		public StatusVM(SharpTracerModel model)
		{
			_model = model;
		}

		void OnTimedEvent(Object source, ElapsedEventArgs e)
		{
			NotifyPropertyChanged("CameraRot");
			NotifyPropertyChanged("CameraLoc");
		}

		private SharpTracerModel _model;
	}
}
