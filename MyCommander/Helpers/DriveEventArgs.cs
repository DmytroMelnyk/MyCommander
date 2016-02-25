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
        public DriveEventArgs(string driveName)
        {
            this.DriveName = driveName;
        }

        public string DriveName { get; set; }
    }
}
