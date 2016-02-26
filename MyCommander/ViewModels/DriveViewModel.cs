using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyCommander.UserControls
{
    public class DriveViewModel : ViewModelBase, IEquatable<DriveViewModel>, IComparable<DriveViewModel>, IComparable
    {
        private DriveInfo di;
        private bool isCurrentDrive;

        public DriveViewModel(string driveName, bool isCurrentDrive = false)
        {
            this.di = new DriveInfo(driveName);
            this.isCurrentDrive = isCurrentDrive;
        }

        public DriveInfo DriveInfo
        {
            get { return this.di; }
        }

        public bool IsCurrentDrive
        {
            get { return this.isCurrentDrive; }
            set { this.Set(ref this.isCurrentDrive, value); }
        }

        public string Name
        {
            get { return this.DriveInfo.Name; }
        }

        public bool IsReady
        {
            get { return this.DriveInfo.IsReady; }
        }

        public long TotalFreeSpace
        {
            get
            {
                return this.DriveInfo.IsReady ? this.DriveInfo.TotalFreeSpace : 0;
            }
        }

        public long TotalSize
        {
            get
            {
                return this.DriveInfo.IsReady ? this.DriveInfo.TotalSize : 0;
            }
        }

        public static List<DriveViewModel> GetDrives()
        {
            return DriveInfo.GetDrives().Select(item => new DriveViewModel(item.Name)).ToList();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            return this.Equals((DriveViewModel)obj);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public override string ToString()
        {
            return this.Name;
        }

        public bool Equals(DriveViewModel other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Name.Equals(other.Name);
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return 1;
            }

            if (ReferenceEquals(this, obj))
            {
                return 0;
            }

            if (this.GetType() != obj.GetType())
            {
                throw new ArgumentException("Object is not a " + nameof(DriveViewModel));
            }

            return this.CompareTo((DriveViewModel)obj);
        }

        public int CompareTo(DriveViewModel other)
        {
            if (ReferenceEquals(other, null))
            {
                return 1;
            }

            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            return this.Name.CompareTo(other.Name);
        }

        protected override IEnumerable<string> GetCustomErrorMessages<T>(string propertyName, T propertyValue)
        {
            return null;
        }
    }
}
