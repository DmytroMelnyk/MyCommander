using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace MyCommander.UserControls
{
    /// <summary>
    /// Interaction logic for CommanderTab.xaml
    /// </summary>
    public partial class CommanderTab : UserControl
    {
        public CommanderTab()
        {
            InitializeComponent();
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var aaa = this.Name;
            ListView lv = e.Source as ListView;
            if (lv == null)
                return;

            FileSystemInfoWrapper fdi = lv.SelectedItem as FileSystemInfoWrapper;
            if (fdi == null)
                return;

            if (fdi.IsDirectory)
            {
                Presenter presenter = (Presenter)DataContext;
                presenter.CurrentDirectory = fdi.FullName;
            }
            else
            {
                Process.Start(fdi.FullName);
            }
        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            FileSystemInfoWrapper fdi = (FileSystemInfoWrapper)e.Item;
            e.Accepted = true;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Presenter presenter = (Presenter)DataContext;
            presenter.CurrentDirectory = (string)e.Parameter;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new Presenter("C:\\");
        }
    }
}
