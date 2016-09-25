using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfDumper.Helpers;
using WpfDumper.Services;

namespace WpfDumper.ViewModel
{
    public class MainWindowViewModel : NotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            LoadCommands();
        }

        private void LoadCommands()
        {
            TickerCommand = new RelayCommand(OpenTicker, CanOpenTicker);
        }

        private bool CanOpenTicker(object obj)
        {
            return true;
        }

        private void OpenTicker(object obj)
        {
            dialogService.ShowDialog(DialogService.DIALOG.TICKERVIEW, new TickerViewModel());
        }

        DialogService dialogService = new DialogService();
        public ICommand TickerCommand
        {
            get;
            set;
        }

    }
}
