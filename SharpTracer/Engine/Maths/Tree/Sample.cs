using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Maths
{

    public class Sample<T>
    {
        private T _payload;
        private Func<T,float>[] _datumFunctions;


        public float this[int Index]
        {
            get
            {
                return _datumFunctions[Index](_payload);
            }
        }

        public int Dimensions
        {
            get => _datumFunctions.Length;
        }

        public T Payload
        {
            get
            {
                return _payload;
            }
            set
            {
                _payload = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="datumFunctions"></param>
        public Sample(T payload, Func<T,float>[] datumFunctions)
        {
            _payload = payload;
            _datumFunctions = datumFunctions;
        }
    }
}
