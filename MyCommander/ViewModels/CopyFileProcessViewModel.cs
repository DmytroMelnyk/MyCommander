using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCommander.ViewModels
{
    public class CopyFileProcessViewModel : ViewModelBase
    {
        CurProgressValue
            MaxProgressValue
            CancelCommand

        public CopyFileProcessViewModel()
        {

        }

        private static async Task CopyFileSystemItems(string sourceItem, string targetPath, CancellationToken ct, IProgress<double> progress)
        {
            string sourceName;
            if (File.GetAttributes(sourceItem).HasFlag(FileAttributes.Directory))
            {
                sourceName = new DirectoryInfo(sourceItem).Name;
                foreach (var currentItem in Directory.EnumerateFileSystemEntries(sourceItem, "*.*", SearchOption.AllDirectories).
                    Where(item => !File.GetAttributes(item).HasFlag(FileAttributes.Directory)))
                {
                    string resultItem = targetPath + sourceName + currentItem.Remove(0, sourceItem.Length);
                }

                return;
            }

            sourceName = new FileInfo(sourceItem).Name;
        }

        protected override IEnumerable<string> GetCustomErrorMessages<T>(string propertyName, T propertyValue)
        {
            return null;
        }
    }
}
