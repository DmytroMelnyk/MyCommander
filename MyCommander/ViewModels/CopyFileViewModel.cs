using System;
using System.Collections.Generic;

namespace MyCommander.ViewModels
{
    public class CopyFileViewModel : ViewModelBase
    {
        private string targetFileName;

        public CopyFileViewModel(string sourceFileName, string targetFileName)
        {
            this.SourceFileName = sourceFileName;
            this.TargetFileName = targetFileName;
        }

        public string TargetFileName
        {
            get { return this.targetFileName; }
            set { this.Set(ref this.targetFileName, value); }
        }

        public string SourceFileName { get; private set; }

        protected override IEnumerable<string> GetCustomErrorMessages<T>(string propertyName, T propertyValue)
        {
            return null;
        }
    }
}
