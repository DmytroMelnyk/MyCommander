using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    [TemplatePart(Name = PARTID_Tab1, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = PARTID_Tab2, Type = typeof(ContentPresenter))]
    public class TotalContainer : Control
    {
        ContentPresenter _tab1;
        ContentPresenter _tab2;

        private const string PARTID_Tab1 = "PART_Tab1";
        private const string PARTID_Tab2 = "PART_Tab2";

        public override void OnApplyTemplate()
        {
            _tab1 = GetTemplateChild(PARTID_Tab1) as ContentPresenter;
            WeakEventManager<ContentPresenter, MouseButtonEventArgs>.AddHandler(_tab1, nameof(_tab1.PreviewMouseDown), (s, e) =>
            {
                if (SelectedTabContent == null || SelectedTabContent == SecondTabContent)
                    SelectedTabContent = FirstTabContent;
            });

            _tab2 = GetTemplateChild(PARTID_Tab2) as ContentPresenter;
            WeakEventManager<ContentPresenter, MouseButtonEventArgs>.AddHandler(_tab2, nameof(_tab2.PreviewMouseDown), (s, e) =>
            {
                if (SelectedTabContent == null || SelectedTabContent == FirstTabContent)
                    SelectedTabContent = SecondTabContent;
            });
        }

        public static readonly DependencyProperty TabTemplateProperty =
            DependencyProperty.Register("TabTemplate", typeof(DataTemplate), typeof(TotalContainer));

        public DataTemplate TabTemplate
        {
            get { return (DataTemplate)GetValue(TabTemplateProperty); }
            set { SetValue(TabTemplateProperty, value); }
        }

        static TotalContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TotalContainer), new FrameworkPropertyMetadata(typeof(TotalContainer)));
        }

        public object SelectedTabContent
        {
            get { return (object)GetValue(SelectedTabContentProperty); }
            set { SetValue(SelectedTabContentProperty, value); }
        }

        public static readonly DependencyProperty SelectedTabContentProperty =
            DependencyProperty.Register("SelectedTabContent", typeof(object), typeof(TotalContainer));

        public object FirstTabContent
        {
            get { return (object)GetValue(FirstTabContentProperty); }
            set { SetValue(FirstTabContentProperty, value); }
        }

        public static readonly DependencyProperty FirstTabContentProperty =
            DependencyProperty.Register("FirstTabContent", typeof(object), typeof(TotalContainer));

        public object SecondTabContent
        {
            get { return (object)GetValue(SecondTabContentProperty); }
            set { SetValue(SecondTabContentProperty, value); }
        }

        public static readonly DependencyProperty SecondTabContentProperty =
            DependencyProperty.Register("SecondTabContent", typeof(object), typeof(TotalContainer));
    }
}
