using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace SharpTracer.Base
{
	public class NotifyingUserControl : UserControl, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
		{
			Application.Current?.Dispatcher?.Invoke(
			   () =>
			   {
				   PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			   });
		}
	}
}
