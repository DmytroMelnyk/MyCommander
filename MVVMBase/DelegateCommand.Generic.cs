using System;

namespace MyCommander
{
    public class DelegateCommand<T> : DelegateCommandBase
    {
        public DelegateCommand(Action<T> executeMethod)
          : base(o => executeMethod((T)o), null)
        {
        }

        public DelegateCommand(Action<T> executeMethod, Predicate<T> canExecuteMethod = null)
          : base(o => executeMethod((T)o), o => canExecuteMethod((T)o))
        {
        }
    }
}
