using GlmSharp;
using System;
using SharpTracer.Engine.Scene;

namespace SharpTracer.Scripting.Scripts
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