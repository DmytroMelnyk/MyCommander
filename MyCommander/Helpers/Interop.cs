using System.Windows.Media;
using System.Runtime.InteropServices;
using System;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.IO;
using System.Drawing;

namespace MyCommander
{
    public enum ItemType : short
    {
        Folder,
        File
    }

    public enum IconSize : short
    {
        Small,
        Large
    }

    public enum ItemState : short
    {
        Undefined,
        Open,
        Close
    }

    public static class Interop
    {
        public const uint SHGFIICON = 0x000000100;
        public const uint SHGFIUSEFILEATTRIBUTES = 0x000000010;
        public const uint SHGFIOPENICON = 0x000000002;
        public const uint SHGFISMALLICON = 0x000000001;
        public const uint SHGFILARGEICON = 0x000000000;
        public const uint FILEATTRIBUTEDIRECTORY = 0x00000010;
        public const uint FILEATTRIBUTEFILE = 0x00000100;

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, out SHFileInfo psfi, uint cbFileInfo, uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyIcon(IntPtr hIcon);
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct SHFileInfo
    {
        public IntPtr HIcon;

        public int IIcon;

        public uint DwAttributes;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string SzDisplayName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string SzTypeName;
    }
}