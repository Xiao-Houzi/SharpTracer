using SharpEngine.Maths;
using System.Collections.Generic;
using SharpTracer.Engine;
using SharpTracer.Engine.Graphics;
using SharpTracer.Engine.Scene.RenderGeometry;
using SharpTracer.Maths;

namespace SharpTracer.Engine.Scene
{
    public class Entity
    {
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public DisplayType DisplayMode => _diplayMode;

        public Entity Parent
        { get; set; }

        public List<Entity> Children
        {
            get; set;
        }

        public Geometry Geometry
        { get { return _mesh; } set { _mesh = value; } }

        public Transform Transform
        {
            get { return _transform; }
            set { _transform = value; }
        }

        public Material Material
        {
            get { return _material; }
            set { _material = value; }
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
        public Entity(string name, Geometry mesh, Material material) : this(name)
        {
            _mesh = mesh;
            _material = material;
        }
        public Entity(string name, Geometry mesh, Material material, IScript script) : this(name, mesh, material)
        {
            _script = script;
        }

        public virtual void Initialise()
        {
            _mesh.Initialise();
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
            _time += delta;
            _script?.Run(delta);
        }

        public void Render(Layer layer, State state)
        {
            GLLayer.Render(state, layer, this);
        }

        private string _name;
        protected GLMesh _geometry;
        protected Geometry _mesh;
        protected Material _material;
        protected Transform _transform;
        protected IScript _script;
        private DisplayType _diplayMode = DisplayType.DISPLAY_SOLID;
        private float _time;
    }
}
