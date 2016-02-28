using MyCommander.UserControls;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using System.Linq;

namespace MyCommander.ViewModels
{
    internal class MainViewModel : ViewModelBase, IDisposable
    {
        private TabViewModel tabModelView1 = new TabViewModel();
        private TabViewModel tabModelView2 = new TabViewModel();
        private TabViewModel activeTab;
        private DelegateCommand disposeCommand;
        private DelegateCommand copyCommand;

        public MainViewModel()
        {
            this.ActiveTab = this.tabModelView1;
        }

        public TabViewModel TabViewModel1
        {
            get { return this.tabModelView1; }
        }

        public TabViewModel TabViewModel2
        {
            get { return this.tabModelView2; }
        }

        public TabViewModel ActiveTab
        {
            get { return this.activeTab; }
            set { this.Set(ref this.activeTab, value); }
        }

        public TabViewModel NonActiveTab
        {
            get { return (this.activeTab == this.TabViewModel1) ? this.TabViewModel2 : this.TabViewModel1; }
        }

        public DelegateCommand DisposeCommand
        {
            get
            {
                return this.disposeCommand ?? (this.disposeCommand =
                    new DelegateCommand(() => this.Dispose()));
            }
        }

        public void Dispose()
        {
            this.tabModelView1.Dispose();
            this.tabModelView2.Dispose();
        }

        public DelegateCommand CopyCommand
        {
            get
            {
                return this.copyCommand ?? (this.copyCommand =
                    new DelegateCommand(
                        () =>
                        {
                            if (this.ActiveTab.SelectedItem == null)
                            {
                                DialogService.Instance.ShowMessageBox("There are no selected files", "MyCommander", System.Windows.MessageBoxButton.OK);
                            }
                            else
                            {
                                DialogService.Instance.ShowDialog(new CopyFileViewModel(this.ActiveTab.SelectedItem.Name, this.NonActiveTab.CurrentDirectory));
                            }
                        }));
            }
        }

        private static async Task CopyFileSystemItems(string sourceItem, string targetDirectory, CancellationToken ct, IProgress<double> progress)
        {
            string sourceName;
            if (File.GetAttributes(sourceItem).HasFlag(FileAttributes.Directory))
            {
                sourceName = new DirectoryInfo(sourceItem).Name;
                foreach (var file in Directory.EnumerateFileSystemEntries(sourceItem, "*.*", SearchOption.AllDirectories).
                    Where(item => !File.GetAttributes(item).HasFlag(FileAttributes.Directory)))
                {
                    string resultPath = targetDirectory + sourceName + file.Remove(0, sourceItem.Length);
                }

                return;
            }

            sourceName = new FileInfo(sourceItem).Name;
        }

        private static async Task CopySingleFile(string sourceFileName, string targetFileName, CancellationToken ct, IProgress<double> progress)
        {
            const int bufferSize = 4096;
            using (FileStream sourceStream =
                new FileStream(sourceFileName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, useAsync: true))
            using (FileStream targetStream =
                new FileStream(targetFileName, FileMode.CreateNew, FileAccess.Write, FileShare.None, bufferSize, useAsync: true))
            {
                double totalBytesRead = 0;
                byte[] buffer = new byte[bufferSize];
                int bytesRead = 0;
                while ((bytesRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length, ct)) != 0)
                {
                    totalBytesRead += bytesRead;
                    await targetStream.WriteAsync(buffer, 0, bytesRead, ct);
                    if (progress != null)
                    {
                        progress.Report(totalBytesRead / sourceStream.Length);
                    }
                }
            }
        }
    }
}
