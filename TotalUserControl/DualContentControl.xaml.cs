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

namespace TotalUserControl
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class DualContentControl : UserControl
    {
        public static readonly DependencyProperty ContentOneProperty = DependencyProperty.Register("ContentOne", typeof(object), typeof(DualContentControl));
        public static readonly DependencyProperty ContentOneTemplateProperty = DependencyProperty.Register("ContentOneTemplate", typeof(DataTemplate), typeof(DualContentControl));
        public static readonly DependencyProperty ContentOneTemplateSelectorProperty = DependencyProperty.Register("ContentOneTemplateSelector", typeof(DataTemplateSelector), typeof(DualContentControl));

        public static readonly DependencyProperty ContentTwoProperty = DependencyProperty.Register("ContentTwo", typeof(object), typeof(DualContentControl));
        public static readonly DependencyProperty ContentTwoTemplateProperty = DependencyProperty.Register("ContentTwoTemplate", typeof(DataTemplate), typeof(DualContentControl));
        public static readonly DependencyProperty ContentTwoTemplateSelectorProperty = DependencyProperty.Register("ContentTwoTemplateSelector", typeof(DataTemplateSelector), typeof(DualContentControl));


        public DualContentControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        public object ContentOne
        {
            get { return GetValue(ContentOneProperty); }
            set { SetValue(ContentOneProperty, value); }
        }

        public DataTemplate ContentOneTemplate
        {
            get { return (DataTemplate)GetValue(ContentOneTemplateProperty); }
            set { SetValue(ContentOneTemplateProperty, value); }
        }


        public DataTemplateSelector ContentOneTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(ContentOneTemplateSelectorProperty); }
            set { SetValue(ContentOneTemplateSelectorProperty, value); }
        }

        public object ContentTwo
        {
            get { return GetValue(ContentTwoProperty); }
            set { SetValue(ContentTwoProperty, value); }
        }

        public DataTemplate ContentTwoTemplate
        {
            get { return (DataTemplate)GetValue(ContentTwoTemplateProperty); }
            set { SetValue(ContentTwoTemplateProperty, value); }
        }


        public DataTemplateSelector ContentTwoTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(ContentTwoTemplateSelectorProperty); }
            set { SetValue(ContentTwoTemplateSelectorProperty, value); }
        }
    }
}
