using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTracer.Engine.Maths
{
    internal class Noise
    {
		private static Random rand = new Random();

		public static vec3 Random3()
		{
			vec3 p;
			do
			{
				p = (2.0f * new vec3(Random1(), Random1(), Random1())) - 1.0f;
			} while (p.Length >= 1.0f);
			return p;
		}

		public static vec2 Random2()
		{
			vec2 p;
			do
			{
				p = 2.0f * new vec2(Random1(), Random1()) - new vec2(1);
			} while (p.Length >= 1.0f);
			return p;
		}

		public static float Random1()
		{
			return rand.Next() / (float)int.MaxValue;
		}
	}
}
