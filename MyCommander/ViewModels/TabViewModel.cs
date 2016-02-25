using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using MyCommander.Validators;
using MyCommander.Helpers;

namespace MyCommander.UserControls
{
    class TabViewModel : ViewModelBase, IDisposable
    {
        public TabViewModel()
        {
            CurrentDirectory = Drives.First(drive => drive.IsReady).Name;
        }

        ObservableDriveCollection _Drives = new ObservableDriveCollection();
        public ObservableDriveCollection Drives
        {
            get { return _Drives; }
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

        [DirectoryExist]
        public string CurrentDirectory
        {
            get { return _CurrentDirectory; }
            set
            {
                if (ValidateProperty(value, notifyErrorChanged: false))
                {
                    Set(ref _CurrentDirectory, value, validateProperty: false);
                    if (CurrentDisk != null)
                    {
                        CurrentDisk.IsCurrentDrive = false;
                    }
                    int idx = Drives.BinarySearch(new DriveViewModel(CurrentDirectory));
                    var curDrive = Drives[idx];
                    curDrive.IsCurrentDrive = true;
                    CurrentDisk = curDrive;
                    FDICollection?.Dispose();
                    FDICollection = new ObservableDirectory(new DirectoryInfo(CurrentDirectory));
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
            Drives.Dispose();
            _FDICollection?.Dispose();
        }
    }
}
