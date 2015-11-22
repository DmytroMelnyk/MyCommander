using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MyCommander.UserControls
{
    class Presenter : INotifyPropertyChanged, IDisposable
    {
        ObservableCollection<FileSystemInfoWrapper> drives = new ObservableCollection<FileSystemInfoWrapper>(DriveInfo.GetDrives().Select(item => new FileSystemInfoWrapper(item.Name)));

        public ObservableCollection<FileSystemInfoWrapper> Drives
        {
            get { return drives; }
        }

        ObservableCollection<FileSystemInfoWrapper> _FDICollection;
        public ObservableCollection<FileSystemInfoWrapper> FDICollection
        {
            get { return _FDICollection; }
            set
            {
                if (_FDICollection != value)
                {
                    _FDICollection = value;
                    OnPropertyChanged();
                }
            }
        }

        Model _model;
        Model Model
        {
            get { return _model; }
            set
            {
                if (_model == value)
                    return;

                if (_model != null)
                    _model.Dispose();

                _model = value;
                FDICollection = new ObservableCollection<FileSystemInfoWrapper>(_model.FDIs);
                BindingOperations.EnableCollectionSynchronization(FDICollection, _locker);
                CurrentDirectory = _model.CurrentDirectory.FullName;
                WeakEventManager<FileSystemWatcher, FileSystemEventArgs>.AddHandler(_model.CurrentDirectoryWatcher, "Created", fsw_Created);
                WeakEventManager<FileSystemWatcher, FileSystemEventArgs>.AddHandler(_model.CurrentDirectoryWatcher, "Changed", fsw_Changed);
                WeakEventManager<FileSystemWatcher, FileSystemEventArgs>.AddHandler(_model.CurrentDirectoryWatcher, "Deleted", fsw_Deleted);
                WeakEventManager<FileSystemWatcher, RenamedEventArgs>.AddHandler(_model.CurrentDirectoryWatcher, "Renamed", fsw_Renamed);
            }
        }

        private object _locker = new object();

        string _currentDirectory;
        public string CurrentDirectory 
        {
            get { return _currentDirectory; }
            set
            {
                if (_currentDirectory != value && Directory.Exists(value))
                {
                    _currentDirectory = value;
                    Model = new Model(_currentDirectory);
                    OnPropertyChanged();
                }
            }
        }

        public Presenter(Model model)
        {
            Model = model;
        }

        void fsw_Renamed(object sender, RenamedEventArgs e)
        {
            FDICollection.Remove(new FileSystemInfoWrapper(e.OldFullPath));
            FDICollection.Add(new FileSystemInfoWrapper(e.FullPath));
        }

        void fsw_Deleted(object sender, FileSystemEventArgs e)
        {
            FDICollection.Remove(new FileSystemInfoWrapper(e.FullPath));
        }

        void fsw_Created(object sender, FileSystemEventArgs e)
        {
            FDICollection.Add(new FileSystemInfoWrapper(e.FullPath));
        }

        void fsw_Changed(object sender, FileSystemEventArgs e)
        {
            FDICollection.Remove(new FileSystemInfoWrapper(e.FullPath));
            FDICollection.Add(new FileSystemInfoWrapper(e.FullPath));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public void Dispose()
        {
            _model.Dispose();
        }
    }
}
