using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Maths
{
	class Tools
	{
        public static float smoothstep(float edge0, float edge1, float x)
        {
            // Scale, bias and saturate x to 0..1 range
            x = clamp((x - edge0) / (edge1 - edge0), 0.0f, 1.0f);
            // Evaluate polynomial
            return x * x * (3 - 2 * x);
        }
        public static float clamp(float x, float lowerlimit, float upperlimit)
        {
            if(x < lowerlimit)
                x = lowerlimit;
            if(x > upperlimit)
                x = upperlimit;
            return x;
        }

		public static float Interp(float start, float end, float t)
		{
			return ((1.0f - t) * start) + (t * end);
		}
		public static vec2 Interp(vec2 start, vec2 end, float t) 
		{
			return ((1.0f - t) * start) + (t * end);
		}
		public static vec3 Interp(vec3 start, vec3 end, float t)
		{
			return ((1.0f - t) * start) + (t * end);
		}
		public static vec4 Interp(vec4 start, vec4 end, float t)
		{
			return ((1.0f - t) * start) + (t * end);
		}

		public static vec3 Reflect(vec3 v, vec3 n)
		{
			return v - 2.0f * vec3.Dot(v, n) * n;
		}
		public static bool Refract(vec3 v, vec3 n, float ni_over_nt, ref vec3 refracted)
		{
			vec3 uv = (v).Normalized;
			float dt = vec3.Dot(uv, n);
			float discriminant = 1.0f - ni_over_nt * ni_over_nt * (1 - dt * dt);
			if (discriminant > 0)
			{
				refracted = ni_over_nt * (uv - n * dt) - n * (float)Math.Sqrt(discriminant);
				return true;
			}
			return false;
		}
		public static float SchlickApproximation(float cos, float ref_idx)
		{
			float r0 = (1 - ref_idx) / (1 + ref_idx);
			r0 = r0 * r0;
			return r0 + (1 - r0) * (float)Math.Pow(1 - cos, 5);
		}
	}
}
