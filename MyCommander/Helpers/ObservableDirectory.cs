using MyCommander.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Data;

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
                this.Add(new FileSystemViewModel(di.Parent, true));
            }

            BindingOperations.EnableCollectionSynchronization(this, this.locker);

            this.di = di;
            this.fsw = new FileSystemWatcher(di.FullName)
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = false
            };

            this.fsw.Changed += (_, e) =>
            {
                this.Remove(new FileSystemViewModel(e.FullPath));
                this.Add(new FileSystemViewModel(e.FullPath));
            };
            this.fsw.Renamed += (_, e) =>
            {
                this.Remove(new FileSystemViewModel(e.OldFullPath));
                this.Add(new FileSystemViewModel(e.FullPath));
            };
            this.fsw.Created += (_, e) =>
            {
                if (!this.Any(item => item.FullName == e.FullPath))
                {
                    this.Add(new FileSystemViewModel(e.FullPath));
                }
            };
            this.fsw.Deleted += (_, e) => this.Remove(new FileSystemViewModel(e.FullPath));
        }

        public void Dispose()
        {
            this.fsw.Dispose();
        }

        private static IEnumerable<FileSystemViewModel> GetDirectoryItems(DirectoryInfo di)
        {
            return di.EnumerateFileSystemInfos().
                Where(item => !item.Attributes.HasFlag(FileAttributes.ReparsePoint)).
                Select(item => new FileSystemViewModel(item));
        }
    }
}
