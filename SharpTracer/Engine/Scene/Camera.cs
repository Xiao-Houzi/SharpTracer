using System;
using SharpEngine.Maths;
using GlmSharp;

namespace SharpEngine.Engine
{
	public class Camera
	{
		private vec3 _position;
		private float _incline, _tilt, _direction;
		private bool _changed;
		private float _zoom;

		public float view;

		public Camera()
		{
			_position = new vec3(0, 0, 0);
			_direction = 0;
			_zoom = 1;
			view = .707f;
		}

		public mat4 Matrix
		{
			get
			{
				vec3 pos = new vec3(Position);
				vec3 zoom = new vec3(0, 0, _zoom);
				vec3 up = new vec3(0, 1, 0);

				vec3 dir = new vec3(0, 0, -1);
				up *= quat.FromAxisAngle(_tilt, new vec3(0, 0, -1));
				dir *= quat.FromAxisAngle(_incline, new vec3(1, 0, 0));
				dir *= quat.FromAxisAngle(_direction, new vec3(0, 1, 0));

				pos -= dir * zoom.z;

				mat4 view = mat4.LookAt(pos, Position, up);
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
				return view;
			}
			set
			{
				view = value;
				if(view < .1) view = .1f;
				if(view > 3) view = 3;
				_changed = true;
			}
		}

		public bool IsChanged
		{
			get
			{
				if(_changed)
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
	
		}
		public float Direction
		{
			get => _direction;
			set => _direction = value;
		}
		public float Incline
		{
			get => _incline;
			set => _incline = value;
		}
		public float Tilt
		{
			get => _tilt;
			set => _tilt = value;
		}

		public void Translate(vec3 vector)
		{
			vector *= quat.FromAxisAngle(Direction, new vec3(0, 1, 0));
			_position -= vector;
		}

		public void CameraOrbit(float orbitChange)
		{
			Direction += orbitChange;
			if(Direction > Math.PI * 2) Direction = 0;
			if(Direction < 0) Direction = (float)Math.PI*2;
		}

		public void CameraPan(float v1, int v2)
		{
			Translate(new vec3(-v1, v2, 0));
		}

		public void CameraIncline(float v)
		{
			_incline += v;
			if(_incline > 1) _incline = 1;
			if(_incline < -1) _incline = -1;
		}
		public void CameraTilt(float v)
		{
			_tilt += v;
			if(_tilt > 1) _tilt = 1;
			if(_tilt < -1) _tilt = -1;
		}

		public void CameraZoom(float zoom)
		{
			_zoom += zoom;
			if(_zoom < 1f) _zoom = 1f;
			if(_zoom > 10f) _zoom = 10f;
			_changed = true;
		}
		public void CameraFOV(float rad)
		{
			view -= rad;
			if(view < .1) view = .1f;
			if(view > 3) view = 3;
			_changed = true;
		}

		public void Advance(float v)
		{
			_position += new vec3(0,0,-v) * quat.FromAxisAngle(Direction, new vec3(0,1,0));
		}

		public void Reset()
		{
			_position = new vec3(0, 0, 0);
			_direction = 0;
			_incline = 0;
			_tilt = 0;
		}
	}
}