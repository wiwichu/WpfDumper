using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfDumper.Helpers;
using WpfDumper.View;

namespace WpfDumper.Services
{
    public class DialogService
    {
        public enum DIALOG
        {
            TICKERVIEW,
            TRADEVIEW
        }

        private static ConcurrentDictionary<DIALOG, Window> dialogMap = new ConcurrentDictionary<DIALOG, Window>();
        private Window view = null;
        public MessageBoxResult ShowMessageBox(string msg)
        {
            return MessageBox.Show(msg);
        }
        public void ShowDialog(DIALOG dialog, NotifyPropertyChanged viewModel)
        {
            switch (dialog)
            {
                case DIALOG.TICKERVIEW:
                    view = new TickerView();
                    if (viewModel != null)
                    {
                        view.DataContext = viewModel;
                    }
                    break;
                case DIALOG.TRADEVIEW:
                    view = new TradeView();
                    if (viewModel != null)
                    {
                        view.DataContext = viewModel;
                    }
                    break;
            }

            view.ShowDialog();
        }
        public void CloseDialog()
        {
            if (view != null)
            {
                view.Close();
            }
        }
    }
}
