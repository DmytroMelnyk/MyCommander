using MyCommander.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management;
using System.Windows.Data;

namespace MyCommander.Helpers
{
    internal class DriveEventArgs : EventArgs
    {
        public string DriveName { get; set; }

        public DriveEventArgs(string driveName)
        {
            DriveName = driveName;
        }
    }

    internal class SystemDriveWatcher : IDisposable
    {
        private const string watcherConnectedQuery = "SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2";
        private const string watcherDisconnectedQuery = "SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 3";

        public event EventHandler<DriveEventArgs> DriveConnected;
        public event EventHandler<DriveEventArgs> DriveDisconnected;

        ManagementEventWatcher _watcherConnected = new ManagementEventWatcher(new WqlEventQuery(watcherConnectedQuery));
        ManagementEventWatcher _watcherDisconnected = new ManagementEventWatcher(new WqlEventQuery(watcherDisconnectedQuery));

        public SystemDriveWatcher()
        {
            _watcherConnected.EventArrived += (s, e) =>
            {
                DriveConnected?.Invoke(this, new DriveEventArgs(GetDriveName(e)));
            };
            _watcherDisconnected.EventArrived += (s, e) =>
            {
                DriveDisconnected?.Invoke(this, new DriveEventArgs(GetDriveName(e)));
            };
        }

        public void Start()
        {
            _watcherConnected.Start();
            _watcherDisconnected.Start();
        }

        private string GetDriveName(EventArrivedEventArgs e)
        {
            return e.NewEvent.Properties["DriveName"].Value.ToString();
        }

        public void Dispose()
        {
            _watcherConnected.Dispose();
            _watcherDisconnected.Dispose();
        }

        public void Stop()
        {
            _watcherConnected.Stop();
            _watcherDisconnected.Stop();
        }
    }

    public class ObservableDriveCollection : ObservableCollection<DriveViewModel>, IDisposable
    {
        SystemDriveWatcher _sdw = new SystemDriveWatcher();
        object _locker = new object();

        public ObservableDriveCollection() : 
            base(DriveViewModel.GetDrives())
        {
            BindingOperations.EnableCollectionSynchronization(this, _locker);
            _sdw.DriveConnected += (s, e) =>
            {
                Add(new DriveViewModel(e.DriveName));
            };
            _sdw.DriveDisconnected += (s, e) =>
            {
                Remove(new DriveViewModel(e.DriveName));
            };
            _sdw.Start();
        }

        public int BinarySearch(DriveViewModel item)
        {
            return ((List<DriveViewModel>)this.Items).BinarySearch(item);
        }

        public new void Add(DriveViewModel item)
        {
            int position = this.BinarySearch(item);
            if (position < 0)
                position = ~position;
            this.Insert(position, item);
        }

        public void Dispose()
        {
            _sdw.Stop();
            _sdw.Dispose();
        }
    }
}
