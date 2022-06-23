using System;

namespace SharpTracer.View.Controls
{
	public interface ITabPage
	{
		string Visibility
		{ get; set; }
		string Title
		{
			get;
		}
		string Icon
		{
			get;
			set;
		}

		RibbonVM Ribbon
		{
			get;
			set;
		}
		ExpandablePanelVM Parent
		{
			get;
			set;
		}

		Object Content
		{
			get;
		}

		void Update();
	}
}
