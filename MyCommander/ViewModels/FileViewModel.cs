using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MyCommander.ViewModels
{
    internal sealed class FileViewModel : FileSystemViewModel
    {
        private const int BufferSize = 4096;
        private FileInfo fileInfo;
        private ImageSource imageSource;

        public FileViewModel(string fullName)
        {
            this.FullName = fullName;
            this.fileInfo = new FileInfo(this.FullName);
        }

        public override bool IsDirectory
        {
            get { return false; }
        }

        public override long? Length
        {
            get { return this.FileInfo.Length; }
        }

        public override bool? IsCurrentDirectory
        {
            get { return null; }
        }

        public override ImageSource Icon
        {
            get { return this.imageSource ?? (this.imageSource = FileManager.GetImageSource(this.FullName)); }
        }

        protected override FileSystemInfo FileSystemInfo
        {
            get { return this.FileInfo; }
        }

        private FileInfo FileInfo
        {
            get { return this.fileInfo; }
        }

        public override async Task CopyTo(DirectoryViewModel targetDirectory, CancellationToken ct, IProgress<double> progress)
        {
            var resultFileSystemFullName = Path.Combine(targetDirectory.FullName, this.Name);
            using (FileStream sourceStream =
                new FileStream(this.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize, useAsync: true))
            using (FileStream targetStream =
                new FileStream(resultFileSystemFullName, FileMode.CreateNew, FileAccess.Write, FileShare.None, BufferSize, useAsync: true))
            {
                double totalBytesRead = 0;
                byte[] buffer = new byte[BufferSize];
                int bytesRead = 0;
                while ((bytesRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length, ct)) != 0)
                {
                    totalBytesRead += bytesRead;
                    await targetStream.WriteAsync(buffer, 0, bytesRead, ct);
                    progress?.Report(totalBytesRead / sourceStream.Length);
                }
            }
        }

        public override void Update()
        {
            this.fileInfo = new FileInfo(this.FullName);
            this.OnPropertyChanged(nameof(this.Length));
            this.OnPropertyChanged(nameof(this.Attributes));
        }

        public override void Rename(string fullPath)
        {
            this.fileInfo = new FileInfo(fullPath);
            this.FullName = FileInfo.FullName;
            this.imageSource = FileManager.GetImageSource(this.FullName);
            this.OnPropertyChanged(nameof(this.FullName));
            this.OnPropertyChanged(nameof(this.Extension));
            this.OnPropertyChanged(nameof(this.Name));
            this.OnPropertyChanged(nameof(this.Icon));
        }
    }
}
