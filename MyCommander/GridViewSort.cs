using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;

namespace Wpf.Util
{
    public class GridViewSort
    {
        #region Attached properties

        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached(
                "Command",
                typeof(ICommand),
                typeof(GridViewSort),
                new UIPropertyMetadata(
                    null,
                    (o, e) =>
                    {
                        ItemsControl listView = o as ItemsControl;
                        if (listView != null && !GetAutoSort(listView))
                        { // Don't change click handler if AutoSort enabled
                            if (e.OldValue != null && e.NewValue == null)
                            {
                                listView.RemoveHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                            }
                            if (e.OldValue == null && e.NewValue != null)
                            {
                                listView.AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                            }
                        }
                    }
                )
            );

        public static bool GetAutoSort(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoSortProperty);
        }

        public static void SetAutoSort(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoSortProperty, value);
        }

        // Using a DependencyProperty as the backing store for AutoSort.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AutoSortProperty =
            DependencyProperty.RegisterAttached(
                "AutoSort",
                typeof(bool),
                typeof(GridViewSort),
                new UIPropertyMetadata(
                    false,
                    (o, e) =>
                    {
                        ListView listView = o as ListView;
                        if (listView != null && GetCommand(listView) == null)
                        { // Don't change click handler if a command is set
                            bool oldValue = (bool)e.OldValue;
                            bool newValue = (bool)e.NewValue;
                            if (oldValue && !newValue)
                            {
                                listView.RemoveHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                            }
                            if (!oldValue && newValue)
                            {
                                listView.AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                            }
                        }
                    }
                )
            );

        public static string GetPropertyName(DependencyObject obj)
        {
            return (string)obj.GetValue(PropertyNameProperty);
        }

        public static void SetPropertyName(DependencyObject obj, string value)
        {
            obj.SetValue(PropertyNameProperty, value);
        }

        // Using a DependencyProperty as the backing store for PropertyName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.RegisterAttached(
                "PropertyName",
                typeof(string),
                typeof(GridViewSort),
                new UIPropertyMetadata(null)
            );

        #endregion

        #region Column header click event handler

        private static void ColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            ListView listView = null;
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            if (headerClicked != null)
            {
                string propertyName = GetPropertyName(headerClicked.Column);
                if (!string.IsNullOrEmpty(propertyName))
                {
                    listView = GetAncestor<ListView>(headerClicked);
                    if (listView != null)
                    {
                        ICommand command = GetCommand(listView);
                        if (command != null && command.CanExecute(propertyName))
                        {
                            command.Execute(propertyName);
                        }
                        else if (GetAutoSort(listView))
                        {
                            // check propertyName equal to null
                            var currentSort = listView.Items.SortDescriptions.FirstOrDefault(current => current.PropertyName == propertyName);
                            ListSortDirection direction = ListSortDirection.Ascending;
                            if (currentSort.PropertyName == null)
                            {
                                var allProperties = (listView.View as GridView).Columns.Select(column => GetPropertyName(column));
                                currentSort = listView.Items.SortDescriptions.Join(allProperties, outerKey => outerKey.PropertyName, innerKey => innerKey, (resSel, _) => resSel).SingleOrDefault();
                            }
                            else
                            {
                                direction = (currentSort.Direction == ListSortDirection.Ascending) ?
                                    ListSortDirection.Descending : ListSortDirection.Ascending;
                            }
                            listView.Items.SortDescriptions.Remove(currentSort);
                            listView.Items.SortDescriptions.Add(new SortDescription(propertyName, direction));
                            //ApplySort(listView.Items, propertyName);
                        }
                    }
                }
            }
            CurrentSortColumnSetGlyph(headerClicked.Column, listView);
        }

        public static void CurrentSortColumnSetGlyph(GridViewColumn gvc, ListView lv)
        {
            ListSortDirection lsd;
            Brush brush;
            if (lv == null)
            {
                lsd = ListSortDirection.Ascending;
                brush = Brushes.Transparent;
            }
            else
            {
                SortDescriptionCollection sdc = lv.Items.SortDescriptions;
                if (sdc == null || sdc.Count < 1) return;
                lsd = sdc[0].Direction;
                brush = Brushes.Gray;
            }

            FrameworkElementFactory fefGlyph =
                new FrameworkElementFactory(typeof(Path));
            fefGlyph.Name = "arrow";
            fefGlyph.SetValue(Path.StrokeThicknessProperty, 1.0);
            fefGlyph.SetValue(Path.FillProperty, brush);
            fefGlyph.SetValue(StackPanel.HorizontalAlignmentProperty,
                HorizontalAlignment.Center);

            int s = 4;
            if (lsd == ListSortDirection.Ascending)
            {
                PathFigure pf = new PathFigure();
                pf.IsClosed = true;
                pf.StartPoint = new Point(0, s);
                pf.Segments.Add(new LineSegment(new Point(s * 2, s), false));
                pf.Segments.Add(new LineSegment(new Point(s, 0), false));

                PathGeometry pg = new PathGeometry();
                pg.Figures.Add(pf);

                fefGlyph.SetValue(Path.DataProperty, pg);
            }
            else
            {
                PathFigure pf = new PathFigure();
                pf.IsClosed = true;
                pf.StartPoint = new Point(0, 0);
                pf.Segments.Add(new LineSegment(new Point(s, s), false));
                pf.Segments.Add(new LineSegment(new Point(s * 2, 0), false));

                PathGeometry pg = new PathGeometry();
                pg.Figures.Add(pf);

                fefGlyph.SetValue(Path.DataProperty, pg);
            }

            FrameworkElementFactory fefTextBlock =
                new FrameworkElementFactory(typeof(TextBlock));
            fefTextBlock.SetValue(TextBlock.HorizontalAlignmentProperty,
                HorizontalAlignment.Center);
            fefTextBlock.SetValue(TextBlock.TextProperty, new Binding());

            FrameworkElementFactory fefDockPanel =
                new FrameworkElementFactory(typeof(StackPanel));
            fefDockPanel.SetValue(StackPanel.OrientationProperty,
                Orientation.Horizontal);
            fefDockPanel.AppendChild(fefTextBlock);
            fefDockPanel.AppendChild(fefGlyph);


            DataTemplate dt = new DataTemplate(typeof(GridViewColumn));
            dt.VisualTree = fefDockPanel;

            gvc.HeaderTemplate = dt;
        }

        #endregion

        #region Helper methods

        public static T GetAncestor<T>(DependencyObject reference) where T : DependencyObject
        {
            T targetParent = null;
            do
            {
                reference = VisualTreeHelper.GetParent(reference);
                targetParent = reference as T;
            } while (targetParent == null);
            return targetParent;
        }

        public static void ApplySort(ICollectionView view, string propertyName)
        {
            //ListSortDirection direction = ListSortDirection.Ascending;
            //if (view.SortDescriptions.FirstOrDefault(currentSort => currentSort.PropertyName == propertyName))
            //{
            //    SortDescription currentSort = view.SortDescriptions[2];
            //    if (currentSort.PropertyName == propertyName)
            //    {
            //        direction = (currentSort.Direction == ListSortDirection.Ascending) ?
            //            ListSortDirection.Descending : ListSortDirection.Ascending;
            //    }
            //    view.SortDescriptions.Clear();
            //}
            //if (!string.IsNullOrEmpty(propertyName))
            //{
            //    view.SortDescriptions.Add(new SortDescription(propertyName, direction));
            //}
        }

        #endregion
    }
}