using SharpEngine.Engine;
using SharpEngine.Engine.Graphics;
using SharpEngine.Maths;
using SharpTracer;
using SharpTracer.Base;
using SharpTracer.RendererScripting.States;
using System;
using System.Collections.Generic;
using System.IO;
using Range = SharpEngine.Maths.Range;

namespace SharpTracer.Model
{
	public enum FileType
	{
		Bin, PTX
	}
	public class ProjectLoadOptions
	{
		public FileType filetype;
	}

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
			kdTree = new KdTree(3, new Range[]{ new Range(-10,10), new Range(-10, 10), new Range(-2, 10) }, 20);

			// add the action delegates
		}

		public void Load(Uri path, Type script, ProjectLoadOptions option)
		{
			Entity cloud = new Entity();

			Entities.Add(cloud);
			CurrentEntity = Entities[Entities.IndexOf(cloud)];
			String[] files;
			string layer;

			switch(option.filetype)
			{
				case FileType.Bin:
					
					files = Directory.GetFiles(path.AbsolutePath, "*.ccd");
					layer = "Model";
					break;
				case FileType.PTX:
					// load a cloud object
					files = Directory.GetFiles(path.LocalPath, "*.ptx");
					layer = "Model";
					break;
				default:
					files = new string[0];
					layer = "Model";
					break;
			}

			
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
