﻿using System.Windows.Media;
using System.Runtime.InteropServices;
using System;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.IO;
using System.Drawing;

namespace MyCommander
{

    public static class FolderManager
    {
        public static ImageSource GetImageSource(string directory, ItemState folderType)
        {
            try
            {
                return FolderManager.GetImageSource(directory, new Size(16, 16), folderType);
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