using System;

namespace MyCommander
{
    public sealed class DelegateCommand<T> : DelegateCommandBase
    {
        public DelegateCommand(Action<T> executeMethod)
          : base(o => executeMethod((T)o), null)
        {
        }

        public DelegateCommand(Action<T> executeMethod, Predicate<T> canExecuteMethod)
          : base(o => executeMethod((T)o), o => canExecuteMethod((T)o))
        {
        }
    }
}
