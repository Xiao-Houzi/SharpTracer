using System.Windows.Controls;
using System.Windows.Media;

namespace SharpTracer.View.Controls
{
   /// <summary>
   /// Interaction logic for AelfcraeftLogo.xaml
   /// </summary>
   public partial class AelfcraeftLogo : UserControl
   {

      public bool Clipped { set { if (value) VB.Height = 90; LogoCanvas.RenderTransform = new TranslateTransform(0, -75); } }
      public AelfcraeftLogo()
      {
         InitializeComponent();
      }
   }
}
