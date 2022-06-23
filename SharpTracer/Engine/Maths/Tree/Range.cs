using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Maths
{
	public class Range
	{
        private float _lowerBound;
        private float _upperBound;

        public float LowerBound
        {
            get
            {
                return _lowerBound;
            }
            set
            {
                _lowerBound = value;
            }
        }
        public float UpperBound
        {
            get
            {
                return _upperBound;
            }
            set
            {
                _upperBound = value;
            }
        }

        public float Span
        {
            get
            {
                return _upperBound - _lowerBound;
            }
        }

        public Range(float lowerBound, float upperbound)
        {
            if(upperbound > lowerBound)
            {
                _lowerBound = lowerBound;
                _upperBound = upperbound;
            }
            else
            {
                _lowerBound = upperbound;
                _upperBound = lowerBound;
            }
        }

        public bool IsInRange(float sample)
        {
            if(sample < _lowerBound || sample > _upperBound)
                return false;
            return true;
        }
    }
}
