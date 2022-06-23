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
		private string _axis;

		public MainWindowVM VM
		{
			get;
		}

		public String Axis
		{
			get { return _axis; }
			set { _axis = value; NotifyPropertyChanged(); }
		}

		public float Zoom
		{
			get
			{
				return VM.Model.Renderer.CurrentState.Camera.Zoom;
			}
			set
			{
				VM.Model.Renderer.CurrentState.Camera.Zoom = value;
				NotifyPropertyChanged();
			}
		}

		public float FOV
		{
			get
			{
				return VM.Model.Renderer.CurrentState.Camera.FOV;
			}
			set
			{
				VM.Model.Renderer.CurrentState.Camera.FOV = value;
				NotifyPropertyChanged();
			}
		}

		public float Pointsize
		{
			get
			{
				return VM.Pointsize;
			}
			set
			{
				VM.Pointsize= value;
				NotifyPropertyChanged();
			}
		}

		public float Confidence
		{
			get
			{
				return VM.Confidence;
			}
			set
			{
				VM.Confidence = value;
				NotifyPropertyChanged();
			}
		}

		public int Near
		{
			get
			{
				return VM.Near;
			}
			set
			{
				VM.Near = value;
				NotifyPropertyChanged();
			}
		}

		public int Far
		{
			get
			{
				return VM.Far;
			}
			set
			{
				VM.Far = value;
				NotifyPropertyChanged();
			}
		}

		public float Speed
		{
			get
			{
				return SharpTracerModel.Speed;
			}
			set
			{
				SharpTracerModel.Speed = value;
			}
		}

		public float Time
		{
			get
			{
				return VM.Model.Renderer.CurrentState.Time;
			}
			set
			{
				VM.Model.Renderer.CurrentState.Time = value;
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

		public CameraControlVM(MainWindowVM VM)
		{
			this.VM = VM;
			Axis = "X";
			ShowGizmos = new Command(() => true, (x) => VM.IsolateGizmos = !VM.IsolateGizmos, "ShowGizmos");
			ShowSingle = new Command(() => true, (x) => VM.SingleEntity = !VM.SingleEntity, "ShowSingle");
			GeometryFreq = new Command(() => true, (x) => VM.Model.GeometryFreq = Convert.ToInt32(x), "GeometryFreq");
			ResetView = new Command(() => true, (x) => VM.Model.ResetView(), "ResetView");
		}

		public void Update()
		{
			NotifyPropertyChanged(nameof(Zoom));
			NotifyPropertyChanged(nameof(FOV));
			NotifyPropertyChanged(nameof(Time));
		}
	}
}
