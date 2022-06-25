using System;
using System.Windows.Input;

namespace SharpTracer.Base
{
    public class CommandAction : ICommand
    {
        private Predicate<object> _canExecute;
        private Action<object> _execute;

        public CommandAction(Predicate<object> canExecute, Action<object> execute)
        {
            this._canExecute = canExecute;
            this._execute = execute;
        }

        public event System.EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

    }
}