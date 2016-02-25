using System.Windows.Media;
using System.Runtime.InteropServices;
using System;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.IO;
using System.Drawing;

namespace MyCommander
{

    public class ShellManager
    {
        public static Icon GetIcon(string path, ItemType type, IconSize size, ItemState state)
        {
            var flags = (uint)(Interop.SHGFIICON | Interop.SHGFIUSEFILEATTRIBUTES);
            var attribute = (uint)(object.Equals(type, ItemType.Folder) ? Interop.FILEATTRIBUTEDIRECTORY : Interop.FILEATTRIBUTEFILE);
            if (object.Equals(type, ItemType.Folder) && object.Equals(state, ItemState.Open))
            {
                flags += Interop.SHGFIOPENICON;
            }

            if (object.Equals(size, IconSize.Small))
            {
                flags += Interop.SHGFISMALLICON;
            }
            else
            {
                flags += Interop.SHGFILARGEICON;
            }

            var shfi = default(SHFileInfo);
            var res = Interop.SHGetFileInfo(path, attribute, out shfi, (uint)Marshal.SizeOf(shfi), flags);
            if (object.Equals(res, IntPtr.Zero))
            {
                throw Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            try
            {
                Icon.FromHandle(shfi.HIcon);
                return (Icon)Icon.FromHandle(shfi.HIcon).Clone();
            }
            catch
            {
                throw;
            }
            finally
            {
                Interop.DestroyIcon(shfi.HIcon);
            }
        }
    }
}