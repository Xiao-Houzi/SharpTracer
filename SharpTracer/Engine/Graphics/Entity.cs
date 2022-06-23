using SharpGL;
using SharpEngine.Maths;
using GlmSharp;
using System;
using System.Collections.Generic;


namespace SharpEngine.Engine.Graphics
{
    public class Entity
    {
        public List<Entity> Children
        {
            get; set;
        }

        public Mesh Mesh
        { get { return _mesh; } set { _mesh = value; } }

        public Transform Transform
        {
            get { return _transform; }
            set { _transform = value; }
        }

        public Material Material
        {
            get { return _material; } set { _material = value; } 
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public float Time
        {
            get;
            set;
        }

        public Entity()
        {
            _name = "";
            _geometry = null;
            _mesh = null;
            _material = new Material();
            _transform = new Transform();
            _script = null;

        }
        public Entity(string name) : this()
        {
            _name = name;
        }
        public Entity(string name, Mesh mesh, Material material) : this(name)
        {
            _mesh = mesh;
            _material = material;
        }
        public Entity(string name, Mesh mesh, Material material, IScript script) : this(name, mesh, material)
        {
            _script = script;
        }

        public virtual void Initialise()
        {
            _script?.Initialise(this);

        }
        public void Rotate(float x, float y, float z)
        {

        }

        public List<Entity> FetchDescendants()
        {
            return new List<Entity>();
        }

        public virtual void Run(float delta)
        {
            Time += delta;
            _script?.Run(delta);
        }

        public void Render(Layer layer, State state)
        {
            GLLayer.Render(state, layer, this);
        }

        private string _name;
        protected Geometry _geometry;
        protected Mesh _mesh;
        protected Material _material;
        protected Transform _transform;
        protected IScript _script;
    }
}
