using System.Collections.Generic;

namespace SharpTracer.View.Controls
{
	public class RibbonGroupVM
    {
        public string Title { get; set; }
        public  List<RibbonCommandVM> Commands { get; set; }

        public RibbonGroupVM()
        {
            Commands = new List<RibbonCommandVM>();
        }

        public void Add(RibbonCommandVM command)
        {
            Title = command.Group;
            Commands.Add(command);
        }
    }
}
