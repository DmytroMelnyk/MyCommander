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
    class TabViewModel : ViewModelBase, IDisposable
    {
        public TabViewModel()
        {
            string currentDirectory = Drives.First(drive => drive.IsReady).Name;
            CurrentDirectory = Path.GetFullPath(currentDirectory);

            foreach (var drive in Drives)
                drive.IsCurrentDrive = false;

            CurrentDisk = Drives.Single(item => item.Name == Path.GetPathRoot(CurrentDirectory));
            CurrentDisk.IsCurrentDrive = true;
        }

        static ObservableCollection<DriveViewModel> _Drives = new ObservableCollection<DriveViewModel>(DriveViewModel.GetDrives());
        public ObservableCollection<DriveViewModel> Drives
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

        FileSystemViewModel _SelectedItem;
        public FileSystemViewModel SelectedItem
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

        DriveViewModel _CurrentDisk;
        public DriveViewModel CurrentDisk
        {
            get { return _CurrentDisk; }
            set { Set(ref _CurrentDisk, value); }
        }

        DelegateCommand<DriveViewModel> _ChangeDiskCommand;
        public DelegateCommand<DriveViewModel> ChangeDiskCommand
        {
            get
            {
                return _ChangeDiskCommand ?? (_ChangeDiskCommand = new DelegateCommand<DriveViewModel>
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
