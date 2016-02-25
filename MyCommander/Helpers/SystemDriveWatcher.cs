using MyCommander.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management;
using System.Windows.Data;

namespace MyCommander.Helpers
{
    internal class SystemDriveWatcher : IDisposable
    {
        private const string WatcherConnectedQuery = "SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2";
        private const string WatcherDisconnectedQuery = "SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 3";
        private ManagementEventWatcher watcherConnected = new ManagementEventWatcher(new WqlEventQuery(WatcherConnectedQuery));
        private ManagementEventWatcher watcherDisconnected = new ManagementEventWatcher(new WqlEventQuery(WatcherDisconnectedQuery));

        public SystemDriveWatcher()
        {
            this.watcherConnected.EventArrived += (s, e) =>
            {
                this.DriveConnected?.Invoke(this, new DriveEventArgs(this.GetDriveName(e)));
            };
            this.watcherDisconnected.EventArrived += (s, e) =>
            {
                this.DriveDisconnected?.Invoke(this, new DriveEventArgs(this.GetDriveName(e)));
            };
        }

        public event EventHandler<DriveEventArgs> DriveConnected;

        public event EventHandler<DriveEventArgs> DriveDisconnected;

        public void Start()
        {
            this.watcherConnected.Start();
            this.watcherDisconnected.Start();
        }

        public void Dispose()
        {
            this.watcherConnected.Dispose();
            this.watcherDisconnected.Dispose();
        }

        public void Stop()
        {
            this.watcherConnected.Stop();
            this.watcherDisconnected.Stop();
        }

        private string GetDriveName(EventArrivedEventArgs e)
        {
            return e.NewEvent.Properties["DriveName"].Value.ToString();
        }
    }
}
