using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyCommander
{
    public static class CustomCommands
    {
        public static readonly RoutedUICommand Exit = new RoutedUICommand
            (
            "Exit",
            "Exit",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F4, ModifierKeys.Alt)
            }
            );

        public static readonly RoutedUICommand ChangeDisk = new RoutedUICommand
        (
        "ChangeDisk",
        "ChangeDisk",
        typeof(CustomCommands)
        );
        //Define more commands here, just like the one above
    }
}
