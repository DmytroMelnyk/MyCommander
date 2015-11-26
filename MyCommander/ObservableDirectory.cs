using MyCommander.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyCommander
{
    class ObservableDirectory : ObservableCollection<FileSystemInfoWrapper>, IDisposable
    {
        DirectoryInfo _di;
        FileSystemWatcher _fsw;
        object _locker = new object();

        public ObservableDirectory(DirectoryInfo di) :
            base(GetDirectoryItems(di))
        {
            if (di.Parent != null)
                this.Add(new FileSystemInfoWrapper(di.Parent, true));
            //BindingOperations.EnableCollectionSynchronization(this, _locker);

            _di = di;
            _fsw = new FileSystemWatcher(di.FullName)
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = false
            };

            _fsw.Changed += (_, e) =>
            {
                Remove(new FileSystemInfoWrapper(e.FullPath));
                Add(new FileSystemInfoWrapper(e.FullPath));
            };
            _fsw.Renamed += (_, e) =>
            {
                Remove(new FileSystemInfoWrapper(e.OldFullPath));
                Add(new FileSystemInfoWrapper(e.FullPath));
            };
            _fsw.Created += (_, e) =>
            {
                if (!this.Any(item => item.FullName == e.FullPath))
                    Add(new FileSystemInfoWrapper(e.FullPath));
            };
            _fsw.Deleted += (_, e) => Remove(new FileSystemInfoWrapper(e.FullPath));
        }

        static IEnumerable<FileSystemInfoWrapper> GetDirectoryItems(DirectoryInfo di)
        {
            return di.EnumerateFileSystemInfos().
                Where(item => !item.Attributes.HasFlag(FileAttributes.ReparsePoint)).
                Select(item => new FileSystemInfoWrapper(item));
        }

        public void Dispose()
        {
            _fsw.Dispose();
        }
    }
}
