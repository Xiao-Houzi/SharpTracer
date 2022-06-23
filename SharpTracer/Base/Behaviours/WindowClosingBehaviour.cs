using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace SharpTracer.Base.Behaviours
{
	public class WindowClosingBehavior
	{
		public static ICommand GetClosing(DependencyObject obj)
		{
			return (ICommand)obj.GetValue(ClosingProperty);
		}
		public static ICommand GetClosed(DependencyObject obj)
		{
			return (ICommand)obj.GetValue(ClosedProperty);
		}
		public static ICommand GetCancelClosing(DependencyObject obj)
		{
			return (ICommand)obj.GetValue(CancelClosingProperty);
		}

		public static void SetClosing(DependencyObject obj, ICommand value)
		{
			obj.SetValue(ClosingProperty, value);
		}
		public static void SetClosed(DependencyObject obj, ICommand value)
		{
			obj.SetValue(ClosedProperty, value);
		}
		public static void SetCancelClosing(DependencyObject obj, ICommand value)
		{
			obj.SetValue(CancelClosingProperty, value);
		}

		public static readonly DependencyProperty ClosingProperty = DependencyProperty.RegisterAttached(
			"Closing", typeof(ICommand), typeof(WindowClosingBehavior),
			new UIPropertyMetadata(new PropertyChangedCallback(ClosingChanged)));

		public static readonly DependencyProperty ClosedProperty = DependencyProperty.RegisterAttached(
			"Closed", typeof(ICommand), typeof(WindowClosingBehavior),
			new UIPropertyMetadata(new PropertyChangedCallback(ClosedChanged)));

		public static readonly DependencyProperty CancelClosingProperty = DependencyProperty.RegisterAttached(
			"CancelClosing", typeof(ICommand), typeof(WindowClosingBehavior));

		private static void ClosingChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
		{
			Window window = target as Window;

			if (window != null)
			{
				if (e.NewValue != null)
				{
					window.Closing += Window_Closing;
				}
				else
				{
					window.Closing -= Window_Closing;
				}
			}
		}

		private static void ClosedChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
		{
			Window window = target as Window;

			if (window != null)
			{
				if (e.NewValue != null)
				{
					window.Closed += Window_Closed;
				}
				else
				{
					window.Closed -= Window_Closed;
				}
			}
		}
		
		static void Window_Closing(object sender, CancelEventArgs e)
		{
			ICommand closing = GetClosing(sender as Window);
			if (closing != null)
			{
				if (closing.CanExecute(null))
				{
					closing.Execute(null);
				}
				else
				{
					ICommand cancelClosing = GetCancelClosing(sender as Window);
					if (cancelClosing != null)
					{
						cancelClosing.Execute(null);
					}

					e.Cancel = true;
				}
			}
		}

		static void Window_Closed(object sender, EventArgs e)
		{
			ICommand closed = GetClosed(sender as Window);
			if (closed != null)
			{
				closed.Execute(null);
			}
		}

	}
}
