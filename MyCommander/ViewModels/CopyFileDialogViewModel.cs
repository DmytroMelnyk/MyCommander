using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyCommander.ViewModels
{
    public class CopyFileDialogViewModel : ViewModelBase
    {
        private ICommand confirmCopyCommand;
        private string targetPath;

        public CopyFileDialogViewModel(string sourceFileName, string targetPath)
        {
            this.SourceFileName = sourceFileName;
            this.TargetPath = targetPath;
        }

        public string TargetPath
        {
            get { return this.targetPath; }
            set { this.Set(ref this.targetPath, value); }
        }

        public string SourceFileName { get; private set; }

        public ICommand ConfirmCopyCommand
        {
            get
            {
                return this.confirmCopyCommand ?? (this.confirmCopyCommand = new DelegateCommand(() =>
                {
                    // DialogService.Instance.ShowDialog()
                }));
            }
        }

        protected override IEnumerable<string> GetCustomErrorMessages<T>(string propertyName, T propertyValue)
        {
            return null;
        }
    }
}
