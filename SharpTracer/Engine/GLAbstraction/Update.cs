using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTracer.Engine.GLAbstraction
{
    internal class Update
    {
        public enum UpdateType
        {
            Texture,
            Geometry
        }


        internal UpdateType Type { get => _type;}
        public uint[] Values { get => _values; set => _values = value; }
        public uint Id { get => _id; set => _id = value; }
        public byte[] Data { get => _data; set => _data = value; }

        public Update(uint width, uint height, byte[]data, uint id)
        {
            _type = UpdateType.Texture;
            _values = new uint[2] { width, height };
            _id = id;
            _data = data;
        }

        public Update(uint vertices, uint indices, bool colours, bool uvs, bool normals, byte[]data, uint id)
        {
            _type = UpdateType.Geometry;
        }

        #region Private
        private byte[] _data;
        private uint _id;
        private uint[] _values;
        private UpdateType _type;

        #endregion
    }
}
