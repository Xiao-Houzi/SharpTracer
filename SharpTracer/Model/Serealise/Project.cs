using System;
using System.IO;
using SharpEngine.Engine;
using SharpEngine.Maths;
using SharpTracer.Base;
using SharpTracer.Engine.Scene;
using System.Collections.Generic;
using SharpTracer.Engine;
using SharpTracer.Model.Base.Messaging;
using Range = SharpEngine.Maths.Range;
using SharpTracer.Engine.GLAbstraction;

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
        public uint FrameWidth
        { get; set; }
        public uint FrameHeight
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
            _sceneCamera = new Camera();
            _entities = new List<Entity>();
            kdTree = new KdTree(3, new Range[] { new Range(-10, 10), new Range(-10, 10), new Range(-2, 10) }, 20);

            Messenger.ModelEvent += ModelEventHandler;
        }

        private void ModelEventHandler(object sender, ModelArgs args)
        {
            switch (args.Reason)
            {
                case EventReason.LoadProject:
                    Deserialise();
                    break;

                case EventReason.SaveProject:
                    Serialise();
                    break;

                case EventReason.ResetViewCamera:
                    ResetViewCamera();
                    break;

                case EventReason.AddedEntity:
                    AddEntity((Geometry)args.DataObject);
                    break;
            }
        }

        public void Load()
        {
            Name = "New Scene";
        }

        #region private
        private List<Entity> _entities;
        private Camera _viewCamera;
        private Camera _sceneCamera;
        private Entity _currentEntity;
        private KdTree kdTree;

        private void ResetViewCamera()
        {
            _viewCamera.Reset();
        }
        private void Serialise()
        {
            throw new NotImplementedException();
        }
        private void Deserialise()
        {
            throw new NotImplementedException();
        }
        private void AddEntity(Geometry geometry)
        {
            
            int number = 0;
            Entity entity = new Entity($"{geometry.Name}{number}", geometry, new Material());

            Entities.Add(entity);
        }
        #endregion
    }
}
