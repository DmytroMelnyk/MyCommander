using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MyCommander.UserControls
{
    class TabModelView : Notifier, IDisposable
    {
        public TabModelView()
        {
            string currentDirectory = Drives.First(drive => drive.IsReady).Name;
            CurrentDirectory = Path.GetFullPath(currentDirectory);
            CurrentDisk = new DriveInfo(Path.GetPathRoot(CurrentDirectory));
        }

        static ObservableCollection<DriveInfo> _Drives = new ObservableCollection<DriveInfo>(DriveInfo.GetDrives());
        public ObservableCollection<DriveInfo> Drives
        {
            get { return _Drives; }
            set { Set(ref _Drives, value); }
        }

        ObservableDirectory _FDICollection;
        public ObservableDirectory FDICollection
        {
            get { return _FDICollection; }
            set { Set(ref _FDICollection, value); }
        }

        FileSystemInfoWrapper _SelectedItem;
        public FileSystemInfoWrapper SelectedItem
        {
            get { return _SelectedItem; }
            set { Set(ref _SelectedItem, value); }
        }

        string _CurrentDirectory;
        public string CurrentDirectory
        {
            get { return _CurrentDirectory; }
            set
            {
                if (_CurrentDirectory != value && Directory.Exists(value))
                {
                    Set(ref _CurrentDirectory, value);
                    if (FDICollection != null)
                        FDICollection.Dispose();
                    FDICollection = new ObservableDirectory(new DirectoryInfo(_CurrentDirectory));
                }
            }
        }

        DelegateCommand _DoubleClickCommand;
        public DelegateCommand DoubleClickCommand
        {
            get
            {
                return _DoubleClickCommand ?? (_DoubleClickCommand = new DelegateCommand
                    (
                        () =>
                        {
                            if (SelectedItem.IsDirectory)
                                CurrentDirectory = SelectedItem.FullName;
                            else
                                Process.Start(SelectedItem.FullName);
                        }
                    ));
            }
        }

        DriveInfo _CurrentDisk;
        public DriveInfo CurrentDisk
        {
            get { return _CurrentDisk; }
            set { Set(ref _CurrentDisk, value); }
        }

        DelegateCommand<DriveInfo> _ChangeDiskCommand;
        public DelegateCommand<DriveInfo> ChangeDiskCommand
        {
            get
            {
                return _ChangeDiskCommand ?? (_ChangeDiskCommand = new DelegateCommand<DriveInfo>
                    (
                        parameter => CurrentDirectory = parameter.Name,
                        parameter => parameter.IsReady
                    ));
            }
        }

        public void Dispose()
        {
            if (_FDICollection != null)
                _FDICollection.Dispose();
        }
    }
}
