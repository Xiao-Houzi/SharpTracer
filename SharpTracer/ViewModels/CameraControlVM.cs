using SharpTracer;
using SharpTracer.Base;
using SharpTracer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTracer
{
	public class CameraControlVM : NotificationBase
	{
		public String Axis
		{
			get { return _axis; }
			set { _axis = value; NotifyPropertyChanged(); }
		}

		public float Zoom
		{
			get
			{
				return _model.Renderer.ViewCamera.Zoom;
			}
			set
			{
				_model.Renderer.ViewCamera.Zoom = value;
				NotifyPropertyChanged();
			}
		}

		public float FOV
		{
			get
			{
				return _model.Renderer.ViewCamera.FOV;
			}
			set
			{
				_model.Renderer.ViewCamera.FOV = value;
				NotifyPropertyChanged();
			}
		}

		public Command ShowGizmos
		{
			get;
			set;
		}
		public Command ShowSingle
		{
			get;
			set;
		}
		public Command GeometryFreq
		{
			get;set;
		}
		public Command ResetView
		{get;set;
		}

		public CameraControlVM(SharpTracerModel model)
		{
			_model = model;
			ResetView = new Command(() => true, (x) => _model.Project.ResetViewCamera(), "ResetView");
		}

		private SharpTracerModel _model;
		private string _axis;
	}
}
