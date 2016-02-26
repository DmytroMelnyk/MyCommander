using System;
using MyCommander;

namespace MyCommander
{
    public class DelegateCommand : DelegateCommandBase
    {
        public DelegateCommand(Action executeMethod)
          : base(_ => executeMethod(), null)
        {
        }
    }
}
