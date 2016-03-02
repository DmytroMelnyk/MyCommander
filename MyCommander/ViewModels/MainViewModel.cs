using MyCommander.UserControls;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Input;
using System.Linq;

namespace MyCommander.ViewModels
{
    internal class MainViewModel : ViewModelBase, IDisposable
    {
        private TabViewModel tabModelView1 = new TabViewModel();
        private TabViewModel tabModelView2 = new TabViewModel();
        private TabViewModel activeTab;
        private ICommand disposeCommand;
        private ICommand copyCommand;

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

        public ICommand DisposeCommand
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

        protected override IEnumerable<string> GetCustomErrorMessages<T>(string propertyName, T propertyValue)
        {
            return null;
        }

        public ICommand CopyCommand
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
                                DialogService.Instance.ShowDialog(new CopyFileDialogViewModel(this.ActiveTab.SelectedItem.Name, this.NonActiveTab.CurrentDirectory));
                            }
                        }));
            }
        }
    }
}
