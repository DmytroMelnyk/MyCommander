using System.IO;
using System.Text;

namespace MyCommander
{
    internal static class FileAttributesExtension
    {
        public static string GetString(this FileAttributes value)
        {
            StringBuilder strbld = new StringBuilder();
            if (value.HasFlag(FileAttributes.Archive))
            {
                strbld.Append("a");
            }
            else
            {
                strbld.Append("-");
            }

            if (value.HasFlag(FileAttributes.ReadOnly))
            {
                strbld.Append("r");
            }
            else
            {
                strbld.Append("-");
            }

            if (value.HasFlag(FileAttributes.Hidden))
            {
                strbld.Append("h");
            }
            else
            {
                strbld.Append("-");
            }

            if (value.HasFlag(FileAttributes.System))
            {
                strbld.Append("s");
            }
            else
            {
                strbld.Append("-");
            }

            return strbld.ToString();
        }
    }
}
