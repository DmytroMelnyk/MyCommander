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
        public Presenter(string currentDirectory)
        {
            CurrentDirectory = currentDirectory;
        }

        ObservableCollection<DriveInfo> drives = new ObservableCollection<DriveInfo>(DriveInfo.GetDrives());

        public ObservableCollection<DriveInfo> Drives
        {
            get { return drives; }
        }

        ObservableDirectory _FDICollection;
        public ObservableDirectory FDICollection
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

        string _currentDirectory;
        public string CurrentDirectory 
        {
            get { return _currentDirectory; }
            set
            {
                if (_currentDirectory != value && Directory.Exists(value))
                {
                    _currentDirectory = value;
                    if (FDICollection != null)
                        FDICollection.Dispose();
                    FDICollection = new ObservableDirectory(new DirectoryInfo(_currentDirectory));
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public void Dispose()
        {
            if (_FDICollection != null)
                _FDICollection.Dispose();
        }
    }
}
