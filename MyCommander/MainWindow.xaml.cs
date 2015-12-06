using MyCommander.UserControls;
using System;
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

namespace MyCommander
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CommanderTab ActiveTab
        {
            get { return (CommanderTab)GetValue(ActiveTabProperty); }
            set { SetValue(ActiveTabProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActiveTab.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActiveTabProperty =
            DependencyProperty.Register("ActiveTab", typeof(CommanderTab), typeof(MainWindow), new PropertyMetadata(null));

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
