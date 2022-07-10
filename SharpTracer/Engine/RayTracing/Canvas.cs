using GlmSharp;
using SharpEngine.Maths;
using SharpTracer.Engine.Maths;
using SharpTracer.Engine.Scene;
using SharpTracer.Engine.Scene.RenderGeometry;
using SharpTracer.Model;
using SharpTracer.Model.Base.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpTracer.Engine.RayTracing
{
    public class Canvas
    {
        public Canvas()
        {
            _samples = 50;
        }

        public void Initialise(Project project)
        {
            _project = project;
           
        }
        public void Render()
        {
            _width = _project.FrameWidth;
            _height = _project.FrameHeight;
            _pixels = new vec4[_width * _height];
            _collected = new vec4[_width * _height];
            for (int i = 0; i < _height; i++)
                for (int j = 0; j < _width; j++)
                {
                    _pixels[i * _width + j] = new vec4((1.0f / _height) * i, (1.0f / _width) * j, 0, 0);
                    _collected[i * _width + j] = new vec4(0);
                }
            RaiseEvent.Model(this, EventReason.RenderStarted, null);
            Task.Run(() =>
            {
                _rendering = true;
                float u, v;
                for (int sample = 0; sample < _samples; sample++)
                {
                    _sample = sample;
                    for (float i = 0; i < _height; i++)
                        for (float j = 0; j < _width; j++)
                        {
                            vec4 color = new vec4(0);

                            float x = Noise.Random1() - 0.5f;
                            float y = Noise.Random1() - 0.5f;
                            u = (j + x) / _width;
                            v = (i + y) / _height;
                            vec4 col = GetColour(u, v);
                            SetPixel((int)j, (int)i, sample,col);
                            //SetPixel((int)j, (int)i, sample, new vec4(1.f));
                        }
                   RaiseEvent.Model(this, EventReason.RenderUpdated, (1.0f/_samples) * sample);
                }
                _rendering = false;
                RaiseEvent.Model(this, EventReason.RenderEnded, null);
            });

        }
        public byte[] GetData()
        {
            _renderGuard.WaitOne();
            vec4[] data = _pixels;
            _renderGuard.ReleaseMutex();
            byte[] bytes = new byte[data.Length * 4];
            int index = 0;
            foreach (vec4 v in data)
            {
                bytes[index + 0] = (byte)(v.r * 127);
                bytes[index + 1] = (byte)(v.g * 127);
                bytes[index + 2] = (byte)(v.b * 127);
                bytes[index + 3] = (byte)(v.a * 127);
                index += 4;
            }
            return bytes;
        }

        internal vec4 GetColour(float u, float v)
        {
            return Colour(_project.SceneCamera.GetRay(u, v));
        }

        vec4 Colour(Ray ray, int bounce = 0)
        {
            Hit hit = new Hit();

            bool isHit = false;
            float max = 1000.0f;
            foreach (Entity entity in _project.Entities)
            {
                if (entity.Geometry.Test(ray, entity.Transform, entity.Material, 0.001f, max, ref hit))
                {
                    isHit = true;
                    max = hit.t;
                }
            }

            if (isHit)
            {

                Ray scattered = null;
                vec4 attenuation = new vec4(1);
                vec4 emitted = hit.Material.Emmited(hit.u, hit.v, ref hit.p);
                if (bounce < 50 && hit.Material.Scattered(ray, hit, ref attenuation, ref scattered))
                    return emitted + attenuation * Colour(scattered, bounce + 1);
                else
                    return emitted;
            }

            vec3 d = ray.Direction;
            float t = 0.5f * (d.y + 1.0f);

            // background
            return Tools.Interp(new vec4(1.0f), new vec4(0.5f, 0.7f, 1.0f, 1.0f), t);
        }

        void SetPixel(int x, int y, int sample, vec4 colour)
        {
            colour.a = 1;
            _collected[y * _width + x] += colour;
            _renderGuard.WaitOne();
            _pixels[y * _width + x] =  _collected[y * _width + x] / (float)(sample + 1.0f);
            _renderGuard.ReleaseMutex();
        }

        #region Private
        private Mutex _renderGuard = new Mutex();
        uint _width, _height;
        vec4[] _pixels, _collected;
        private int _samples, _sample;
        private bool _rendering;
        private Project _project;
        #endregion
    }
}
