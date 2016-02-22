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
    class ObservableDirectory : ObservableCollection<FileSystemViewModel>, IDisposable
    {
        DirectoryInfo _di;
        FileSystemWatcher _fsw;
        object _locker = new object();

        public ObservableDirectory(DirectoryInfo di) :
            base(GetDirectoryItems(di))
        {
            if (di.Parent != null)
                Add(new FileSystemViewModel(di.Parent, true));
            BindingOperations.EnableCollectionSynchronization(this, _locker);

            _di = di;
            _fsw = new FileSystemWatcher(di.FullName)
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = false
            };

            _fsw.Changed += (_, e) =>
            {
                Remove(new FileSystemViewModel(e.FullPath));
                Add(new FileSystemViewModel(e.FullPath));
            };
            _fsw.Renamed += (_, e) =>
            {
                Remove(new FileSystemViewModel(e.OldFullPath));
                Add(new FileSystemViewModel(e.FullPath));
            };
            _fsw.Created += (_, e) =>
            {
                if (!this.Any(item => item.FullName == e.FullPath))
                    Add(new FileSystemViewModel(e.FullPath));
            };
            _fsw.Deleted += (_, e) => Remove(new FileSystemViewModel(e.FullPath));
        }

        static IEnumerable<FileSystemViewModel> GetDirectoryItems(DirectoryInfo di)
        {
            return di.EnumerateFileSystemInfos().
                Where(item => !item.Attributes.HasFlag(FileAttributes.ReparsePoint)).
                Select(item => new FileSystemViewModel(item));
        }

        public void Dispose()
        {
            _fsw.Dispose();
        }
    }
}
