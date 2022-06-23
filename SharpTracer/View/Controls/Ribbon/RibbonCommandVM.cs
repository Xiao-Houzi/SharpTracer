using SharpTracer.Base;
using System.Windows.Input;

namespace SharpTracer.View.Controls
{
    public class RibbonCommandVM :NotificationBase
    {
        private Command _command;

		public string Group { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Tooltip { get; set; }
        public Command Command
        {
            get { return _command;}
            set { _command = value; NotifyPropertyChanged(); }
        }

        public RibbonCommandVM(string group, string title, string image, string tooltip, ICommand command)
        {
            Group = group;
            Title = title;
            Image = image;
            Tooltip = tooltip;
            Command = (Command)command;
        }
	}
}
