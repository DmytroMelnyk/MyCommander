using MyCommander.DialogServices;
using System.Windows;

namespace MyCommander
{
    public class DialogService : IDialogService
    {
        private static IDialogService instance = new DialogService();

        public static IDialogService Instance
        {
            get { return instance; }
        }

        public bool? ShowDialog(ViewModelBase viewModel)
        {
            return new DialogView
            {
                Owner = Application.Current.MainWindow,
                DataContext = viewModel,
                ShowInTaskbar = false
            }.ShowDialog();
        }

        public void Show(ViewModelBase viewModel)
        {
            var dv = new DialogView
            {
                DataContext = viewModel
            };
            viewModel.CloseCommand = new DelegateCommand(dv.Close);
            dv.Show();
        }

        public MessageBoxResult ShowMessageBox(string content, string title, MessageBoxButton buttons)
        {
            return MessageBox.Show(content, title, buttons);
        }
    }
}
