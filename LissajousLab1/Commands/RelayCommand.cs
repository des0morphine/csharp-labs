using System;
using System.Windows.Input;

namespace LissajousLab1.Commands
{
    public class RelayCommand : ICommand
    {
        private Action<object?> executeAction;
        private Func<object?, bool>? canExecuteFunc;

        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            executeAction = execute;
            canExecuteFunc = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            if (canExecuteFunc != null)
            {
                return canExecuteFunc(parameter);
            }
            return true;
        }

        public void Execute(object? parameter)
        {
            if (executeAction != null)
            {
                executeAction(parameter);
            }
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
