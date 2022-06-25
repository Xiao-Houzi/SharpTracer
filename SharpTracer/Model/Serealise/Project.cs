using SharpEngine.Engine;
using SharpEngine.Maths;
using SharpTracer;
using SharpTracer.Base;
using SharpTracer.Engine;
using SharpTracer.Engine.Scene;
using SharpTracer.Scripting.States;
using System;
using System.Collections.Generic;
using System.IO;
using Range = SharpEngine.Maths.Range;

namespace SharpTracer.Model
{
    public class Project : NotificationBase
    {
        private static Entity _currentEntity;
        private KdTree kdTree;

        public List<Entity> Entities
        {
            get; set;
        }
        public Entity CurrentEntity
        {
            get
            {
                return _currentEntity;
            }
            set
            {
                _currentEntity = value;
            }
        }
        public State ProjectState
        {
            get;
            set;
        }
        public String Name
        {
            get; set;
        }

        public Project()
        {
            Entities = new List<Entity>();
            ProjectState = new ProjectState();
            kdTree = new KdTree(3, new Range[] { new Range(-10, 10), new Range(-10, 10), new Range(-2, 10) }, 20);

            // add the action delegates
        }

        public void Load(Uri path, Type script)
        {
            Entity cloud = new Entity();

            Entities.Add(cloud);
            CurrentEntity = Entities[Entities.IndexOf(cloud)];
            String[] files;
            string layer;

            // load a projrct object
            files = Directory.GetFiles(path.LocalPath, "*.ptx");
            layer = "Model";

            Name = path.LocalPath;
        }

        public void Update(Scene scene, Type script)
        {
            string layer = "Model";
            ProjectState.Layers[layer].Clear();
            foreach (Entity v in scene.Entities)
            {
                ProjectState.Layers[layer].AddEntity(v);
            }
        }

        internal void ExportData()
        {
            throw new NotImplementedException();
        }
    }
}
