using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCommander.UserControls
{
    public class DriveViewModel : ViewModelBase
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

        public override string ToString()
        {
            return DriveInfo.ToString();
        }
    }
}
