using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MyCommander.ViewModels
{
    internal abstract class FileSystemViewModel : IEquatable<FileSystemViewModel>, INotifyPropertyChanged
    {
        public string FullName
        {
            get;
            protected set;
        }

        public abstract bool? IsCurrentDirectory { get; }

        public abstract bool IsDirectory { get; }

        public string Name
        {
            get { return this.FileSystemInfo.Name; }
        }

        public string Extension
        {
            get { return this.FileSystemInfo.Extension; }
        }

        public abstract long? Length { get; }

        public DateTime CreationTime
        {
            get { return this.FileSystemInfo.CreationTime; }
        }

        public FileAttributes Attributes
        {
            get { return this.FileSystemInfo.Attributes; }
        }

        protected abstract FileSystemInfo FileSystemInfo { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        public abstract void Rename(string fullPath);

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

        public abstract ImageSource Icon { get; }

        public abstract Task CopyTo(DirectoryViewModel targetDirectory, CancellationToken ct, IProgress<double> progress);

        public static FileSystemViewModel GetFileSystemViewModel(string path, bool isCurrentDirectory = false)
        {
            string fullPath = Path.GetFullPath(path);
            if (File.GetAttributes(fullPath).HasFlag(FileAttributes.Directory))
            {
                return new DirectoryViewModel(fullPath, isCurrentDirectory);
            }

            return new FileViewModel(fullPath);
        }

        public abstract void Update();
    }
}
