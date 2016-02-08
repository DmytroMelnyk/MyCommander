using System;
using System.Collections;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace CommanderControl
{
    public partial class CommanderTab : UserControl
    {
        public CommanderTab()
        {
            InitializeComponent();
        }

        public ICommand ChangeDiskCommand
        {
            get { return (ICommand)GetValue(ChangeDiskCommandProperty); }
            set { SetValue(ChangeDiskCommandProperty, value); }
        }

        public static readonly DependencyProperty ChangeDiskCommandProperty =
            DependencyProperty.Register("ChangeDiskCommand", typeof(ICommand), typeof(CommanderTab));

        public IEnumerable DiskCollection
        {
            get { return (IEnumerable)GetValue(DiskCollectionProperty); }
            set { SetValue(DiskCollectionProperty, value); }
        }

        public static readonly DependencyProperty DiskCollectionProperty =
            DependencyProperty.Register("DiskCollection", typeof(IEnumerable), typeof(CommanderTab));
    }
}
