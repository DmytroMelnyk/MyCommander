using System;
using System.Windows.Input;

namespace MyCommander
{
    public class DelegateCommandBase : ICommand
    {
        private Action<object> executeMethod;
        private Predicate<object> canExecuteMethod;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (this.canExecuteMethod != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (this.canExecuteMethod != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        protected DelegateCommandBase(Action<object> executeMethod, Predicate<object> canExecuteMethod = null)
        {
            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecuteMethod == null || this.canExecuteMethod(parameter);
        }

        public void Execute(object parameter)
        {
            this.executeMethod(parameter);
        }
    }
}
