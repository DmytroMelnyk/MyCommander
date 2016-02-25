using System;
using System.IO;
using System.Windows.Data;

namespace MyCommander
{

    public class AttributeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            FileAttributes fA = (FileAttributes)value;
            return fA.GetString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
