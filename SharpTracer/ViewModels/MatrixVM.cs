using SharpEngine.Maths;
using SharpTracer;
using GlmSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using SharpTracer.Base;

namespace SharpTracer
{
	public class MatrixVM : NotificationBase
	{
		private mat4 matrix = new mat4();

		public static readonly DependencyProperty TitleProperty =
		 DependencyProperty.Register(
		 "Title", typeof(String),
		 typeof(Matrix)
		 );

		public String Title
		{
			get { return (String)GetValue(TitleProperty); }
			set
			{
				SetValue(TitleProperty, value);
			}
		}
	
		public float this[int index]
		{
			get
			{
				return matrix[index];
			}
			set
			{
				matrix[index] = value;
				NotifyPropertyChanged();
				Update?.Invoke();
			}
		}

		public mat4 Matrix
		{
			get
			{
				return matrix;
			}

			set
			{
				matrix = value;
				NotifyPropertyChanged();
			}
		}

		public Action Update
		{
			get;set;
		}

		public MatrixVM(Action action)
		{
			Update = action;
			matrix = new mat4();
		}

		public MatrixVM(mat4 matrix) : this(null)
		{
			SetMatrix(matrix);
		}

		public MatrixVM SetMatrix(mat4 matrix)
		{
			this.matrix = matrix;
			return this;
		}

		public static MatrixVM operator *(MatrixVM lhs, MatrixVM rhs)
		{
			MatrixVM newVM = new MatrixVM(null);

			newVM.matrix = lhs.matrix * rhs.matrix;

			return newVM;
		}
	}

	public class DynamicFloat : NotificationBase
	{
		private MatrixVM _vm;
		private float _value;
		public float Value
		{
			get { return _value; }
			set {
				_value = value;
				_vm?.Update();
				NotifyPropertyChanged();
			}
		}

		public MatrixVM VM { get => _vm; set => _vm = value; }

		public DynamicFloat(MatrixVM vm)
		{
			_vm = vm;
		}

		public DynamicFloat(float value)
		{
			_value = value;
		}

		public static implicit operator DynamicFloat(float value)
		{
			return new DynamicFloat(value);
		}
	}
}
