using SharpEngine.Engine.Graphics;
using GlmSharp;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Engine
{
	public class Layer
	{
		private List<Entity> _entities;

		public List<Entity> Entities
		{
			get
			{
				return _entities;
			}

			set
			{
				_entities = value;
			}
		}

		public Camera Camera
		{
			get; set;
		}
		public bool IsFlat
		{
			get;
			set;
		}
		public mat4 ShaderData
		{
			get; set;
		}


		public Layer(bool flat = false)
		{
			ShaderData = new mat4();
			Entities = new List<Entity>();
			Camera = new Camera();
		}

		public void Initialise()
		{
			foreach(Entity entity in Entities)
				entity?.Initialise();
		}

		public void AddEntity(Entity entity)
		{
			Entities.Add(entity);
		}

		public void Clear()
		{
			Entities.Clear();
		}
	}
}
