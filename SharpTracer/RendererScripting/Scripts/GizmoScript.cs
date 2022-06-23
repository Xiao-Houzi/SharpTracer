using SharpEngine.Engine.Graphics;
using GlmSharp;
using System;

namespace SharpTracer.RendererScripting.Scripts
{
	public class GizmoScript : IScript
	{
		Entity Gizmo;


		public bool Highlight
		{
			get; set;
		}


		void IScript.Initialise(Entity gizmo)
		{
			Gizmo = (Entity)gizmo;
		}

		void IScript.Run(float delta)
		{
			if(Gizmo == null) return;
		}

		void IScript.Destroy()
		{
		}

	}
}