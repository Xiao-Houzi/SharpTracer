using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SharpEngine.Maths
{
    /// <summary>
    /// The pose class combines Translation, Orientation and Timestamp to gove a complete 4D position
    /// </summary>
    public class Transform
    {
        /// <summary>
        /// 
        /// </summary>
        public vec3 Translation
        { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public quat Orientation
        { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public vec3 Scale
        { get; set; }

        public float this[int r, int c]
        {
            get { return _transform[r, c]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Transform()
        {
            _translation = new vec3();
            _orientation = new quat(0, 0, 0, 1);
            _scale = new vec3(1);
            _transform = ToMatrix();
        }

        public Transform(Transform other)
        {
            this.Translation = other.Translation;
            this.Orientation = other.Orientation;
            this.Scale = other.Scale;
        }

        public mat4 ToMatrix()
        {
            _transform = Orientation.ToMat4;

            _transform.m30 = Translation.x;
            _transform.m31 = Translation.y;
            _transform.m32 = Translation.z;

            return _transform;
        }

        public float[] ToArray()
        {
            return new float[16];
        }

        internal void Translate(vec3 displacement)
        {
            Translation += displacement;
        }


        #region private
        vec3 _translation;
        quat _orientation;
        vec3 _scale;
        mat4 _transform;
        #endregion
    }
}
