using SharpTracer.Base;
using SharpTracer.Model.Events;
using SharpTracer.View;
using SharpTracer.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SharpTracer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
		SharpMessenger messenger;
		public DiagnosticLog log;
		public string StartUpFile;
		public string logName = "DiagnosticLog.txt";

		void App_Startup(object sender, StartupEventArgs startupEventArgs)
		{
			try
			{
				messenger = new SharpMessenger();
				if (startupEventArgs.Args.Length > 0) StartUpFile = startupEventArgs.Args[0];
				log = new DiagnosticLog();
				//log.Path = "D:/";
				log.Initialise(new Uri(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"D:\", "DiagnosticLog.txt")));

				SharpTracerModel modelController = new SharpTracerModel(logName);

				MainWindowVM mainVM = new MainWindowVM(modelController);
				MainWindow window = new MainWindow() { DataContext = mainVM };

				window.Show();
			}
			catch (Exception e)
			{
				MessageBox.Show("Something serious has happened and prevented the app from starting properly: \n\n" + e.Message + "\n\n" + e.InnerException?.Message + "\n\n" + e.StackTrace, "App Exception");
			}
		}
	}
}
