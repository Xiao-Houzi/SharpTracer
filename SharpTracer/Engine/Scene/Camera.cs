using System;
using SharpEngine.Maths;
using GlmSharp;
using SharpTracer.Engine.RayTracing;
using SharpTracer.Engine.Maths;

namespace SharpTracer.Engine
{
    public class Camera
    {
        public mat4 Matrix
        {
            get
            {
                mat4 view = mat4.LookAt(Position, _lookAt,  _up);
                return view;
            }
        }

        public float Zoom
        {
            get
            {
                return _zoom;
            }
            set
            {
                _zoom = value;
                _changed = true;
            }
        }

        public float FOV
        {
            get
            {
                return _vfov;
            }
            set
            {
                _vfov = value;
                if (_vfov < .1f) _vfov = .1f;
                if (_vfov > 3.1f) _vfov = 3.1f;
                _changed = true;
            }
        }

        public bool IsChanged
        {
            get
            {
                if (_changed)
                {
                    _changed = false;
                    return true;
                }
                return false;
            }

        }

        public vec3 Position
        {
            get => _position;
            set => _position = value;
        }
        public vec3 Target
        {
            get => _lookAt;
            set => _lookAt = value;
        }

        public float Roll
        {
            get => _roll;
            set => _roll = value;
        }

        public float LensRadius
        {
            get => _lensRadius;
            set => _lensRadius = value;
        }

        public Camera()
        {
            _vfov = 1.57f;
            _aspect = 1.0f;
            Reset();
            Initialise();
        }

        void Initialise()
        {
            float halfHeight = (float)Math.Tan(_vfov / 2);
            float halfWidth = _aspect * halfHeight;
            float focalDist = (_lookAt - _position).Length;

            _O = _position;

            _w = (_position - _lookAt).Normalized;
            _u = vec3.Cross(_up, _w).Normalized;
            _v = vec3.Cross(_w, _u).Normalized;

            _LL = _O - halfWidth * focalDist * _u - halfHeight * focalDist * _v - focalDist * _w;
            _H = 2 * halfWidth * focalDist * _u;
            _V = 2 * halfHeight * focalDist * _v;
        }

        public Ray GetRay(float s, float t)
        {
            vec3 rd = _lensRadius * Noise.Random3() * new vec3(1, 1, 0);
            vec3 offset = rd.x * _u + rd.y * _v;
            float time = _t0 + Noise.Random1() * (_t1 - _t0);
            return new Ray(_O + offset, _LL + s * _H + t * _V - _O - offset, time);
        }

        public void Aspect(int width, int height)
        {
            _aspect = width/height;
        }

        public void Translate(vec3 vector)
        {
            _position += vector;
        }

        public void Orbit(vec2 orbitChange)
        {
            vec3 range = (_position - _lookAt);
            _position = _lookAt + range ; //rotate 'range' proportional to orbitChange
            
        }

        public void Pan(float v1, int v2)
        {
            Translate(new vec3(-v1, v2, 0));
        }

        public void Tilt(float roll)
        {
            _roll += roll;
            if (_roll > Math.PI) _roll -= (float)(2*Math.PI);
            if (_roll < -Math.PI) _roll += (float)(2*Math.PI);
        }

        public void ZoomBy(float zoom)
        {
            _zoom += zoom;
            if (_zoom < 1f) _zoom = 1f;
            if (_zoom > 10f) _zoom = 10f;
            _changed = true;
        }

        public void Reset()
        {
            _position = new vec3(0, 0, 5);
            _lookAt = new vec3(0, 0, 0);
            _up = new vec3(0, 1, 0);
            _roll = 0;
            _zoom = 1;
            _vfov = 0.707f; // 45deg
            _lensRadius = 0.003f;
            _t0 = 0;
            _t1 = 0;
        }

        #region Private
        private vec3 _position, _lookAt, _up, _LL, _H, _V, _O, _u, _v, _w;
        private float _zoom, _lensRadius, _t0, _t1, _aspect, _vfov, _roll;
        private bool _changed;
        #endregion
    }
}