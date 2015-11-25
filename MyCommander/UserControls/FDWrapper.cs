using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCommander.UserControls
{
    class FileSystemInfoWrapper : IEquatable<FileSystemInfoWrapper>
    {
        public FileSystemInfoWrapper(string path, bool? isCurrentDirectory = null)
        {
            FullName = Path.GetFullPath(path);
            _isCurrentDirectory = isCurrentDirectory;
        }

        public FileSystemInfoWrapper(FileSystemInfo fsi, bool? isCurrentDirectory = null) :
            this(fsi.FullName, isCurrentDirectory) { }

        public string FullName
        {
            get;
            private set;
        }

        bool? _isCurrentDirectory;
        public bool? IsCurrentDirectory
        {
            get
            {
                if (IsDirectory)
                    return _isCurrentDirectory;
                return null;
            }
        }

        FileSystemInfo _fsi;
        FileSystemInfo FileSystemInfo
        {
            get
            {
                if (_fsi == null)
                {
                    if (IsDirectory)
                        _fsi = new DirectoryInfo(FullName);
                    else
                        _fsi = new FileInfo(FullName);
                }
                return _fsi;
            }
        }

        public bool IsDirectory
        {
            get { return File.GetAttributes(FullName).HasFlag(FileAttributes.Directory); }
        }

        public string Name
        {
            get { return FileSystemInfo.Name; } 
        }

        public string Extension
        {
            get { return FileSystemInfo.Extension; } 
        }

        public long? Length 
        {
            get
            {
                if (IsDirectory)
                    return null;
                return ((FileInfo)FileSystemInfo).Length;
            } 
        }

        public DateTime CreationTime
        {
            get { return FileSystemInfo.CreationTime; }
        }

        public FileAttributes Attributes 
        {
            get { return FileSystemInfo.Attributes; } 
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            return Equals((FileSystemInfoWrapper)obj);
        }

        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }

        public bool Equals(FileSystemInfoWrapper other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return FullName.Equals(other.FullName);
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}
