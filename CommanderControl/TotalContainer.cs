using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace CommanderControl
{
    public class TotalContainer : Control
    {
        static TotalContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TotalContainer), new FrameworkPropertyMetadata(typeof(TotalContainer)));
        }

        public TotalContainer()
        {
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == FirstTabProperty)
            {
                ((FrameworkElement)e.NewValue).GotFocus += FirstTabFocus;
            }
            else if (e.Property == SecondTabProperty)
            {
                ((FrameworkElement)e.NewValue).GotFocus += SecondTabFocus;
            }
            base.OnPropertyChanged(e);
        }

        private void FirstTabFocus(object sender, RoutedEventArgs e)
        {
            SelectedTab = FirstTab;
        }

        private void SecondTabFocus(object sender, RoutedEventArgs e)
        {
            SelectedTab = SecondTab;
        }

        public FrameworkElement SelectedTab
        {
            get { return (FrameworkElement)GetValue(SelectedTabProperty); }
            set { SetValue(SelectedTabProperty, value); }
        }

        public static readonly DependencyProperty SelectedTabProperty =
            DependencyProperty.Register("SelectedTab", typeof(FrameworkElement), typeof(TotalContainer));

        public FrameworkElement FirstTab
        {
            get { return (FrameworkElement)GetValue(FirstTabProperty); }
            set { SetValue(FirstTabProperty, value); }
        }

        public static readonly DependencyProperty FirstTabProperty =
            DependencyProperty.Register("FirstTab", typeof(FrameworkElement), typeof(TotalContainer));

        public FrameworkElement SecondTab
        {
            get { return (FrameworkElement)GetValue(SecondTabProperty); }
            set { SetValue(SecondTabProperty, value); }
        }

        public static readonly DependencyProperty SecondTabProperty =
            DependencyProperty.Register("SecondTab", typeof(FrameworkElement), typeof(TotalContainer));
    }
}
