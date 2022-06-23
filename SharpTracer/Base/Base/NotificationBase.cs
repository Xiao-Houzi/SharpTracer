using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace SharpTracer.Base
{
	public class NotificationBase : DependencyObject, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (Application.Current == null) return;
			Application.Current?.Dispatcher.Invoke(
			   () =>
			   {
				   PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			   });
		}

		protected virtual void ExternalPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e?.PropertyName != "")
				NotifyPropertyChanged(e.PropertyName);
		}
	}
}
