using System.Windows;

namespace MyCommander
{
    public interface IDialogService
    {
        MessageBoxResult ShowMessageBox(string content, string title, MessageBoxButton buttons);

        bool? ShowDialog(ViewModelBase viewModel);
    }
}
