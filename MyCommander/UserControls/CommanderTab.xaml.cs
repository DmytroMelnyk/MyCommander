using System.Windows;
using System.Windows.Controls;

namespace MyCommander.UserControls
{
    /// <summary>
    /// Interaction logic for CommanderTab.xaml
    /// </summary>
    public partial class CommanderTab : UserControl
    {
        public bool IsActiveTab
        {
            get { return (bool)this.GetValue(IsActiveTabProperty); }
            set { this.SetValue(IsActiveTabProperty, value); }
        }

        public static readonly DependencyProperty IsActiveTabProperty =
            DependencyProperty.Register("IsActiveTab", typeof(bool), typeof(CommanderTab), new PropertyMetadata(false));

        public CommanderTab()
        {
            this.InitializeComponent();
        }
    }
}
