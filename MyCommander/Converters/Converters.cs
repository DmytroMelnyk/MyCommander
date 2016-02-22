using MyCommander.UserControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyCommander
{
    public class FileSystemInfoWrapperToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DriveViewModel)
                return FolderManager.GetImageSource(value.ToString(), ItemState.Undefined);

            FileSystemViewModel fsi = (FileSystemViewModel)value;
            return fsi.IsDirectory ? FolderManager.GetImageSource(fsi.FullName, ItemState.Undefined) :
                FileManager.GetImageSource(fsi.FullName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    static class FileAttributesExtension
    {
        public static string GetString(this FileAttributes value)
        {
            StringBuilder strbld = new StringBuilder();
            if (value.HasFlag(FileAttributes.Archive))
                strbld.Append("a");
            else
                strbld.Append("-");

            if (value.HasFlag(FileAttributes.ReadOnly))
                strbld.Append("r");
            else
                strbld.Append("-");

            if (value.HasFlag(FileAttributes.Hidden))
                strbld.Append("h");
            else
                strbld.Append("-");

            if (value.HasFlag(FileAttributes.System))
                strbld.Append("s");
            else
                strbld.Append("-");

            //if (value.HasFlag(FileAttributes.Compressed))
            //    strbld.Append("C");
            //if (value.HasFlag(FileAttributes.Device))
            //    strbld.Append("D");
            //if (value.HasFlag(FileAttributes.Directory))
            //    strbld.Append("Dir");
            //if (value.HasFlag(FileAttributes.Encrypted))
            //    strbld.Append("E");
            //if (value.HasFlag(FileAttributes.IntegrityStream))
            //    strbld.Append("IS");
            //if (value.HasFlag(FileAttributes.Normal))
            //    strbld.Append("N");
            //if (value.HasFlag(FileAttributes.NoScrubData))
            //    strbld.Append("NSD");
            //if (value.HasFlag(FileAttributes.NotContentIndexed))
            //    strbld.Append("NCI");
            //if (value.HasFlag(FileAttributes.Offline))
            //    strbld.Append("O");
            //if (value.HasFlag(FileAttributes.ReparsePoint))
            //    strbld.Append("RP");
            //if (value.HasFlag(FileAttributes.SparseFile))
            //    strbld.Append("SF");
            //if (value.HasFlag(FileAttributes.Temporary))
            //    strbld.Append("T");

            return strbld.ToString();
        }
    }

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
