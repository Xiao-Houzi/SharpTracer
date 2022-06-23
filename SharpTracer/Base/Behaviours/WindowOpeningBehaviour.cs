using System;
using System.Windows;
using System.Windows.Input;

namespace SharpTracer.Base.Behaviours
{
	public class WindowLoadingBehavior
	{
		public static ICommand GetLoaded(DependencyObject obj)
		{
			return (ICommand)obj.GetValue(LoadedProperty);
		}

		public static void SetLoaded(DependencyObject obj, ICommand value)
		{
			obj.SetValue(LoadedProperty, value);
		}

		public static readonly DependencyProperty LoadedProperty = DependencyProperty.RegisterAttached(
			"Loaded", typeof(ICommand), typeof(WindowLoadingBehavior),
			new UIPropertyMetadata(new PropertyChangedCallback(LoadedChanged)));

		private static void LoadedChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
		{
			Window window = target as Window;

			if (window != null)
			{
				if (e.NewValue != null)
				{
					window.Loaded += Window_Loaded;
				}
				else
				{
					window.Loaded -= Window_Loaded;
				}
			}
		}

		static void Window_Loaded(object sender, EventArgs e)
		{
			ICommand closed = GetLoaded(sender as Window);
			if (closed != null)
			{
				closed.Execute(sender);
			}
		}

	}
}
