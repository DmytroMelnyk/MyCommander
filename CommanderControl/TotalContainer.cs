using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommanderControl
{
    [TemplatePart(Name = PARTIDTab1, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = PARTIDTab2, Type = typeof(ContentPresenter))]
    public class TotalContainer : Control
    {
        public static readonly DependencyProperty TabTemplateProperty =
            DependencyProperty.Register("TabTemplate", typeof(DataTemplate), typeof(TotalContainer));

        public static readonly DependencyProperty SelectedTabContentProperty =
            DependencyProperty.Register("SelectedTabContent", typeof(object), typeof(TotalContainer));

        public static readonly DependencyProperty FirstTabContentProperty =
            DependencyProperty.Register("FirstTabContent", typeof(object), typeof(TotalContainer));

        public static readonly DependencyProperty SecondTabContentProperty =
            DependencyProperty.Register("SecondTabContent", typeof(object), typeof(TotalContainer));

        private const string PARTIDTab1 = "PART_Tab1";
        private const string PARTIDTab2 = "PART_Tab2";
        private ContentPresenter tab1;
        private ContentPresenter tab2;

        static TotalContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TotalContainer), new FrameworkPropertyMetadata(typeof(TotalContainer)));
        }

        public DataTemplate TabTemplate
        {
            get { return (DataTemplate)this.GetValue(TabTemplateProperty); }
            set { this.SetValue(TabTemplateProperty, value); }
        }

        public object SelectedTabContent
        {
            get { return (object)this.GetValue(SelectedTabContentProperty); }
            set { this.SetValue(SelectedTabContentProperty, value); }
        }

        public object FirstTabContent
        {
            get { return (object)this.GetValue(FirstTabContentProperty); }
            set { this.SetValue(FirstTabContentProperty, value); }
        }

        public object SecondTabContent
        {
            get { return (object)this.GetValue(SecondTabContentProperty); }
            set { this.SetValue(SecondTabContentProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            this.tab1 = this.GetTemplateChild(PARTIDTab1) as ContentPresenter;
            WeakEventManager<ContentPresenter, MouseButtonEventArgs>.AddHandler(this.tab1, nameof(ContentPresenter.PreviewMouseDown), (s, e) =>
            {
                if (this.SelectedTabContent == null || this.SelectedTabContent == this.SecondTabContent)
                {
                    this.SelectedTabContent = this.FirstTabContent;
                }
            });

            this.tab2 = this.GetTemplateChild(PARTIDTab2) as ContentPresenter;
            WeakEventManager<ContentPresenter, MouseButtonEventArgs>.AddHandler(this.tab2, nameof(ContentPresenter.PreviewMouseDown), (s, e) =>
            {
                if (this.SelectedTabContent == null || this.SelectedTabContent == this.FirstTabContent)
                {
                    this.SelectedTabContent = this.SecondTabContent;
                }
            });

            base.OnApplyTemplate();
        }
    }
}
