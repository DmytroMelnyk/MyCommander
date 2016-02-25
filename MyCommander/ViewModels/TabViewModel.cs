using Microsoft.Practices.Prism.Commands;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using MyCommander.Validators;
using MyCommander.Helpers;

namespace MyCommander.UserControls
{
    internal class TabViewModel : ViewModelBase, IDisposable
    {
        private ObservableDriveCollection drives = new ObservableDriveCollection();
        private ObservableDirectory fDICollection;
        private FileSystemViewModel selectedItem;
        private string currentDirectory;
        private DelegateCommand doubleClickCommand;
        private DriveViewModel currentDisk;
        private DelegateCommand<DriveViewModel> changeDiskCommand;

        public TabViewModel()
        {
            this.CurrentDirectory = this.Drives.First(drive => drive.IsReady).Name;
        }

        public ObservableDriveCollection Drives
        {
            get { return this.drives; }
        }

        public ObservableDirectory FDICollection
        {
            get { return this.fDICollection; }
            set { this.Set(ref this.fDICollection, value); }
        }

        public FileSystemViewModel SelectedItem
        {
            get { return this.selectedItem; }
            set { this.Set(ref this.selectedItem, value); }
        }

        [DirectoryExist]
        public string CurrentDirectory
        {
            get
            {
                return this.currentDirectory;
            }

            set
            {
                if (this.ValidateProperty(value, notifyErrorChanged: false))
                {
                    this.Set(ref this.currentDirectory, value, validateProperty: false);
                    if (this.CurrentDisk != null)
                    {
                        this.CurrentDisk.IsCurrentDrive = false;
                    }

                    int idx = this.Drives.BinarySearch(new DriveViewModel(this.CurrentDirectory));
                    var curDrive = this.Drives[idx];
                    curDrive.IsCurrentDrive = true;
                    this.CurrentDisk = curDrive;
                    this.FDICollection?.Dispose();
                    this.FDICollection = new ObservableDirectory(new DirectoryInfo(this.CurrentDirectory));
                }
            }
        }

        public DelegateCommand DoubleClickCommand
        {
            get
            {
                return this.doubleClickCommand ?? (this.doubleClickCommand = new DelegateCommand(
                        () =>
                        {
                            if (this.SelectedItem.IsDirectory)
                            {
                                this.CurrentDirectory = this.SelectedItem.FullName;
                            }
                            else
                            {
                                Process.Start(this.SelectedItem.FullName);
                            }
                        }));
            }
        }

        public DriveViewModel CurrentDisk
        {
            get { return this.currentDisk; }
            set { this.Set(ref this.currentDisk, value); }
        }

        public DelegateCommand<DriveViewModel> ChangeDiskCommand
        {
            get
            {
                return this.changeDiskCommand ?? (this.changeDiskCommand =
                    new DelegateCommand<DriveViewModel>(
                        parameter => this.CurrentDirectory = parameter.Name,
                        parameter => parameter.IsReady));
            }
        }

        public void Dispose()
        {
            this.Drives.Dispose();
            this.fDICollection?.Dispose();
        }
    }
}
