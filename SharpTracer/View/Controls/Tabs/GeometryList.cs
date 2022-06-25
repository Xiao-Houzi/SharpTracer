using SharpEngine.Engine.Graphics;
using SharpTracer.Base;
using SharpTracer.Engine.Scene;
using SharpTracer.Model.Events;
using System;
using System.Collections.Generic;

namespace SharpTracer.View.Controls
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
					foreach(Entity l in Model.Geometry)
					entites.AddRange(l.FetchDescendants());
				return entites;
			}
		}

		public Entity SelectedEntity
		{
			get
			{
				SharpTracerEvent.UI(this, SharpTracerUIArgs.EventReason.EntitySelected, SharpTracerModel.SelectedEntity);
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
			_title = "Geometry View";
			Icon = "";
			Ribbon.Add(new RibbonCommandVM("", "X", "X", "", new Command(()=>true, execute)));

			SharpMessenger.ModelEvent += SharpTracerMessenger_ModelEvent;
		}

		private void SharpTracerMessenger_ModelEvent(object sender, SharpTracerModelArgs args)
		{
			switch(args.Reason)
			{
				case SharpTracerModelArgs.EventReason.EntitysUpdated:
					NotifyPropertyChanged(nameof(GLMesh));
					break;
			}
		}

		private void execute(object Parameter)
		{
			MessageDialog dlg = new MessageDialog()
			{
				Content = "Don't just press weird buttons, are you mad?"
			};
			dlg.Show();
		}

        public void Update()
        {
            throw new NotImplementedException();
        }

        private object _content;
		private string _title;

	}
}
