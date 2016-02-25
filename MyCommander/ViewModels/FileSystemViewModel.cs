using System;
using System.IO;

namespace MyCommander.UserControls
{
    internal class FileSystemViewModel : IEquatable<FileSystemViewModel>
    {
        private FileSystemInfo fsi;
        private bool? isCurrentDirectory;

        public FileSystemViewModel(string path, bool? isCurrentDirectory = null)
        {
            this.FullName = Path.GetFullPath(path);
            this.isCurrentDirectory = isCurrentDirectory;
        }

        public FileSystemViewModel(FileSystemInfo fsi, bool? isCurrentDirectory = null)
            : this(fsi.FullName, isCurrentDirectory)
        {
        }

        public string FullName
        {
            get;
            private set;
        }

        public bool? IsCurrentDirectory
        {
            get
            {
                if (this.IsDirectory)
                {
                    return this.isCurrentDirectory;
                }

                return null;
            }
        }

        public bool IsDirectory
        {
            get { return File.GetAttributes(this.FullName).HasFlag(FileAttributes.Directory); }
        }

        public string Name
        {
            get { return this.FileSystemInfo.Name; }
        }

        public string Extension
        {
            get { return this.FileSystemInfo.Extension; }
        }

        public long? Length
        {
            get
            {
                if (this.IsDirectory)
                {
                    return null;
                }

                return ((FileInfo)this.FileSystemInfo).Length;
            }
        }

        public DateTime CreationTime
        {
            get { return this.FileSystemInfo.CreationTime; }
        }

        public FileAttributes Attributes
        {
            get { return this.FileSystemInfo.Attributes; }
        }

        private FileSystemInfo FileSystemInfo
        {
            get
            {
                if (this.fsi == null)
                {
                    if (this.IsDirectory)
                    {
                        this.fsi = new DirectoryInfo(this.FullName);
                    }
                    else
                    {
                        this.fsi = new FileInfo(this.FullName);
                    }
                }

                return this.fsi;
            }
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

            return this.Equals((FileSystemViewModel)obj);
        }

        public override int GetHashCode()
        {
            return this.FullName.GetHashCode();
        }

        public bool Equals(FileSystemViewModel other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.FullName.Equals(other.FullName);
        }

        public override string ToString()
        {
            return this.FullName;
        }
    }
}
