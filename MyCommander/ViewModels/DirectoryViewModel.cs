using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MyCommander.ViewModels
{
    internal sealed class DirectoryViewModel : FileSystemViewModel
    {
        private bool isCurrentDirectory;
        private DirectoryInfo directoryInfo;
        private ImageSource imageSource;

        public DirectoryViewModel(string fullName, bool isCurrentDirectory)
        {
            this.FullName = fullName;
            this.isCurrentDirectory = isCurrentDirectory;
        }

        public override bool IsDirectory
        {
            get { return true; }
        }

        public override bool? IsCurrentDirectory
        {
            get { return this.isCurrentDirectory; }
        }

        public override long? Length
        {
            get { return null; }
        }

        public override ImageSource Icon
        {
            get { return this.imageSource ?? (this.imageSource = FolderManager.GetImageSource(this.FullName, ItemState.Undefined)); }
        }

        protected override FileSystemInfo FileSystemInfo
        {
            get { return this.DirectoryInfo; }
        }

        private DirectoryInfo DirectoryInfo
        {
            get { return this.directoryInfo ?? (this.directoryInfo = new DirectoryInfo(this.FullName)); }
        }

        public override async Task CopyTo(DirectoryViewModel targetDirectory, CancellationToken ct, IProgress<double> progress)
        {
            var resultFileSystemFullName = Path.Combine(targetDirectory.FullName, this.Name);
            Directory.CreateDirectory(resultFileSystemFullName);
        }

        public override void Update()
        {
        }

        public override void Rename(string fullPath)
        {
            directoryInfo = new DirectoryInfo(fullPath);
            FullName = directoryInfo.FullName;
            OnPropertyChanged(nameof(this.FullName));
            OnPropertyChanged(nameof(this.Name));
            OnPropertyChanged(nameof(this.Extension));
        }
    }
}
