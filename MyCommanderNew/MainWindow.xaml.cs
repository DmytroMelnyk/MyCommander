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

namespace MyCommanderNew
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var sp = new StackPanel();
            sp.Children.Add(new TextBlock { Text = "text1" });
            sp.Children.Add(new Button { Content = "btn1" });
            totCom.FirstTab = sp;

            totCom.SecondTab = new Button { Content = "Button2" };
        }
    }
}
