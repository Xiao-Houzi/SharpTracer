using SharpTracer.Base;
using SharpTracer.Engine.Scene;
using SharpTracer.View.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTracer.ViewModels
{
    internal class EntityVM : NotificationBase, ITabPage
    {
        public string Visibility { get ; set; }
        public string Title => _title;
        public string Icon { get ; set ; }
        public RibbonVM Ribbon { get; set; }
        public ExpandablePanelVM Parent { get; set; }

        public object Content => this;

        public EntityVM()
        {
            _entity = null;


            Ribbon = new RibbonVM();
            _title = "Geometry View";
            Icon = "";
            Ribbon.Add(new RibbonCommandVM("", "Buttons", "Button", "", new Command(() => true, execute)));
            Ribbon.Add(new RibbonCommandVM("", "Buttons", "Button", "", new Command(() => true, execute)));
            Ribbon.Add(new RibbonCommandVM("", "others", "Button", "", new Command(() => true, execute)));
        }

        public void SetEntity(Entity entity)
        {
            _entity = entity;
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
        }

        private Entity _entity;
        private string _title;
    }
}
