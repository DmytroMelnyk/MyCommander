using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyCommander.ViewModels
{
    public class CopyFileProcessViewModel : ViewModelBase
    {
        private int progressValue;
        private int currentItemIndex;
        private ICommand cancelCommand;
        private string currentItem;
        private CancellationTokenSource cts = new CancellationTokenSource();
        private IProgress<double> singleItemHandler;
        private IProgress<int> itemCollectionHandler;

        public CopyFileProcessViewModel(string sourceItem, string targetPath, int totalItems)
        {
            this.TotalItems = totalItems;
            this.SourceItem = sourceItem;
            this.TargetPath = targetPath;
        }

        public int ProgressValue
        {
            get { return this.progressValue; }
            set { this.Set(ref this.progressValue, value); }
        }

        public int CurrentItemIndex
        {
            get { return this.currentItemIndex; }
            set { this.Set(ref this.currentItemIndex, value); }
        }

        public string CurrentItem
        {
            get { return this.currentItem; }
            set { this.Set(ref this.currentItem, value); }
        }

        public int TotalItems { get; private set; }

        public string TargetPath { get; private set; }

        public string SourceItem { get; private set; }

        public ICommand CancelCommand
        {
            get
            {
                return this.cancelCommand ?? (this.cancelCommand =
                    new DelegateCommand(() => this.cts.Cancel()));
            }
        }

        private IProgress<double> SingleItemHandler
        {
            get
            {
                return this.singleItemHandler ?? (this.singleItemHandler =
                    new Progress<double>(progressResult => this.ProgressValue = (int)(100 * progressResult)));
            }
        }

        private IProgress<int> ItemCollectionHandler
        {
            get
            {
                return this.itemCollectionHandler ?? (this.itemCollectionHandler =
                    new Progress<int>(item => this.CurrentItemIndex = item));
            }
        }


        public override async void OnLoaded(object sender, EventArgs e)
        {
            await CopyFileSystemItems(this.SourceItem, this.TargetPath, this.cts.Token, this.SingleItemHandler, this.ItemCollectionHandler);
        }

        protected override IEnumerable<string> GetCustomErrorMessages<T>(string propertyName, T propertyValue)
        {
            return null;
        }

        private static async Task CopyFileSystemItems(string sourceItem, string targetPath, CancellationToken ct, IProgress<double> singleItemProgress, IProgress<int> progress)
        {
            string sourceName;
            if (File.GetAttributes(sourceItem).HasFlag(FileAttributes.Directory))
            {
                sourceName = new DirectoryInfo(sourceItem).Name;
                foreach (var currentItem in Directory.EnumerateFileSystemEntries(sourceItem, "*.*", SearchOption.AllDirectories).
                    Where(item => !File.GetAttributes(item).HasFlag(FileAttributes.Directory)))
                {
                    string resultItem = targetPath + sourceName + currentItem.Remove(0, sourceItem.Length);
                }

                return;
            }

            sourceName = new FileInfo(sourceItem).Name;
        }
    }
}
