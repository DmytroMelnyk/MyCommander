using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MyCommander
{
    public static class FolderManager
    {
        public static ImageSource GetImageSource(string directory, ItemState folderType)
        {
            try
            {
                return GetImageSource(directory, new Size(16, 16), folderType);
            }
            catch
            {
                throw;
            }
        }

        public static ImageSource GetImageSource(string directory, Size size, ItemState folderType)
        {
            try
            {
                using (var icon = ShellManager.GetIcon(directory, ItemType.Folder, IconSize.Large, folderType))
                {
                    return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight((int)size.Width, (int)size.Height));
                }
            }
            catch
            {
                throw;
            }
        }
    }
}