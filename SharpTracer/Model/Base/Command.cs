using System;
using System.Windows.Input;

namespace SharpTracer.Base
{
    public class Command : ICommand
    {
        #region ICommand Members
        public delegate bool CanExecuteDelegate( );
        public delegate void ExecuteDelegate(object Parameter);

        private CanExecuteDelegate _canExecute;
        private ExecuteDelegate _execute;

        public String Name { get; }

        public Command(CanExecuteDelegate canExecute, ExecuteDelegate execute, String name ="Command")
        {
			Name = name;
            _canExecute = canExecute;
            _execute = execute;
        }

		public bool CanExecute(object parameter )
        {
            return _canExecute();
        }
        public event System.EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

   

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
        #endregion
    }
}
