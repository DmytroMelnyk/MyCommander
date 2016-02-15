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
            DependencyProperty.Register("FirstTab", typeof(FrameworkElement), typeof(TotalContainer), new PropertyMetadata((d, e) =>
            {
                TotalContainer @this = (TotalContainer)d;
                @this.AssignEvents(e.OldValue as FrameworkElement, e.NewValue as FrameworkElement, (s, _) =>
                {
                    if (@this.SelectedTab == null || @this.SelectedTab == @this.SecondTab)
                        @this.SelectedTab = @this.FirstTab;
                });
            }));

        public FrameworkElement SecondTab
        {
            get { return (FrameworkElement)GetValue(SecondTabProperty); }
            set { SetValue(SecondTabProperty, value); }
        }

        public static readonly DependencyProperty SecondTabProperty =
            DependencyProperty.Register("SecondTab", typeof(FrameworkElement), typeof(TotalContainer), new PropertyMetadata((d, e) =>
            {
                TotalContainer @this = (TotalContainer)d;
                @this.AssignEvents(e.OldValue as FrameworkElement, e.NewValue as FrameworkElement, (s, _) =>
                {
                    if (@this.SelectedTab == null || @this.SelectedTab == @this.FirstTab)
                        @this.SelectedTab = @this.SecondTab;
                });
            }));

        private void AssignEvents(FrameworkElement oldFrameworkElement, FrameworkElement newFrameworkElement, MouseButtonEventHandler previewMouseDown)
        {
            if (oldFrameworkElement != null)
                oldFrameworkElement.PreviewMouseDown -= previewMouseDown;

            if (newFrameworkElement != null)
                newFrameworkElement.PreviewMouseDown += previewMouseDown;
        }
    }
}
