using System.Runtime.InteropServices;
using System;
using System.Drawing;

namespace MyCommander
{






    internal class ShellManager
    {
        public static Icon GetIcon(string path, ItemType type, IconSize size, ItemState state)
        {
            var flags = Interop.SHGFIICON | Interop.SHGFIUSEFILEATTRIBUTES;
            var attribute = Equals(type, ItemType.Folder) ? Interop.FILEATTRIBUTEDIRECTORY : Interop.FILEATTRIBUTEFILE;
            if (Equals(type, ItemType.Folder) && Equals(state, ItemState.Open))
            {
                flags += Interop.SHGFIOPENICON;
            }

            if (Equals(size, IconSize.Small))
            {
                flags += Interop.SHGFISMALLICON;
            }
            else
            {
                flags += Interop.SHGFILARGEICON;
            }

            var shfi = default(SHFileInfo);
            var res = Interop.SHGetFileInfo(path, attribute, out shfi, (uint)Marshal.SizeOf(shfi), flags);
            if (Equals(res, IntPtr.Zero))
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