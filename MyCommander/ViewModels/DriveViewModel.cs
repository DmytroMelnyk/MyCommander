using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCommander.UserControls
{
    public class DriveViewModel : ViewModelBase, IEquatable<DriveViewModel>, IComparable<DriveViewModel>, IComparable
    {
        public DriveViewModel(string driveName, bool isCurrentDrive = false)
        {
            _di = new DriveInfo(driveName);
            _isCurrentDrive = isCurrentDrive;
        }

        DriveInfo _di;
        public DriveInfo DriveInfo
        {
            get { return _di; }
        }

        bool _isCurrentDrive;
        public bool IsCurrentDrive
        {
            get { return _isCurrentDrive; }
            set { Set(ref _isCurrentDrive, value); }
        }

        public string Name
        {
            get { return DriveInfo.Name; }
        }

        public bool IsReady
        {
            get { return DriveInfo.IsReady; }
        }

        public static List<DriveViewModel> GetDrives()
        {
            return DriveInfo.GetDrives().Select(item => new DriveViewModel(item.Name)).ToList();
        }

        public long TotalFreeSpace
        {
            get
            {
                return DriveInfo.IsReady ? DriveInfo.TotalFreeSpace : 0;
            }
        }

        public long TotalSize
        {
            get
            {
                return DriveInfo.IsReady ? DriveInfo.TotalSize : 0;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            return Equals((DriveViewModel)obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }

        public bool Equals(DriveViewModel other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Name.Equals(other.Name);
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(obj, null))
                return 1;

            if (ReferenceEquals(this, obj))
                return 0;

            if (this.GetType() != obj.GetType())
                throw new ArgumentException("Object is not a " + nameof(DriveViewModel));

            return CompareTo((DriveViewModel)obj);
        }

        public int CompareTo(DriveViewModel other)
        {
            if (ReferenceEquals(other, null))
                return 1;

            if (ReferenceEquals(this, other))
                return 0;

            return Name.CompareTo(other.Name);
        }
    }
}
