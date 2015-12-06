using MyCommander.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyCommander.ViewModels
{
    class MainViewModel : Notifier, IDisposable
    {
        TabModelView tabModelView1 = new TabModelView();
        TabModelView tabModelView2 = new TabModelView();

        public TabModelView TabViewModel1
        {
            get { return tabModelView1; }
        }

        public TabModelView TabViewModel2
        {
            get { return tabModelView2; }
        }

        public MainViewModel()
        {
            ActiveTab = tabModelView1;
        }

        TabModelView _ActiveTab;
        public TabModelView ActiveTab
        {
            get { return _ActiveTab; }
            set { Set(ref _ActiveTab, value); }
        }

        static async Task CopyFiles(string sourceFileName, string targetFileName, CancellationToken ct, IProgress<double> progress)
        {
            const int bufferSize = 4096;
            using (FileStream sourceStream = new FileStream(sourceFileName,
                FileMode.Open, FileAccess.Read, FileShare.Read,
                bufferSize, useAsync: true))
            using (FileStream targetStream = new FileStream(targetFileName,
                FileMode.CreateNew, FileAccess.Write, FileShare.None,
                bufferSize, useAsync: true))
            {
                double totalBytesRead = 0;
                byte[] buffer = new byte[bufferSize];
                int bytesRead = 0;
                while ((bytesRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length, ct)) != 0)
                {
                    totalBytesRead += bytesRead;
                    await targetStream.WriteAsync(buffer, 0, bytesRead, ct);
                    if (progress != null)
                        progress.Report(totalBytesRead / sourceStream.Length);
                }
            }
        }

        public void Dispose()
        {
            tabModelView1.Dispose();
            tabModelView2.Dispose();
        }
    }
}
