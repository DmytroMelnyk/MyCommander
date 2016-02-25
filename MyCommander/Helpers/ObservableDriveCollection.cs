using MyCommander.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace MyCommander.Helpers
{
    public class ObservableDriveCollection : ObservableCollection<DriveViewModel>, IDisposable
    {
        private SystemDriveWatcher sdw = new SystemDriveWatcher();
        private object locker = new object();

        public ObservableDriveCollection()
            : base(DriveViewModel.GetDrives())
        {
            BindingOperations.EnableCollectionSynchronization(this, this.locker);
            this.sdw.DriveConnected += (s, e) =>
            {
                this.Add(new DriveViewModel(e.DriveName));
            };
            this.sdw.DriveDisconnected += (s, e) =>
            {
                this.Remove(new DriveViewModel(e.DriveName));
            };
            this.sdw.Start();
        }

        public int BinarySearch(DriveViewModel item)
        {
            return ((List<DriveViewModel>)this.Items).BinarySearch(item);
        }

        public new void Add(DriveViewModel item)
        {
            int position = this.BinarySearch(item);
            if (position < 0)
            {
                position = ~position;
            }

            this.Insert(position, item);
        }

        public void Dispose()
        {
            this.sdw.Stop();
            this.sdw.Dispose();
        }
    }
}
