using MyCommander.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MyCommander
{
    class MyTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CurrentDirectory { get; set; }
        public DataTemplate Directory { get; set; }
        public DataTemplate File { get; set; }

        public override DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            FileSystemInfoWrapper it = item as FileSystemInfoWrapper;
            if (it != null)
            {
                if (it.IsCurrentDirectory.HasValue)
                {
                    if (it.IsCurrentDirectory.Value)
                        return CurrentDirectory;
                    else
                        return Directory;
                }
                return File;
            }
            return base.SelectTemplate(item, container);
        }
    }
}
