using System.Collections.ObjectModel;

namespace SharpTracer
{
	public class Axes
	{
		public float[] AxisValues { get; set; }

		public Axes()
		{
			AxisValues = new float[12];
		}

		public static implicit operator Axes(ObservableCollection<DynamicString> axes)
		{
			AdjustmentControlVM VM = axes[0].VM;
			Axes Axes = new Axes();

			for(int  i=0; i<12;i++)
			{
				float val = 0;
				if (axes[i].On)
				{
					val = 1;
					if (axes[i].Value[0] == '-')
						val = -1;
				}
				Axes.AxisValues[i] = val;
			}

			return Axes;
		}
	}
}