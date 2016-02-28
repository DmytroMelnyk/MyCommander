using MyCommander.ViewModels;
using MyCommander.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Data;
using System.Collections.Specialized;

namespace MyCommander
{
    internal class ObservableDirectory : ObservableCollection<FileSystemViewModel>, IDisposable
    {
        private DirectoryInfo di;
        private FileSystemWatcher fsw;
        private object locker = new object();

        public ObservableDirectory(DirectoryInfo di)
            : base(GetDirectoryItems(di))
        {
            if (di.Parent != null)
            {
                this.Add(FileSystemViewModel.GetFileSystemViewModel(di.Parent.FullName, true));
            }

            BindingOperations.EnableCollectionSynchronization(this, this.locker);

            this.di = di;
            this.fsw = new FileSystemWatcher(di.FullName)
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = false,
            };

            this.fsw.Changed += (_, e) =>
            {
                this.Items.First(item => item.FullName == e.FullPath).Update();
            };
            this.fsw.Renamed += (_, e) =>
            {
                this.Items.FirstOrDefault(item => item.FullName == e.OldFullPath)?.Rename(e.FullPath);
            };
            this.fsw.Created += (_, e) =>
            {
                if (!this.Any(item => item.FullName == e.FullPath))
                {
                    this.Add(FileSystemViewModel.GetFileSystemViewModel(e.FullPath));
                }
            };
            this.fsw.Deleted += (_, e) =>
            {
                int index = this.IndexOf(item => item.FullName == e.FullPath);
                if (index != -1)
                {
                    this.RemoveAt(index);
                }
            };
        }

        public void Dispose()
        {
            this.fsw.Dispose();
        }

        private static IEnumerable<FileSystemViewModel> GetDirectoryItems(DirectoryInfo di)
        {
            return di.EnumerateFileSystemInfos().
                Where(item => !item.Attributes.HasFlag(FileAttributes.ReparsePoint)).
                Select(item => FileSystemViewModel.GetFileSystemViewModel(item.FullName));
        }
    }
}
