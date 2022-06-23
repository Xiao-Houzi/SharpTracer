using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Maths
{
	class Tools
	{
        static float smoothstep(float edge0, float edge1, float x)
        {
            // Scale, bias and saturate x to 0..1 range
            x = clamp((x - edge0) / (edge1 - edge0), 0.0f, 1.0f);
            // Evaluate polynomial
            return x * x * (3 - 2 * x);
        }

        static float clamp(float x, float lowerlimit, float upperlimit)
        {
            if(x < lowerlimit)
                x = lowerlimit;
            if(x > upperlimit)
                x = upperlimit;
            return x;
        }
    }
}
