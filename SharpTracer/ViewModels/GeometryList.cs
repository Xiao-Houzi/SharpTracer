using SharpTracer.Base;
using SharpTracer.Engine.GLAbstraction;
using SharpTracer.Engine.Scene;
using SharpTracer.Engine.Scene.RenderGeometry;
using SharpTracer.Model.Base.Messaging;
using SharpTracer.View.Controls;
using System;
using System.Collections.Generic;

namespace SharpTracer.ViewModels
{
    public class GeometryList : NotificationBase, ITabPage
    {

        public string Title => _title;

        public string Icon
        {
            get;
            set;
        }
        public string Visibility
        {
            get;
            set;
        }
        public SharpTracerModel Model
        {
            get;
        }
        public RibbonVM Ribbon { get; set; }

        public object Content
        {
            get
            {
                return this;
            }
        }


        public List<Entity> Entities
        {
            get
            {
                List<Entity> entites = new List<Entity>();
                foreach (Entity l in Model.Entities)
                    entites.AddRange(l.FetchDescendants());
                return entites;
            }
        }

        public Entity SelectedEntity
        {
            get
            {
                Event.UI(this, EventReason.EntitySelected, SharpTracerModel.SelectedEntity);
                return SharpTracerModel.SelectedEntity;
            }
            set
            {
                SharpTracerModel.SelectedEntity = value;
                NotifyPropertyChanged();
            }
        }

        public ExpandablePanelVM Parent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public GeometryList(SharpTracerModel model)
        {
            Model = model;
            Ribbon = new RibbonVM();
            _content = new GeometryListView() { DataContext = this };
            _title = "Entity List";
            Icon = "";
            Ribbon.Add(new RibbonCommandVM("Entity", "Add\nSphere", "X", "", new Command(() => true, (x)=>RaiseEvent.UI(this, EventReason.CommandAddEntity, typeof(MeshSphere)))));
            Ribbon.Add(new RibbonCommandVM("Entity", "Add\nCube", "X", "", new Command(() => true, (x) => RaiseEvent.UI(this, EventReason.CommandAddEntity, typeof(MeshSphere)))));
            Ribbon.Add(new RibbonCommandVM("Entity", "Add", "X", "", new Command(() => true, (x) => RaiseEvent.UI(this, EventReason.CommandAddEntity, typeof(MeshSphere)))));

            Messenger.ModelEvent += SharpTracerMessenger_ModelEvent;
        }

        private void SharpTracerMessenger_ModelEvent(object sender, ModelArgs args)
        {
            switch (args.Reason)
            {
                case EventReason.AddedEntity:
                    NotifyPropertyChanged(nameof(Geometry));
                    break;
            }
        }


        public void Update()
        {
            throw new NotImplementedException();
        }

        private object _content;
        private string _title;

    }
}
