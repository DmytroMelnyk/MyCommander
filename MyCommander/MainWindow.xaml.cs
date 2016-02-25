using MyCommander.UserControls;
using System.Windows;

namespace MyCommander
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CommanderTab ActiveTab
        {
            get { return (CommanderTab)this.GetValue(ActiveTabProperty); }
            set { this.SetValue(ActiveTabProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActiveTab.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActiveTabProperty =
            DependencyProperty.Register("ActiveTab", typeof(CommanderTab), typeof(MainWindow), new PropertyMetadata(null));

        public MainWindow()
        {
            this.InitializeComponent();
        }
    }
}
