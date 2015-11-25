using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyCommander.UserControls
{
    class Model : IDisposable
    {
        DirectoryInfo _di;
        FileSystemWatcher _fsw;

        public Model(string diName = @"C:\")
        {
            _di = new DirectoryInfo(diName);
            _fsw = new FileSystemWatcher(diName)
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = false,
                NotifyFilter = NotifyFilters.FileName
            };
        }

        public DirectoryInfo CurrentDirectory
        {
            get { return _di; }
            set { _di = value; }
        }

        public FileSystemWatcher CurrentDirectoryWatcher
        {
            get { return _fsw; }
        }

        public IEnumerable<FileSystemInfoWrapper> FDIs
        {
            get
            {
                var fdis = _di.EnumerateFileSystemInfos().
                    Where(item => !item.Attributes.HasFlag(FileAttributes.ReparsePoint)).
                    Select(item => new FileSystemInfoWrapper(item));

                return (CurrentDirectory.Parent == null) ? fdis :
                    Enumerable.Repeat(new FileSystemInfoWrapper(CurrentDirectory.Parent, true), 1).Concat(fdis);
            }
        }

        public void Dispose()
        {
            _fsw.Dispose();
        }
    }
}
