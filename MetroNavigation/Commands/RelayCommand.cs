using System;
using System.Windows.Input;

namespace MetroNavigation.Commands
{
    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<bool> canExecute;

        public RelayCommand(Action<object> execute, Func<bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute();
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}
