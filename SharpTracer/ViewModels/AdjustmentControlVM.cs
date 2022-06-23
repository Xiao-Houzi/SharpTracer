using SharpEngine.Maths;
using SharpTracer;
using GlmSharp;
using SharpTracer.Model.Infrastructure;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using SharpTracer.Base;

namespace SharpTracer
{
	public class AdjustmentControlVM : NotificationBase
	{
		private Adjustments adjustments;

		private Matrix3VM matrix3vm;
		private MatrixVM matrixvm;
		private bool _mValid = true;
		public static readonly DependencyProperty TitleProperty =
				 DependencyProperty.Register(
				 "Title", typeof(String),
				 typeof(AdjustmentControls)
				 );

		public String Title
		{
			get
			{
				return (String)GetValue(TitleProperty);
			}
			set
			{
				SetValue(TitleProperty, value);
			}
		}
		public ObservableCollection<DynamicString> Axes
		{
			get;
			internal set;
		}

		public Command Click { get; set; }

		public MatrixVM Matrix
		{
			get { return matrixvm; }
			set { matrixvm = value;NotifyPropertyChanged(); }
		}

		public Matrix3VM Matrix3
		{
			get;
			set;
		}

		public string ValidMatrix
		{
			get
			{
				if(_mValid)
					return "White";
				else return "Red";
			}
		}

		public string Chirality
		{
			get
			{
				float c = vec3.Dot(vec3.Cross(Matrix.Matrix.Row0.xyz, Matrix.Matrix.Row1.xyz), Matrix.Matrix.Row2.xyz);
				return c<0 ? "Left" : "Right";
			}
		}

		public AdjustmentControlVM(Adjustments adj)
		{
			adjustments = adj;

			matrixvm = new MatrixVM (adjustments.Update);
			matrix3vm = new Matrix3VM(adjustments.DepthAdjustment, adjustments.Update);

			Title = "Correction";
			Axes = new ObservableCollection<DynamicString>();

			Axes.Add(new DynamicString("+X") { VM = this, On = true });
			Axes.Add(new DynamicString("+Y") { VM = this, On = false });
			Axes.Add(new DynamicString("+Z") { VM = this, On = false });

			Axes.Add(new DynamicString("+X") { VM = this, On = false });
			Axes.Add(new DynamicString("+Y") { VM = this, On = true });
			Axes.Add(new DynamicString("+Z") { VM = this, On = false });

			Axes.Add(new DynamicString("+X") { VM = this, On = false });
			Axes.Add(new DynamicString("+Y") { VM = this, On = false });
			Axes.Add(new DynamicString("+Z") { VM = this, On = true });

			Axes.Add(new DynamicString("+X") { VM = this, On = true });
			Axes.Add(new DynamicString("+Y") { VM = this, On = true });
			Axes.Add(new DynamicString("+Z") { VM = this, On = true });

			Click = new Command(()=>true, ClickExecute);
			SharpMessenger.UIEvent += SharpTracerMessenger_UIEvent;
			PropertyChanged += change;
		}

		private void change(object sender, PropertyChangedEventArgs e)
		{
			
		}

		private void SharpTracerMessenger_UIEvent(object sender, SharpTracerUIArgs args)
		{
			switch(args.Reason)
			{
				case SharpTracerUIArgs.EventReason.UpdateAxes:
					adjustments.Update();
						break;
			}
		}

		private void ClickExecute(object Parameter)
		{
			DynamicString text = (DynamicString)Parameter;

			char sign = text.Value[0];
			char axis = text.Value[1];

			if (sign == '-')
				sign = '+';
			else
				sign = '-';
			int x = Axes.IndexOf((DynamicString)Parameter);
			if (x < 9)
				for (int i = 0; i < 9; i++)
				{
					if (Axes[i].Value[1] == axis)
					{
						Axes[i].On = false;
					}
				}

			text.On = true;

			text.Value = new string(new char[] { sign, axis });

			int n = 0;
			foreach (DynamicString a in Axes)
			{
				if(a.On)
				{
					if(a.Value[0] == '+')
						Matrix[n] = 1;
					else
						Matrix[n] = -1;
				}
				else
				{
					Matrix[n] = 0;
				}
				n++;
				if(n % 4 == 3)
				{
					Matrix[n] = 0;
					n++;
				}
				if(n == 12) break;
			}
			Matrix[3] = Axes[9].Value[0] == '+' ? 0 : -1;
			Matrix[7] = Axes[10].Value[0] == '+' ? 0 : -1;
			Matrix[11] = Axes[11].Value[0] == '+' ? 0 : -1;
			Matrix[15] = 1;
			adjustments.Matrix = Matrix.Matrix;
			Validate();
			adjustments.Update();
		}

		private void Validate()
		{
			_mValid = true;

			bool[] line = new bool[6] { false, false, false, false, false, false };

			for(int i = 0; i < 3; i++)
			{
				if(Axes[i].On)
					if(line[0] == false) line[0] = true; else _mValid = false;
				if(Axes[i+3].On)
					if(line[1] == false) line[1] = true; else _mValid = false;
				if(Axes[i+6].On)
					if(line[2] == false) line[2] = true; else _mValid = false;
				if(Axes[i * 3].On)
					if(line[3] == false) line[3] = true; else _mValid = false;
				if(Axes[i*3+1].On)
					if(line[4] == false) line[4] = true; else _mValid = false;
				if(Axes[i*3+2].On)
					if(line[5] == false) line[5] = true; else _mValid = false;
			}
			NotifyPropertyChanged(nameof(ValidMatrix));
			NotifyPropertyChanged(nameof(Chirality));
		}

		public void Update()
		{
			NotifyPropertyChanged(nameof(Axes));
			NotifyPropertyChanged(nameof(Matrix));
			NotifyPropertyChanged(nameof(Matrix3));
			SharpTracerEvent.UI(this, SharpTracerUIArgs.EventReason.AdjustmentMatrixChanged, Matrix);
		}
	}

	public class DynamicString : NotificationBase
	{
		private AdjustmentControlVM _vm;
		private String _value;
		public String Value
		{
			get { return _value; }
			set
			{
				_value = value;
				NotifyPropertyChanged();
			}
		}

		public bool On { get; set; }

		public string Col
		{
			get
			{
				if (On)
					return "Black";
				else return "Gray";
			}
		}

		public string Bak
		{
			get
			{
				if(On)
					return "Yellow";
				else return "Gray";
			}
		}

		public AdjustmentControlVM VM
		{
			get => _vm; set
			{
				_vm = value; NotifyPropertyChanged();
			}
		}

		public DynamicString(AdjustmentControlVM vm)
		{
			_vm = vm;
		}

		public DynamicString(string value)
		{
			_value = value;
		}

		public static implicit operator DynamicString(string value)
		{
			return new DynamicString(value);
		}
	}
}

