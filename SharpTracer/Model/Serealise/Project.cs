using System;
using System.IO;
using SharpEngine.Engine;
using SharpEngine.Maths;
using SharpTracer.Base;
using SharpTracer.Engine.Scene;
using System.Collections.Generic;
using Range = SharpEngine.Maths.Range;
using SharpTracer.Engine;

namespace SharpTracer.Model
{
    public class Project : NotificationBase
    {
        public List<Entity> Entities
        {
            get => _entities;
            set => _entities = value;
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
        public Camera SceneCamera
        { get => _sceneCamera; set => _sceneCamera = value; }
        public Camera ViewCamera
        { get => _viewCamera; set => _viewCamera = value; }
        public int FrameWidth
        { get; set; }
        public int FrameHeight
        { get; set; }
        public String Name
        {
            get; set;
        }

        public Project()
        {
            FrameWidth = 1024;
            FrameHeight = 576;
            _viewCamera = new Camera();
            _entities = new List<Entity>();
            kdTree = new KdTree(3, new Range[] { new Range(-10, 10), new Range(-10, 10), new Range(-2, 10) }, 20);
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

        internal void Serialise()
        {
            throw new NotImplementedException();
        }

        #region private
        private List<Entity> _entities;
        private Camera _viewCamera;
        private Camera _sceneCamera;
        private Entity _currentEntity;
        private KdTree kdTree;

        internal void ResetViewCamera()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
