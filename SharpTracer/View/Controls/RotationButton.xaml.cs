using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using SharpEngine.Maths;
using GlmSharp;

namespace SharpTracer
{
	/// <summary>
	/// Interaction logic for OrientationButtonX.xaml
	/// </summary>
	public partial class OrientationButton : UserControl
	{
		List<vec3> CubePoints = new List<vec3>();

		public Double X, Y, Z;

		public mat4 CurrentTransform { get; private set; }

		public OrientationButton()
		{
			InitializeComponent();

			X = Y = Z = 0;

			float[] vp =
			{
					 -0.5f, -0.5f, 0.5f,
					 0.5f, -0.5f, 0.5f,
					 0.5f, 0.5f, 0.5f,
					 -0.5f, 0.5f, 0.5f,
					 -0.5f, -0.5f, -0.5f,
					 0.5f, -0.5f, -0.5f,
					 0.5f, 0.5f, -0.5f,
					 -0.5f, 0.5f, -0.5f,

					 -0.2f, -0.2f , 0.5f,
					 0.2f, -0.2f , 0.5f,
					 0.0f, -0.2f , 0.5f,
					 0.0f, 0.2f , 0.5f,

				};

			int[] e =
			{
					 0,1,	 1,2,	 2,3,	 3,0,

					 4,5,	 5,6,	 6,7,	 7,4,

					 0,4,	 1,5,	 2,6,	 3,7,

					 8,9,				 10,11,
				};

			for (int i = 0; i < 28; i++)
			{
				CubePoints.Add(new vec3(vp[e[i] * 3 + 0], vp[e[i] * 3 + 1], vp[e[i] * 3 + 2]));
			}
		}

		protected override void OnRender(DrawingContext DC)
		{
			// create a Matrix44 from the various options
			mat4 xrM = new mat4(),
				yrM = new mat4(),
				zrM = new mat4(),
				cameraM = new mat4(),
				perspectiveM = new mat4();

			// x rotation
			xrM.m11 = (float)Math.Cos(X / 57.295);
			xrM.m22 = (float)Math.Cos(X / 57.295);
			xrM.m12 = (float)Math.Sin(X / 57.295);
			xrM.m21 = -(float)Math.Sin(X / 57.295);

			// y rotation
			yrM.m00 = (float)Math.Cos(Y / 57.295);
			yrM.m22 = (float)Math.Cos(Y / 57.295);
			yrM.m02 = -(float)Math.Sin(Y / 57.295);
			yrM.m20 = (float)Math.Sin(Y / 57.295);

			// z rotation
			zrM.m00 = (float)Math.Cos(Z / 57.295);
			zrM.m11 = (float)Math.Cos(Z / 57.295);
			zrM.m01 = (float)Math.Sin(Z / 57.295);
			zrM.m10 = -(float)Math.Sin(Z / 57.295);

			// camera
			float distance = 4;
			cameraM.m32 = -distance;


			// projection
			float cotangent = (float)Math.Atan(3.14159 / 6.0);

			float nearclip = 1;
			float farclip = 5;

			perspectiveM.m00 = cotangent; perspectiveM.m01 = 0.0f; perspectiveM.m02 = 0.0f; perspectiveM.m03 = 0.0f;
			perspectiveM.m10 = 0.0f; perspectiveM.m11 = cotangent; perspectiveM.m12 = 0.0f; perspectiveM.m13 = 0.0f;
			perspectiveM.m20 = 0.0f; perspectiveM.m21 = 0.0f; perspectiveM.m22 = -((farclip + nearclip) / (farclip - nearclip)); perspectiveM.m23 = -((farclip * nearclip) / (farclip - nearclip));
			perspectiveM.m30 = 0.0f; perspectiveM.m31 = 0.0f; perspectiveM.m32 = -1.0f; perspectiveM.m33 = 0.0f;

			CurrentTransform = xrM * yrM * zrM * cameraM * perspectiveM;

			Double sz = RenderSize.Width * distance;

			DC.DrawRectangle(Brushes.AliceBlue, new Pen(Brushes.Black, 0), new Rect(0, 0, RenderSize.Width, RenderSize.Height));
			//DC.DrawRectangle(null, new Pen(Brushes.Black, .5), new Rect(0, 0, RenderSize.Width, RenderSize.Height * 0.1));
			DC.DrawEllipse(Brushes.LightGoldenrodYellow, new Pen(Brushes.Black, .5), new Point(RenderSize.Width * 0.25, RenderSize.Height * 0.1), RenderSize.Width * 0.1, RenderSize.Height * 0.1);
			DC.DrawEllipse(Brushes.LightGoldenrodYellow, new Pen(Brushes.Black, .5), new Point(RenderSize.Width * 0.5, RenderSize.Height * 0.1), RenderSize.Width * 0.1, RenderSize.Height * 0.1);
			DC.DrawEllipse(Brushes.LightGoldenrodYellow, new Pen(Brushes.Black, .5), new Point(RenderSize.Width * 0.75, RenderSize.Height * 0.1), RenderSize.Width * 0.1, RenderSize.Height * 0.1);

			DC.DrawLine(new Pen(Brushes.Red, .5), new Point(RenderSize.Width * 0.25, RenderSize.Height * 0.1), new Point(RenderSize.Width * 0.25 + RenderSize.Width * 0.1 * Math.Sin(X / 57.295), (RenderSize.Height * 0.1 + RenderSize.Height * 0.1 * Math.Cos(X / 57.295))));
			DC.DrawLine(new Pen(Brushes.Red, .5), new Point(RenderSize.Width * 0.5, RenderSize.Height * 0.1), new Point(RenderSize.Width * 0.5 + RenderSize.Width * 0.1 * Math.Sin(Y / 57.295), (RenderSize.Height * 0.1 + RenderSize.Height * 0.1 * Math.Cos(Y / 57.295))));
			DC.DrawLine(new Pen(Brushes.Red, .5), new Point(RenderSize.Width * 0.75, RenderSize.Height * 0.1), new Point(RenderSize.Width * 0.75 + RenderSize.Width * 0.1 * Math.Sin(Z / 57.295), (RenderSize.Height * 0.1 + RenderSize.Height * 0.1 * Math.Cos(Z / 57.295))));

			//DC.DrawLine(new Pen(Brushes.Red, .5), new Point(RenderSize.Width * (Z/360), 0), new Point(RenderSize.Width * (Z / 360), RenderSize.Height * 0.1));

			for (int i = 0; i < 28; i += 2)
			{
				Point p1, p2;

				vec3 point1 = (CurrentTransform*new vec4(CubePoints[i])).xyz, point2 = (CurrentTransform*new vec4(CubePoints[i + 1])).xyz;

				p1 = new Point(point1.x * sz + RenderSize.Width * .6, point1.y * sz + RenderSize.Height * .55);
				p2 = new Point(point2.x * sz + RenderSize.Width * .6, point2.y * sz + RenderSize.Height * .55);

				DC.DrawLine(new Pen(Brushes.SlateGray, .5), p1, p2);
			}
		}

		public void Rotate(Double x, Double y, Double z)
		{
			X += x; Y += y; Z += z;

			if (X > 360)
				X -= 360;
			if (X < 00)
				X += 360;

			if (Y > 360)
				Y -= 360;
			if (Y < 00)
				Y += 360;

			if (Z > 360)
				Z -= 360;
			if (Z < 00)
				Z += 360;

			InvalidateVisual();
		}

		public void ResetOrientation()
		{
			X = Y = Z = 0;
		}
	}
}
