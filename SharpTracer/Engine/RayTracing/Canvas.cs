using GlmSharp;
using SharpEngine.Maths;
using SharpTracer.Engine.Maths;
using SharpTracer.Engine.Scene;
using SharpTracer.Engine.Scene.RenderGeometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTracer.Engine.RayTracing
{
    internal class Canvas
    {
        Canvas()
        {
            _samples = 500;
        }

        void Initialise(State state)
        {
            _state = state;
            _width = state.CanvasWidth;
            _height = state.CanvasHeight;
            _pixels = new vec4[_width * _height];
            _collected = new vec4[_width * _height];
            for (int i = 0; i < _height; i++)
                for (int j = 0; j < _width; j++)
                {
                    _pixels[i * _width + j] = new vec4((1.0f / _height) * i, (1.0f / _width) * j, 0, 0);
                    _collected[i * _width + j] = new vec4(0);
                }
            _data = new char[_width * _height * 3];
        }

        void Run()
        {
            Task.Run(() =>
            {
                _rendering = true;
                float u, v;
                for (int sample = 0; sample < _samples; sample++)
                {
                    for (float i = 0; i < _height; i++)
                        for (float j = 0; j < _width; j++)
                        {
                            vec4 color = new vec4(0);

                            float x = Noise.Random1() - 0.5f;
                            float y = Noise.Random1() - 0.5f;
                            u = (j + x) / _width;
                            v = (i + y) / _height;
                            SetPixel((int)j, (int)i, sample, GetColour(u, v));
                        }
                    _showframe = true;
                }
                _rendering = false;
            });

        }

        internal vec4 GetColour(float u, float v)
        {
            return Colour(_state.Camera.GetRay(u, v));
        }

        vec4 Colour(Ray ray, int bounce = 0)
        {
            Hit hit = null;

            bool isHit = false;
            float max = 1000.0f;
            foreach (Entity entity in _state.Layers[""].Entities)
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
                vec4 attenuation = new vec4(0);
                vec4 emitted = hit.Material.Emmited(hit.u, hit.v, ref hit.p);
                if (bounce < 50 && hit.Material.Scattered(ray, hit, ref attenuation, ref scattered))
                    return emitted + attenuation * Colour(scattered, bounce + 1);
                else
                    return emitted;
            }

            vec3 d = ray.Direction;
            float t = 0.5f * (d.y + 1.0f);

            // background
            return new vec4(0);
            return Tools.Interp(new vec4(1.0f), new vec4(0.5f, 0.7f, 1.0f, 1.0f), t);
        }

        void SetPixel(int x, int y, int sample, vec4 colour)
        {
            _collected[y * _width + x] += colour;
            _pixels[y * _width + x] = _collected[y * _width + x] / (float)(sample + 1.0f);
        }

        int GetWidth() { return _width; }
        int GetHeight() { return _height; }

        char[] GetData()
        {
            for (int i = 0; i < _height; i++)
                for (int j = 0; j < _width; j++)
                {
                    int datapos = i * _width + j;
                    int pixelpos = i * _width + j;

                    _data[datapos * 3 + 0] = (char)(Math.Min(Math.Sqrt(_pixels[pixelpos].r), 1.0f) * 255.0f);
                    _data[datapos * 3 + 1] = (char)(Math.Min(Math.Sqrt(_pixels[pixelpos].g), 1.0f) * 255.0f);
                    _data[datapos * 3 + 2] = (char)(Math.Min(Math.Sqrt(_pixels[pixelpos].b), 1.0f) * 255.0f);
                }
            return _data;
        }

        private State _state;

        #region Private
        int _width, _height;
        vec4[] _pixels, _collected;
        char[] _data;
        private int _samples;
        private bool _showframe;
        private bool _rendering;
        #endregion
    }
}
