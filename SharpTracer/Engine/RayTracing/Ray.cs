using GlmSharp;

namespace SharpTracer.Engine.RayTracing
{
    public class Ray
    {
        public float Time { get; } = 0;
        public vec3 Origin
        { get { return _origin; } }

        public vec3 Direction
        { get { return _direction; } }

        public Ray()
        {
            _origin = new vec3(0);
            _direction = new vec3(0);
        }

        public Ray(vec3 origin, vec3 direction, float time)
        {
            _origin = origin;
            _direction = direction.Normalized;
        }

        public vec3 PointAt(float time)
        {
            return _origin + time * _direction;
        }

        #region Private
        vec3 _origin, _direction;
        #endregion
    }
}
