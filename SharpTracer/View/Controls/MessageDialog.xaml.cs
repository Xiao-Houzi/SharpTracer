using System.Windows;


namespace SharpTracer.View.Controls
{
    /// <summary>
    /// Interaction logic for MessageDialog.xaml
    /// </summary>
    public partial class MessageDialog : Window
    {
        public MessageDialog()
        {
            InitializeComponent();
        }

        public static void Show(string title, string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageDialog dialog = new MessageDialog();
                dialog.Title = title;
                dialog.ContentText.Text = ConvertNewlines(message);
                dialog.ShowDialog();
            });
        }

        private static string ConvertNewlines(string input)
        {
            return input.Replace(@"\n", @"&#10;");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}