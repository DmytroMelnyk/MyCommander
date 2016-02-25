using MyCommander.UserControls;
using System;
using System.Windows.Data;

namespace MyCommander
{
    public class FileSystemInfoWrapperToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DriveViewModel)
            {
                return FolderManager.GetImageSource(value.ToString(), ItemState.Undefined);
            }

            FileSystemViewModel fsi = (FileSystemViewModel)value;
            return fsi.IsDirectory ? FolderManager.GetImageSource(fsi.FullName, ItemState.Undefined) :
                FileManager.GetImageSource(fsi.FullName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
