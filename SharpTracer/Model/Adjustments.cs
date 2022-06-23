using GlmSharp;
using SharpTracer.Model.Infrastructure;
using SharpTracer.Base;

namespace SharpTracer
{
	public class Adjustments : NotificationBase
	{
		private mat4 matrix;
		private mat3 depthAdj;
		private vec3 trans;

		public mat4 Matrix 
		{ 
			get => matrix;
			set
			{
				Extract( value);
				NotifyPropertyChanged();
			}
		}

		public mat3 DepthAdjustment
		{
			get => depthAdj;
			set
			{
				depthAdj = value;
				NotifyPropertyChanged();
			}
		}
	
		public vec3 Trans 
		{ 
			get => trans; 
			set => trans = value; 
		}

		public Axes Axes { get; set; }

		public Adjustments()
		{
			matrix = mat4.Identity;
			depthAdj = mat3.Identity;
			trans = new vec3();
		}

		void Extract(mat4 matrix)
		{
			this.matrix = matrix;

			trans = new vec3(matrix[3], matrix[7], matrix[11]);
			matrix[3] = matrix[7] = matrix[11] = 1;

			NotifyPropertyChanged(nameof(Matrix));
			NotifyPropertyChanged(nameof(Trans));
		}

		public void Update()
		{
			NotifyPropertyChanged(nameof(Axes));
			NotifyPropertyChanged(nameof(Matrix));
			NotifyPropertyChanged(nameof(DepthAdjustment));
			SharpTracerEvent.UI(this, SharpTracerUIArgs.EventReason.AdjustmentMatrixChanged, Matrix);
		}

	
	}
}