using System.Windows.Controls;

namespace SharpTracer.View.Controls
{
    /// <summary>
    /// Interaction logic for ControlRibbon.xaml
    /// </summary>
    public partial class ControlRibbon : UserControl
    {
        public ControlRibbon()
        {
            InitializeComponent();
        }

        void Refresh()
        {
            InvalidateVisual();
        }
    }
}
