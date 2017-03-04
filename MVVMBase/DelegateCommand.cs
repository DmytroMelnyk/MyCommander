using System;
using MyCommander;

namespace MyCommander
{
    public sealed class DelegateCommand : DelegateCommandBase
    {
        public DelegateCommand(Action executeMethod)
          : base(_ => executeMethod(), null)
        {
        }
    }
}
