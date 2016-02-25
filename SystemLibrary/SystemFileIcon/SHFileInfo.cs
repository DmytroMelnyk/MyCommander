using System;
using System.Runtime.InteropServices;

namespace MyCommander
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct SHFileInfo
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
