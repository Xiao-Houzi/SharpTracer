using SharpTracer;
using GlmSharp;
using System;
using SharpTracer.Base;

namespace SharpTracer
{
	public class Matrix3VM : NotificationBase
	{
		mat3 matrix3;
		public float this[int index]
		{
			get
			{
				return Matrix3[index];
			}
			set
			{
				matrix3[index] = value;
				NotifyPropertyChanged();
				Update?.Invoke();
			}
		}

		public mat3 Matrix3
		{
			get { return matrix3; }
			set
			{
				matrix3 = value;
				for(int i=0;i<9;i++)
				this[i] = value[i];
				NotifyPropertyChanged();
				Update?.Invoke();
			}
		}

		public Command UpdateMatrix
		{
			get;set;
		}

		public Action Update
		{
			get;set;
		}

		public Matrix3VM(mat3 matrix,  Action action)
		{
			Update = action;
			Matrix3 = matrix;
			UpdateMatrix = new Command(() => true,
				(x) => { UpdateFromSelected(); }, "UpdateMatrix");
		}

		private void UpdateFromSelected()
		{
			if (SharpTracerModel.SelectedEntity == null) return;

		}
	}
}
