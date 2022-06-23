using SharpTracer.Base;
using System.Collections.Generic;
using System.Linq;

namespace SharpTracer.View.Controls
{
	public class RibbonVM : NotificationBase
    {
        public delegate void refresh();
        private Dictionary<string, RibbonGroupVM> _groups;
        public List<RibbonGroupVM> Groups
        {
            get
            {
                return new List<RibbonGroupVM>(_groups.Select(x => (x.Value)));
            }
        }

        public refresh Refresh
        {
            get;
            internal set;
        }

        public RibbonVM()
        {
            _groups = new Dictionary<string, RibbonGroupVM>();
        }

        public void Add(RibbonCommandVM command)
        {
            if (!_groups.ContainsKey(command.Group)) _groups.Add(command.Group, new RibbonGroupVM());

            _groups[command.Group].Add(command);
        }

        public void RefreshGroups()
        {
            NotifyPropertyChanged(nameof(Groups));
        }

    }
}