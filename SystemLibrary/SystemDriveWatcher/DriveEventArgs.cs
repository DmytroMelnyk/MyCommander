using System;

namespace MyCommander.Helpers
{
    public class DriveEventArgs : EventArgs
    {
        public DriveEventArgs(string driveName)
        {
            this.DriveName = driveName;
        }

        public string DriveName { get; set; }
    }
}
