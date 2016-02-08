using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyCommanderNew
{
    public class ViewModel : INotifyPropertyChanged
    {
        static ObservableCollection<DriveInfo> _Drives = new ObservableCollection<DriveInfo>(DriveInfo.GetDrives());
        public ObservableCollection<DriveInfo> Drives
        {
            get { return _Drives; }
            set { Set(ref _Drives, value); }
        }

        DelegateCommand<DriveInfo> _ChangeDiskCommand;
        public DelegateCommand<DriveInfo> ChangeDiskCommand
        {
            get
            {
                return _ChangeDiskCommand ?? (_ChangeDiskCommand = new DelegateCommand<DriveInfo>
                    (
                        Foo,
                        parameter => parameter.IsReady
                    ));
            }
        }

        void Foo(DriveInfo di)
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
