using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        public bool IsActiveTab
        {
            get { return (bool)GetValue(IsActiveTabProperty); }
            set { SetValue(IsActiveTabProperty, value); }
        }

        public static readonly DependencyProperty IsActiveTabProperty =
            DependencyProperty.Register("IsActiveTab", typeof(bool), typeof(CommanderTab), new PropertyMetadata(false));

        public CommanderTab()
        {
            InitializeComponent();
        }
    }
}
